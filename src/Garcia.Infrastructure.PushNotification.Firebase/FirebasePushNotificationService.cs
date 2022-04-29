using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Garcia.Application.Contracts.PushNotification;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.PushNotification.Firebase
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebasePushNotificationService(IOptions<FirebasePushNotificationSettings> options)
        {
            var settings = options.Value;

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = !string.IsNullOrEmpty(settings.AccessToken) ? 
                    GoogleCredential.FromAccessToken(settings.AccessToken) :
                    GoogleCredential.FromFile(settings.FilePath)
            });
        }

        public async Task<int> SendPushNotificationAsync(string token, Dictionary<string, string> data)
        {
            var message = new Message()
            {
                Data = data,
                Token = token,

                Android = new AndroidConfig
                {
                    Data = data
                },

                Apns = new ApnsConfig
                {
                    CustomData = (IDictionary<string, object>) 
                        data.ToDictionary(x => x.Key, x => x.Value),

                    Aps = new Aps
                    {
                        MutableContent = true,
                        ContentAvailable = false
                    }
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return string.IsNullOrEmpty(response) ? 0 : 1;
        }

        public async Task<int> SendPushNotificationToTopicAsync(string topic, Dictionary<string, string> data)
        {
            var message = new Message()
            {
                Data = data,
                Topic = topic,

                Android = new AndroidConfig
                {
                    Data = data
                },

                Apns = new ApnsConfig
                {
                    CustomData = (IDictionary<string, object>)
                        data.ToDictionary(x => x.Key, x => x.Value),

                    Aps = new Aps
                    {
                        MutableContent = true,
                        ContentAvailable = false
                    }

                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return string.IsNullOrEmpty(response) ? 0 : 1;
        }

        public async Task<int> SendPushNotificationAsync(List<string> tokens, Dictionary<string, string> data)
        {
            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Data = data,

                Android = new AndroidConfig
                {
                    Data = data
                },

                Apns = new ApnsConfig
                {
                    CustomData = (IDictionary<string, object>)
                        data.ToDictionary(x => x.Key, x => x.Value),

                    Aps = new Aps
                    {
                        MutableContent = true,
                        ContentAvailable = false
                    }
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return response.SuccessCount;
        }

        public async Task<int> SendBatchPushNotificationAsync(string token, List<Dictionary<string, string>> data)
        {
            var messages = data.Select(x => new Message()
            {
                Token = token,
                Data = x,

                Android = new AndroidConfig
                {
                    Data = x
                },

                Apns = new ApnsConfig
                {
                    CustomData = (IDictionary<string, object>)
                        x.ToDictionary(d => d.Key, d => d.Value),

                    Aps = new Aps
                    {
                        MutableContent = true,
                        ContentAvailable = false
                    }
                }
            });
            var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
            return response.SuccessCount;
        }
    }
}