namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using Serilog;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request repository.
/// </summary>
public class JoinRequestRepository : Repository<JoinRequest, int>, IJoinRequestRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public JoinRequestRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<JoinRequest>> GetByUserNameAsync(string userName, CancellationToken ct = default) =>
        await dbContext.JoinRequests
            .Include(e => e.Initiative)
            .Where(e => e.UserName == userName)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public IQueryable<JoinRequest> AddInitiativeFilter(int initiativeId, IQueryable<JoinRequest> query) =>
        query
            .Where(e => e.InitiativeId == initiativeId);

    /// <inheritdoc/>
    public async Task<bool> AnyPendingRequests(int initiativeId, string userName, CancellationToken ct = default) =>
        await dbContext.JoinRequests
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName && e.StatusId == (int)JoinRequestStatusEnum.UnderReview)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<JoinRequest> ReviewRequestAsync(int requestId, string reviewerUserName, int requestStatusId, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
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
                            UserName = entity.UserName,
                            LevelId = entity.LevelId,
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

                return entity;
            },
            "Join request review transaction error",
            ct);

    /// <inheritdoc/>
    public async Task<Dictionary<string, int>> GetPendingOldRequestsAsync(int daysOld, CancellationToken ct = default)
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
                join.jr.CreationDate <= since)
            .GroupBy(join => join.iu.UserName)
            .Select(g => new
            {
                UserName = g.Key,
                Count = g.Count(),
            })
            .ToDictionaryAsync(group => group.UserName, group => group.Count, ct);

        return await pendingRequestsByLeaders;
    }
}
