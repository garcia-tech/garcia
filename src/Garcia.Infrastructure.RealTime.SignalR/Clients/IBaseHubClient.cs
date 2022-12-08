namespace Garcia.Infrastructure.RealTime.SignalR.Clients
{
    public interface IBaseHubClient
    {
        Task ReceiveMessage(string user, string message);
        Task Send(string message);
        Task Notify(string message);
    }
}
