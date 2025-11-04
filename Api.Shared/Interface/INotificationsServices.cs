using Api.Shared.Models;

namespace Api.Shared.Interface
{
    public interface INotificationsServices
    {
        Task<Notification> AddAsync(Notification Model);
        Task<bool> DeleteAsync(int NotificationId);
        Task<Notifications_GetByUser> GetByNotificationId(int NotificationId);
        Task<List<Notifications_GetByUser>> GetByUserId(string UserId);
        Task<int> SendNotification(Notification Model);
        Task<bool> UpdateAsync(Notification Model);
        Task<bool> Viwed(string UserId);
    }
}