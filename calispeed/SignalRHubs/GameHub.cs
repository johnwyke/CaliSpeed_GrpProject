using CaliforniaSpeedLibrary;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            if (/*if card state is playable*/game.PlayCards(Context.ConnectionId.GetHashCode(), row, column))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayResult", true);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayResult", false);
            }
        }

        /// <summary>
        /// The client has asked for all cards to be received, send them.
        /// (Kameron): 'All Cards', as in the whole deck? Or the face up
        /// cards on the board? Or should I be sending all 4 variables
        /// at the same time (play, player1, player2, Deck)?
        /// </summary>
        /// <returns></returns>
        public async Task GetCardsList()
        {
            var game = GetGame();
            string gameString = JsonConvert.SerializeObject(game.Deck/* insert needed property/var here */);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCardsList", gameString);
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
            string cardsString = JsonConvert.SerializeObject(cards/* insert needed property/var here */);
            await Clients.All.SendAsync("Update_AllCards", cardsString);
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
            CardPlay play = new CardPlay() { Card = card, Column = column, Row = row };
            string newCardString = JsonConvert.SerializeObject(play/* insert needed property/var here */);
            await Clients.All.SendAsync("Update_NewCard", newCardString);
        }
    }
}
