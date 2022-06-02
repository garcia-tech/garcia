using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.PushNotification
{
    public interface IPushNotificationService
    {
        Task<int> SendPushNotificationAsync(string token, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null);
        Task<int> SendPushNotificationAsync(List<string> tokens, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null);
        Task<int> SendPushNotificationToTopicAsync(string topic, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null);
    }
}
