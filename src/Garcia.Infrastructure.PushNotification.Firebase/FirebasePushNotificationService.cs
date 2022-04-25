using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Garcia.Application.Contracts.PushNotification;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.PushNotification.Firebase
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly FirebasePushNotificationSettings _settings;
        private readonly FirebaseApp _firebaseApp;

        public FirebasePushNotificationService(IOptions<FirebasePushNotificationSettings> settings)
        {
            _settings = settings.Value;

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromAccessToken(_settings.AccessToken),
            });
        }

        public async Task<int> SendPushNotificationAsync(string token, Dictionary<string, string> data)
        {
            var message = new Message()
            {
                Data = data,
                Token = token,
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return string.IsNullOrEmpty(response) ? 0 : 1;
        }

        public async Task<int> SendPushNotificationAsync(List<string> tokens, Dictionary<string, string> data)
        {
            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Data = data
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return response.SuccessCount;
        }

        public async Task<int> SendBatchPushNotificationAsync(string token, List<Dictionary<string, string>> data)
        {
            var messages = data.Select(x => new Message()
            {
                Token = token,
                Data = x
            });
            var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
            return response.SuccessCount;
        }
    }
}