namespace IAVH.BioTablero.CM.Application.Services.Users;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    private readonly IIamService iamService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iamService">IAM service.</param>
    public UserService(
        IIamService iamService)
    {
        this.iamService = iamService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<ExternalUser> queryOptions, CancellationToken ct = default)
    {
        // Get users data
        var users = await iamService.GetAllEnabledUsersDataAsync(ct);

        // Apply OData filters
        var query = users.AsQueryable();

        query = ODataUtils.AddOdataQueryFilterAndOrder(query, queryOptions);
        query = ODataUtils.AddOdataQueryPagination(query, queryOptions);

        // Get result
        var dataList = queryOptions.ApplyTo(query) as IQueryable<ExternalUser>;

        var odataResponse = new ODataResponse<ExternalUser>()
        {
            TotalItems = users.Count(),
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
}
