using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CaliSpeed.SignalR_Hubs
{
    public class ExampleHub : Hub
    {
        public async Task PlayCard(string user)
        {
            await Clients.All.SendAsync("ReceiveCard", user);
        }
    }
}
