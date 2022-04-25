using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.PushNotification
{
    public interface IPushNotificationService
    {
        Task<int> SendPushNotificationAsync(string token, Dictionary<string, string> data);
        Task<int> SendPushNotificationAsync(List<string> tokens, Dictionary<string, string> data);
        Task<int> SendBatchPushNotificationAsync(string token, List<Dictionary<string, string>> data);
    }
}
