using CaliforniaSpeedLibrary;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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
                await Clients.Client(Context.ConnectionId).SendAsync("PlayResult", true);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("PlayResult", false);
            }
        }

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
