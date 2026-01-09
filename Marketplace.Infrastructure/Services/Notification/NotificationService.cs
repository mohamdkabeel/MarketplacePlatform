using Marketplace.Application.IServices.Notification;
using Marketplace.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Marketplace.Infrastructure.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
