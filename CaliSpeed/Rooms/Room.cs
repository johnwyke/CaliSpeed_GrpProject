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
            //Console.WriteLine("ConnectionId: ");
            //Console.WriteLine(connectionId);
            GameInstance = new Game();
            GameInstance.NewBoardEvent += Game_NewBoardEvent;
            GameInstance.NewCardPlayedEvent += Game_NewCardPlayedEvent;
            GameInstance.NewWinnerEvent += GameInstance_NewWinnerEvent;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Called when a new winner is added
        /// </summary>
        /// <param name="winMessage"></param>
        /// <returns></returns>
        private async Task GameInstance_NewWinnerEvent(string winMessage)
        {
            await _hubContext.Clients.All.SendAsync("NewWinner", winMessage);
        }

        /// <summary>
        /// Callback function that is called when all cards on the play field should be updated
        /// </summary>
        /// <param name="cards">2x4 array containing the cards</param>
        /// <returns></returns>
        private async Task Game_NewBoardEvent(Game.Card[,] cards)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveCardsList", cards);
        }
        /// <summary>
        /// Called when a new card is played by a player
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
