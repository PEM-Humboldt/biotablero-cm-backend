namespace IAVH.BioTablero.CM.WebApi.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using Cronos;

using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Join Request tasks.
/// </summary>
/// <param name="serviceScopeFactory">Service scope factory.</param>
/// <param name="logger">System logger.</param>
public class PendingJoinRequestsTask(
    IServiceScopeFactory serviceScopeFactory,
    ILogger logger) : BackgroundService
{
    private const string TaskName = "Old pending requests";
    private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;
    private readonly CronExpression cronExpression = CronExpression.Parse("0 8 * * MON"); // At 08:00 AM, only on Monday
    private readonly TimeZoneInfo timeZone = TimeZoneInfo.Local;
    private readonly ILogger logger = logger;

    /// <summary>
    /// Execute task.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token.</param>
    /// <returns>Process result.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var joinRequestService = scope.ServiceProvider.GetRequiredService<IJoinRequestService>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var next = cronExpression.GetNextOccurrence(DateTimeOffset.Now, timeZone);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, stoppingToken);
                }
            }

            if (!stoppingToken.IsCancellationRequested)
            {
                await joinRequestService.SendNotificationsOldPendingRequestsAsync(stoppingToken);
                logger.AddLog(LogType.System, "Executed task", "{@Task}", TaskName);
            }
        }
    }
}
