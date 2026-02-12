namespace IAVH.BioTablero.CM.Application.Services.Users;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using ODataUtilsCustom = IAVH.BioTablero.CM.Application.Utils.ODataUtils;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    private readonly IIamService iamService;

    private readonly IValidationErrorTranslator errorTranslator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iamService">IAM service.</param>
    /// <param name="errorTranslator">Error translator.</param>
    public UserService(
        IIamService iamService,
        IValidationErrorTranslator errorTranslator)
    {
        this.iamService = iamService;
        this.errorTranslator = errorTranslator;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<ExternalUser> queryOptions, CancellationToken ct = default)
    {
        try
        {
            // Get users data
            var users = await iamService.GetAllEnabledUsersDataAsync(ct);

            // Apply OData filters
            var query = users.AsQueryable();

            query = ODataUtilsCustom.AddOdataQueryFilterAndOrder(query, queryOptions);
            var totalItems = query.Count();
            query = ODataUtilsCustom.AddOdataQueryPagination(query, queryOptions);

            // Get result
            var dataList = queryOptions.ApplyTo(query) as IQueryable<ExternalUser>;

            var odataResponse = new ODataResponse<ExternalUser>()
            {
                TotalItems = totalItems,
                DataList = [.. dataList],
            };

            return new()
            {
                ResponseBody = new Dictionary<string, object>()
                {
                    ["@odata.count"] = odataResponse.TotalItems,
                    ["value"] = odataResponse.DataList,
                },
            };
        }
        catch (ODataException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter),
            };
        }
    }
}
