namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Custom Join Request repository.
/// </summary>
public class JoinRequestRepository : Repository<JoinRequest>, IJoinRequestRepository
{
    private readonly GeneralContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public JoinRequestRepository(GeneralContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<JoinRequest> AddInitiativeFilter(int initiativeId, IQueryable<JoinRequest> query) => query
            .Where(e => e.InitiativeId == initiativeId);

    /// <summary>
    /// Review request.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="userName">User name.</param>
    /// <param name="requestStatusId">Request status identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated request data.</returns>
    public async Task<JoinRequest> ReviewRequest(int requestId, string reviewerUserName, string userName, int requestStatusId, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var entity = await dbContext.JoinRequests
                .Where(e => e.Id == requestId)
                .FirstOrDefaultAsync(ct);

            // Ignore transaction for empty or reviewed entities or empty reviewer user name
            if (entity == null || entity.StatusId != (int)JoinRequestStatusEnum.UnderReview || string.IsNullOrEmpty(reviewerUserName))
            {
                return entity;
            }

            if (requestStatusId == (int)JoinRequestStatusEnum.Approved)
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
                        UserName = userName,
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
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync(ct);
            return null;
        }
    }

    /// <summary>
    /// Get pending old requests.
    /// </summary>
    /// <param name="daysOld">Requests days old.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Pending requests grouped by leader user name.</returns>
    public async Task<Dictionary<string, int>> GetPendingOldRequests(int daysOld, CancellationToken ct = default)
    {
        var since = DateTime.UtcNow.AddDays(-daysOld);

        var pendingRequestsByLeaders = dbContext.InitiativeUsers
            .Where(iu => iu.LevelId == (int)InitiativeUserLevelEnum.Leader)
            .Join(
                dbContext.Initiatives,
                iu => iu.InitiativeId,
                i => i.Id,
                (iu, i) => new { iu, i })
            .Join(
                dbContext.JoinRequests,
                join => join.i.Id,
                jr => jr.InitiativeId,
                (join, jr) => new { join.iu, join.i, jr })
            .Where(join =>
                join.jr.StatusId == (int)JoinRequestStatusEnum.UnderReview &&
                join.jr.CreationDate >= since)
            .GroupBy(join => join.iu.UserName)
            .ToDictionaryAsync(group => group.Key, group => group.Count(), ct);

        return await pendingRequestsByLeaders;
    }
}
