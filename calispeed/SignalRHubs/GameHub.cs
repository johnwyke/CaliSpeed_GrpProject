using CaliforniaSpeedLibrary;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CaliSpeed.SignalRHubs
{
    public class GameHub : Hub
    {
        /// <summary>
        ///  Container class that encapsulates all data that the client needs for a new play
        /// </summary>
        public class CardPlay
        {
            /// <summary>
            /// Incoming card
            /// </summary>
            public Game.Card Card;
            /// <summary>
            /// Row that the play takes place at
            /// </summary>
            public int Row;
            /// <summary>
            /// Column that the play takes place at
            /// </summary>
            public int Column;
        }

        private static Game game;

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
            if (game.PlayCards(Context.ConnectionId.GetHashCode(), row, column))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("PlayResult", true);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("PlayResult", false);
            }
        }

        /// <summary>
        /// The client has asked for all cards to be received, send them.
        /// </summary>
        /// <returns></returns>
        public async Task GetCardsList()
        {
            var game = GetGame();
            await Clients.Client(Context.ConnectionId).SendAsync("AllCards", game.play);
        }


        private Game GetGame()
        {
            if (game == null)
            {
                game = new Game();
                // Bind events to Game events
                // See CaliforniaSpeedLibrary::Game.cs to see further documentation
                game.NewBoardEvent += Game_NewBoardEvent;
                game.NewCardPlayedEvent += Game_NewCardPlayedEvent;
            }
            return game;
        }

        /// <summary>
        /// Callback function that allows 
        /// </summary>
        /// <param name="cards">2x4 array containing the cards</param>
        /// <returns></returns>
        private async Task Game_NewBoardEvent(Game.Card[,] cards)
        {
            await Clients.All.SendAsync("AllCards", cards);
        }

        private async Task Game_NewCardPlayedEvent(int player, int row, int column, Game.Card card)
        {
            CardPlay play = new CardPlay() { Card = card, Column = column, Row = row };
            await Clients.All.SendAsync("NewCard", play);
        }
    }
}
