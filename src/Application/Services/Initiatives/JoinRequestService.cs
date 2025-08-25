namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request service.
/// </summary>
public class JoinRequestService : ServiceRead<JoinRequest, JoinRequestDto, int, JoinRequestSpec>, IJoinRequestService
{
    private new readonly IJoinRequestRepository entityRepository;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IRepository<InitiativeUser> initiativeUserRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="initiativeUserRepository">Initiative user repository.</param>
    public JoinRequestService(
        IJoinRequestRepository entityRepository,
        IMapper<JoinRequest, JoinRequestDto> mapper,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IRepository<InitiativeUser> initiativeUserRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.initiativeUserRepository = initiativeUserRepository;
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetList(int initiativeId, string userName, ODataQueryOptions<JoinRequest> queryOptions, CancellationToken ct = default)
    {
        // Validate user level
        var userIsLeader = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserLevelSpec(initiativeId, userName, (int)InitiativeUserLevelEnum.Leader), ct);

        if (!userIsLeader)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        var query = entityRepository.GetQueryable();
        query = entityRepository.AddInitiativeFilter(initiativeId, query);

        return await GetOdataListByQuery(query, queryOptions, ct);
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Add(JoinRequestDto entityData, CancellationToken ct = default)
    {
        // Validate initiative
        var initiativeExists = await initiativeRepository.AnyAsync(new InitiativeSpec(entityData.InitiativeId), ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate pending requests
        var hasPendingRequests = await entityRepository.AnyAsync(JoinRequestSpec.PendingRequests(entityData.InitiativeId, entityData.UserName), ct);

        if (hasPendingRequests)
        {
            return new CustomWebResponse(true)
            {
                Message = "There are one or more pending join requests",
            };
        }

        // Validate user and initiative relationship
        var hasUserAndInitiativeRelationship = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserNameSpec(entityData.InitiativeId, entityData.UserName), ct);

        if (hasUserAndInitiativeRelationship)
        {
            return new CustomWebResponse(true)
            {
                Message = "The user already belongs to the initiative",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.StatusId = (int)JoinRequestStatusEnum.UnderReview;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative join request: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Update(int id, JoinRequestDto entityData, CancellationToken ct = default)
    {
        // Validate user level
        var userIsLeader = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserLevelSpec(entityData.InitiativeId, entityData.ReviewerUserName, (int)InitiativeUserLevelEnum.Leader), ct);

        if (!userIsLeader)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        if (entity.StatusId != (int)JoinRequestStatusEnum.UnderReview)
        {
            return new CustomWebResponse(true)
            {
                Message = "The join request has already been reviewed",
            };
        }

        // Update entity data
        entity = await entityRepository.ReviewRequest(id, entityData.ReviewerUserName, entityData.Status.Id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Database error",
            };
        }

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative join request: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }
}
