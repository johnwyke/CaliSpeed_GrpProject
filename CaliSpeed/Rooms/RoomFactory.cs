using CaliSpeed.SignalRHubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaliSpeed.Rooms
{
    /// <summary>
    /// Singleton class that handles creating rooms.
    /// </summary>
    public class RoomFactory
    {
        private static IHubContext<GameHub> _hubContext;

        public static void setup(IHubContext<GameHub> hub)
        {
            _hubContext = hub;
        }

        
        public static Room createRoom()
        {
            return new Room(_hubContext);
        }
    }
}
