using CaliforniaSpeedLibrary;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CaliSpeed.SignalRHubs
{
    public class GameHub : Hub
    {
        public class CardPlay
        {
            public Game.Card Card;
            public int Row;
            public int Column;
        }

        private static Game game;

        public async Task PlayCard(int row, int column)
        {
            var game = GetGame();
            if (game.PlayCards(Context.ConnectionId.GetHashCode(), row, column))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayResult", true);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayResult", false);
            }
        }

        public async Task GetCardsList()
        {
            var game = GetGame();
            string gameString = JsonConvert.SerializeObject(game.play);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCardsList", gameString);
        }


        private Game GetGame()
        {
            if (game == null)
            {
                game = new Game();
                game.NewBoardEvent += Game_NewBoardEvent;
                game.NewCardEvent += Game_CardPlayedEvent;
            }
            return game;
        }

        private async Task Game_NewBoardEvent(Game.Card[,] cards)
        {
            await Clients.All.SendAsync("AllCards", cards);
        }

        private async Task Game_CardPlayedEvent(int player, int row, int column, Game.Card card)
        {
            CardPlay play = new CardPlay() { Card = card, Column = column, Row = row };
            await Clients.All.SendAsync("NewCard", play);
        }
    }
}
