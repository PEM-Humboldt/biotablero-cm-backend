namespace IAVH.BioTablero.CM.Application.Services.Notifications;

using System;
using System.Collections.Concurrent;
using System.Threading.Channels;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;

/// <summary>
/// SSE Notification Dispatcher implementation.
/// </summary>
public class SseNotificationDispatcher : ISseNotificationDispatcher
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Channel<NotificationDto>>> subscriptions = new();

    /// <inheritdoc/>
    public ChannelReader<NotificationDto> Subscribe(string userName, Guid connectionId)
    {
        var channel = Channel.CreateUnbounded<NotificationDto>();
        var userSubs = subscriptions.GetOrAdd(userName, _ => new ConcurrentDictionary<Guid, Channel<NotificationDto>>());
        userSubs.TryAdd(connectionId, channel);
        return channel.Reader;
    }

    /// <inheritdoc/>
    public void Unsubscribe(string userName, Guid connectionId)
    {
        if (subscriptions.TryGetValue(userName, out var userSubs))
        {
            if (userSubs.TryRemove(connectionId, out var channel))
            {
                channel.Writer.TryComplete();
            }

            if (userSubs.IsEmpty)
            {
                subscriptions.TryRemove(userName, out _);
            }
        }
    }

    /// <inheritdoc/>
    public async Task DispatchAsync(string userName, NotificationDto notificationDto)
    {
        if (subscriptions.TryGetValue(userName, out var userSubs))
        {
            foreach (var channel in userSubs.Values)
            {
                await channel.Writer.WriteAsync(notificationDto);
            }
        }
    }
}
