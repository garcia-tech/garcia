using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.PushNotification
{
    public interface IPushNotificationService
    {
        Task<int> SendPushNotificationAsync(string token, Dictionary<string, string> data);
        Task<int> SendPushNotificationAsync(List<string> tokens, Dictionary<string, string> data);
        Task<int> SendBatchPushNotificationAsync(string token, List<Dictionary<string, string>> data);
        Task<int> SendPushNotificationToTopicAsync(string topic, Dictionary<string, string> data);
    }
}
