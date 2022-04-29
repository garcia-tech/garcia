using Garcia.Infrastructure.RealTime.SignalR.Clients;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Garcia.Infrastructure.RealTime.SignalR.Hubs
{
    public class BaseHub : Hub<IBaseHubClient>
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

        public virtual async Task SendMessage(string user, string message)
        {
            await Clients.All.RecieveMessage(user, message);
        }

        public virtual async Task SendPrivateMessage(string user, string message)
        {
            await Clients.User(user).RecieveMessage(user, message);
        }

        public virtual async Task SendMessageToClient(string user, string message)
        {
            await Clients.Client(Context.ConnectionId).RecieveMessage(user, message);
        }

        public virtual async Task SendMessageToGroup(string group, string message)
        {
            await Clients.Group(group ?? Settings.DefaultGroup).Send(message);
        }

        public virtual async Task SendMessageToGroups(IEnumerable<string> groups, string message)
        {
            await Clients.Groups(groups).Send(message);
        }

        public virtual async Task AddGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public virtual async Task RemoveGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
    }
}