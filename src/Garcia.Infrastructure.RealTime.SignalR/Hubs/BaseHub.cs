using Garcia.Infrastructure.RealTime.SignalR.Clients;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.RealTime.SignalR.Hubs
{
    public class BaseHub : BaseHub<IBaseHubClient>
    {
        public BaseHub(IOptions<SignalRSettings> options, ILogger<BaseHub> logger) : base(options, logger)
        {
        }
    }

    public class BaseHub<T> : Hub<T> where T : class, IBaseHubClient
    {
        protected SignalRSettings Settings { get; }
        private readonly ILogger<BaseHub> _logger;

        public BaseHub(IOptions<SignalRSettings> options, ILogger<BaseHub> logger)
        {
            Settings = options.Value;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            if (string.IsNullOrEmpty(Settings.DefaultGroup)) return;
            await AddGroup(Settings.DefaultGroup);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (string.IsNullOrEmpty(Settings.DefaultGroup)
                || Context.ConnectionAborted.IsCancellationRequested) return;
            await RemoveGroup(Settings.DefaultGroup);

            if (exception == null) return;
            _logger.LogError(exception, exception.Message);
        }
        /// <summary>
        /// Sends message to the all clients.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
        /// <summary>
        /// Sends message to the <paramref name="receiver"/>.
        /// </summary>
        /// <param name="sender">The user who sent the <paramref name="message"/>.</param>
        /// <param name="receiver">The user who receive the <paramref name="message"/>.</param>
        /// <param name="message">The sent message.</param>
        /// <returns></returns>
        public virtual async Task SendPrivateMessage(string sender, string receiver, string message)
        {
            await Clients.User(receiver).ReceiveMessage(sender, message);
        }
        /// <summary>
        /// Sends message to the spesific client.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessageToClient(string user, string message)
        {
            await Clients.Client(Context.ConnectionId).ReceiveMessage(user, message);
        }
        /// <summary>
        /// Sends message to the <paramref name="group"/>. If <paramref name="group"/> is not provided message will be sent to the default group.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessageToGroup(string group, string message)
        {
            await Clients.Group(group ?? Settings.DefaultGroup).Send(message);
        }
        /// <summary>
        /// Sends message to <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessageToGroups(IEnumerable<string> groups, string message)
        {
            await Clients.Groups(groups).Send(message);
        }
        /// <summary>
        /// Adds a connection to the <paramref name="group"/>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public virtual async Task AddGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        /// <summary>
        /// Removes a connection from the <paramref name="group"/>
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public virtual async Task RemoveGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
    }
}