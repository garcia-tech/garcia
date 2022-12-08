using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garcia.Application.Contracts.PushNotification
{
    public interface IPushNotificationService
    {
        /// <summary>
        /// Sends a push notification to given token.
        /// </summary>
        /// <param name="token">Device token.</param>
        /// <param name="title">Push notification title.</param>
        /// <param name="body">Push notification body.</param>
        /// <param name="imageUrl">Push notification image url.</param>
        /// <param name="data">Custom data.</param>
        /// <returns></returns>
        Task<string> SendPushNotificationAsync(string token, string title, string body, string imageUrl = null, Dictionary<string, string> data = null);
        /// <summary>
        /// Sends a push notifiaction to given tokens.
        /// </summary>
        /// <param name="tokens">Device tokens.</param>
        /// <param name="title">Push notification title.</param>
        /// <param name="body">Push notification body.</param>
        /// <param name="imageUrl">Push notification image url.</param>
        /// <param name="data">Custom data.</param>
        /// <returns></returns>
        Task<int> SendPushNotificationAsync(List<string> tokens, string title, string body, string imageUrl = null, Dictionary<string, string> data = null);
        /// <summary>
        /// Sends a push notification to topic. Devices subscribed to topic will receive sent notification.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="title">Push notification title.</param>
        /// <param name="body">Push notification body.</param>
        /// <param name="imageUrl">Push notification image url.</param>
        /// <param name="data">Custom data.</param>
        /// <returns></returns>
        Task<string> SendPushNotificationToTopicAsync(string topic, string title, string body, string imageUrl = null, Dictionary<string, string> data = null);
    }
}
