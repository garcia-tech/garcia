using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Infrastructure.RealTime.SignalR.Clients
{
    public interface IBaseHubClient
    {
        Task RecieveMessage(string user, string message);
        Task Send(string message);
        Task Notify(string message);
    }
}
