using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Garcia.Application.Contracts.PushNotification;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Garcia.Infrastructure;

namespace Garcia.Infrastructure.PushNotification.Firebase
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebasePushNotificationService(IOptions<FirebasePushNotificationSettings> options)
        {
            var settings = options.Value;
            GoogleCredential googleCredential = null;

            if (!string.IsNullOrEmpty(settings.JsonString))
            {
                googleCredential = GoogleCredential.FromJson(settings.JsonString);
            }
            else if (!string.IsNullOrEmpty(settings.JsonFilePath))
            {
                googleCredential = GoogleCredential.FromFile(settings.JsonFilePath);
            }
            else if (!string.IsNullOrEmpty(settings.AccessToken))
            {
                googleCredential = GoogleCredential.FromAccessToken(settings.AccessToken);
            }

            if (googleCredential == null)
            {
                throw new InfrastructureException("GoogleCredential cannot be empty.");
            }

            if (FirebaseApp.DefaultInstance == null)
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = googleCredential
                });
            }
            else
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
        }

        private Message CreateMessage(string token, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null)
        {
            return new Message()
            {
                Data = data,
                Token = token,
                Notification = new Notification()
                {
                    Body = body,
                    Title = title,
                    ImageUrl = imageUrl
                }
            };
        }

        public async Task<string> SendPushNotificationAsync(string token, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null)
        {
            var message = CreateMessage(token, title, body, imageUrl, data);
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }

        public async Task<string> SendPushNotificationToTopicAsync(string topic, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null)
        {
            var message = new Message()
            {
                Data = data,
                Topic = topic,
                Notification = new Notification()
                {
                    Body = body,
                    Title = title,
                    ImageUrl = imageUrl
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }

        public async Task<int> SendPushNotificationAsync(List<string> tokens, string title, string body, string? imageUrl = null, Dictionary<string, string>? data = null)
        {
            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Data = data,
                Notification = new Notification()
                {
                    Body = body,
                    Title = title,
                    ImageUrl = imageUrl
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return response.SuccessCount;
        }
    }
}