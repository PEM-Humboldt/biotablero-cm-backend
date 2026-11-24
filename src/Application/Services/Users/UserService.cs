namespace IAVH.BioTablero.CM.Application.Services.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Users;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.User;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using ODataUtilsCustom = Utils.ODataUtils;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    private readonly IIamService iamService;
    private readonly IIamCustomApiService iamCustomApiService;
    private readonly IValidationErrorTranslator errorTranslator;
    private readonly ITerritoryStoryRepository territoryStoryRepository;
    private readonly IResourceRepository resourceRepository;
    private readonly IGeneralStatsRepository generalStatsRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="iamService">IAM service.</param>
    /// <param name="iamCustomApiService">IAM custom api service.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="territoryStoryRepository">Territory Story repository.</param>
    /// <param name="resourceRepository">Resource repository.</param>
    /// <param name="generalStatsRepository">General Statistics repository.</param>
    public UserService(
        IIamService iamService,
        IIamCustomApiService iamCustomApiService,
        IValidationErrorTranslator errorTranslator,
        ITerritoryStoryRepository territoryStoryRepository,
        IResourceRepository resourceRepository,
        IGeneralStatsRepository generalStatsRepository)
    {
        this.iamService = iamService;
        this.iamCustomApiService = iamCustomApiService;
        this.errorTranslator = errorTranslator;
        this.territoryStoryRepository = territoryStoryRepository;
        this.resourceRepository = resourceRepository;
        this.generalStatsRepository = generalStatsRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetProfileDataAsync(string? userName, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var response = new UserProfile
        {
            Username = userName,
            TotalInitiatives = await generalStatsRepository.GetEnabledRecordsCountAsync(userName: userName, ct: ct),
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
                DataList = dataList?.ToList() ?? [],
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

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetListByRolesAsync(string[] roles, bool include, string query, CancellationToken ct = default)
    {
        try
        {
            // Get users data
            var users = await iamCustomApiService.GetUsersDataByRolesAsync(roles, include, query, ct);

            var odataResponse = new ODataResponse<ExternalUser>()
            {
                DataList = users ?? [],
            };

            return new()
            {
                ResponseBody = new Dictionary<string, object>()
                {
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
        catch (HttpRequestException)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.IamCustomApiConnectionError),
            };
        }
    }
}
