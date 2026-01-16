namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using Serilog;

/// <summary>
/// Resource service.
/// </summary>
public class ResourceService : ServiceRead<Resource, ResourceDto, int>, IResourceService
{
    private new readonly IResourceRepository entityRepository;
    private readonly IValidator<ResourceDto> entityValidator;
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    public ResourceService(
        IResourceRepository entityRepository,
        IMapper<Resource, ResourceDto> mapper,
        IValidator<ResourceDto> entityValidator,
        ILogger logger)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity != null)
        {
            var userBelongsToInitiative = await entityRepository.UserRelationshipExistsAsync(id, userName, ct);

            if (!userBelongsToInitiative && entity.IsDraft)
            {
                return new CustomWebResponse(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                entity.ILikedIt = entity.Likes?.Any(e => e.UserName == userName);
            }

            var dataDto = mapper.Map(entity);

            return new()
            {
                ResponseBody = dataDto,
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
            Message = MessageConstants.NotFound,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, string userName, ODataQueryOptions<Resource> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = await entityRepository.GetQueryWithInitiativeAndUserNameAsync(initiativeId, userName, query, ct);

        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);

            foreach (var entity in odataResponse.DataList)
            {
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    entity.ILikedIt = entity?.Likes?.Any(e => e.UserName == userName);
                }
            }

            return GetOdataWebResponse(odataResponse, mapper);
        }
        catch (ODataException ex)
        {
            return new(true)
            {
                Message = $"Invalid filter: {ex.Message}",
            };
        }
    }

    /// <inheritdoc/>
    public Task<CustomWebResponse> AddAsync(string userName, ResourceDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public Task<CustomWebResponse> LikeActionAsync(ResourceDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <inheritdoc/>
    public Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();
}
