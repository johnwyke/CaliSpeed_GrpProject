using CaliforniaSpeedLibrary;
using CaliSpeed.SignalRHubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaliSpeed.Rooms
{
    /// <summary>
    /// Class that holds an instance of the game and provides messaging from outside the SignalR hub
    /// </summary>
    public class Room
    {
        public Game GameInstance { get; set; }

        private IHubContext<GameHub> _hubContext;

        public Room(IHubContext<GameHub> hubContext)
        {
            // Bind events to Game events
            // See CaliforniaSpeedLibrary::Game.cs to see further documentation
            GameInstance = new Game();
            GameInstance.NewBoardEvent += Game_NewBoardEvent;
            GameInstance.NewCardPlayedEvent += Game_NewCardPlayedEvent;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Callback function that allows
        /// (Kameron): "Signifies that all cards have changed"--I believe
        /// this means that Game.cs is trying to tell calispeed.js
        /// that a new game has started and that it needs to update
        /// everything visually. But when does Game.cs do this? I don't see
        /// any code where it happens yet.
        /// </summary>
        /// <param name="cards">2x4 array containing the cards</param>
        /// <returns></returns>
        private async Task Game_NewBoardEvent(Game.Card[,] cards)
        {
            
            await _hubContext.Clients.All.SendAsync("Update_AllCards", cards);
        }
        /// <summary>
        /// (Kameron): Ran when Game.cs tells calispeed.js that a new
        /// card has been played on the board. But when does Game.cs do this? I don't see any code where it happens yet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        private async Task Game_NewCardPlayedEvent(int player, int row, int column, Game.Card card)
        {
            Console.WriteLine("New card has been played by " + player);
            
            await _hubContext.Clients.All.SendAsync("Update_NewCard", row, column, card);

        }
    }
}
