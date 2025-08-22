namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
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

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeJoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeJoinRequestStatus;

/// <summary>
/// Initiative Join Request service.
/// </summary>
public class InitiativeJoinRequestService : ServiceRead<InitiativeJoinRequest, InitiativeJoinRequestDto, int, InitiativeJoinRequestSpec>, IInitiativeJoinRequestService
{
    private new readonly IInitiativeJoinRequestRepository entityRepository;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    public InitiativeJoinRequestService(
        IInitiativeJoinRequestRepository entityRepository,
        IMapper<InitiativeJoinRequest, InitiativeJoinRequestDto> mapper,
        ILogger logger,
        IInitiativeRepository initiativeRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Add(InitiativeJoinRequestDto entityData, CancellationToken ct = default)
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
        var hasPendingRequests = await entityRepository.AnyAsync(InitiativeJoinRequestSpec.PendingRequests(entityData.InitiativeId, entityData.UserName), ct);

        if (hasPendingRequests)
        {
            return new CustomWebResponse(true)
            {
                Message = "There are one or more pending join requests",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.StatusId = (int)InitiativeJoinRequestStatusEnum.UnderReview;

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
    public async Task<CustomWebResponse> Update(int id, InitiativeJoinRequestDto entityData, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        if (entity.StatusId != (int)InitiativeJoinRequestStatusEnum.UnderReview)
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
