namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

using InitiativeJoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeJoinRequestStatus;
using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Custom Initiative Join Request repository.
/// </summary>
public class InitiativeJoinRequestRepository : Repository<InitiativeJoinRequest>, IInitiativeJoinRequestRepository
{
    private readonly GeneralContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeJoinRequestRepository(GeneralContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Review request.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="requestStatusId">Request status identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated request data.</returns>
    public async Task<InitiativeJoinRequest> ReviewRequest(int requestId, string reviewerUserName, int requestStatusId, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var entity = await dbContext.InitiativeJoinRequests
                .Where(e => e.Id == requestId)
                .FirstOrDefaultAsync(ct);

            // Ignore transaction for empty or reviewed entities or empty reviewer user name
            if (entity == null || entity.StatusId != (int)InitiativeJoinRequestStatusEnum.UnderReview || string.IsNullOrEmpty(reviewerUserName))
            {
                return entity;
            }

            if (requestStatusId == (int)InitiativeJoinRequestStatusEnum.Approved)
            {
                // Update initiative and user relationship
                var userBelongsToinitiative = await dbContext.InitiativeUsers
                    .Where(e => e.UserName == entity.UserName && e.InitiativeId == entity.InitiativeId)
                    .AnyAsync(ct);

                if (!userBelongsToinitiative)
                {
                    var initiativeUserEntity = new InitiativeUser()
                    {
                        InitiativeId = entity.InitiativeId,
                        LevelId = (int)InitiativeUserLevelEnum.Member,
                    };

                    await dbContext.InitiativeUsers.AddAsync(initiativeUserEntity, ct);
                    await dbContext.SaveChangesAsync(ct);
                }
            }

            // Update request data
            entity.StatusId = requestStatusId;
            entity.ReviewerUserName = reviewerUserName;
            entity.ResponseDate = DateTime.Now;

            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entity;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            return null;
        }
    }
}
