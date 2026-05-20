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
using IAVH.BioTablero.CM.Core.Domain.Models.User;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

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
    private readonly IInitiativeRepository initiativeRepository;
    private readonly ITerritoryStoryRepository territoryStoryRepository;
    private readonly IResourceRepository resourceRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iamService">IAM service.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="territoryStoryRepository">Territory Story repository.</param>
    /// <param name="resourceRepository">Resource repository.</param>
    public UserService(
        IIamService iamService,
        IValidationErrorTranslator errorTranslator,
        IInitiativeRepository initiativeRepository,
        ITerritoryStoryRepository territoryStoryRepository,
        IResourceRepository resourceRepository)
    {
        this.iamService = iamService;
        this.errorTranslator = errorTranslator;
        this.initiativeRepository = initiativeRepository;
        this.territoryStoryRepository = territoryStoryRepository;
        this.resourceRepository = resourceRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetProfileDataAsync(string userName, CancellationToken ct = default)
    {
        var response = new UserProfile
        {
            Username = userName,
            TotalInitiatives = await initiativeRepository.GetEnabledRecordsCountAsync(userName, ct),
            TotalTerritoryStories = await territoryStoryRepository.GetEnabledRecordsCountAsync(userName, ct),
            TotalResources = await resourceRepository.GetPublishedRecordsCountAsync(userName, ct),
        };

        return new()
        {
            ResponseBody = response,
        };
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
