using CaliforniaSpeedLibrary;
using CaliSpeed.Rooms;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace CaliSpeed.SignalRHubs
{
    public class GameHub : Hub
    {
        private static Room room;

        /// <summary>
        /// Allows a client to request that their card be played.
        /// Sends a play result back indicating whether their play succeeded or not.
        /// </summary>
        /// <param name="row">Row that the card should be placed</param>
        /// <param name="column">Column that the card should be placed</param>
        /// <returns></returns>
        public async Task PlayCard(int row, int column)
        {
            var game = GetGame();
            // verify our play works

            Console.WriteLine($"{Context.ConnectionId} played card at row:{row} column:{column}");

            if (await game.PlayCards(Context.ConnectionId, row, column))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayResult", true);
                Console.WriteLine("Received play result success");
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayResult", false);
                Console.WriteLine("Received play result fail");
            }
        }

        /// <summary>
        /// The client has asked for all cards in the play field to be received, send them
        /// </summary>
        /// <returns></returns>
        public async Task GetCardsList()
        {
            var game = GetGame();
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCardsList", game.getAsCards());
        }

        /// <summary>
        /// Allows the client to join a room
        /// </summary>
        /// <returns></returns>
        public async Task JoinRoom()
        {
            var game = GetGame();
            bool success = game.JoinGame(Context.ConnectionId);
            
            await Clients.Client(Context.ConnectionId).SendAsync("JoinRoomResult", success);

            if (success)
            {
                // also send the client the card list
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCardsList", game.getAsCards());
            }
        }

        public async Task ResetGame()
        {
            room = RoomFactory.createRoom();
            await JoinRoom();
        }

        private Game GetGame()
        {
            if (room == null)
            {
                room = RoomFactory.createRoom();
            }
            return room.GameInstance;
        }
    }
}
