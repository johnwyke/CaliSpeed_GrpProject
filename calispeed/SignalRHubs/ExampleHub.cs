using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CaliSpeed.SignalRHubs
{
    public class ExampleHub : Hub
    {
        public async Task PlayCard(string user)
        {
            await Clients.All.SendAsync("ReceiveCard", user);
        }
    }
}
