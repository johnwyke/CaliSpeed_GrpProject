using Microsoft.VisualStudio.TestTools.UnitTesting;
using CaliforniaSpeedLibrary;
using System.Threading;
using System.Threading.Tasks;
using System;
using static CaliforniaSpeedLibrary.Game;
using System.Collections.Generic;


// See https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics?view=vs-2019


namespace CaliforniaSpeedUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Constructor()
        {
            Game game = new Game();
        }

        [TestMethod]
        public void CheckValidCards()
        {
            Game game = new Game();
            foreach (var deck in game.playgameBoard)
            {
                Assert.IsTrue(deck.cardList.Count > 0);
            }
        }


        private int newCardCalled = 0;

        private List<Tuple<Tuple<int, int>, Card>> findMatches(Game game)
        {
            var matches = new List<Tuple<Tuple<int, int>, Card>>();
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    var deck = game.playgameBoard[i, j];
                    var card = deck.cardList[deck.cardList.Count - 1];
                    if (deck.matchPresent)
                    {
                        matches.Add(new Tuple<Tuple<int, int>, Card>(new Tuple<int, int>(i, j), card));
                    }
                }
            }
            return matches;
        }

        [TestMethod]
        public async Task CheckNewCard()
        {
            Game game = new Game();
            game.player1Id = "Player 1";
            // bind our NewCardPlayedEvent as if we are Room.cs
            game.NewCardPlayedEvent += Game_NewCardPlayedEvent;

            // find matches
            var matches = findMatches(game);
            // ensure we found a match
            Assert.AreNotEqual(matches.Count, 0);

            foreach (var match in matches)
            {
                Assert.IsTrue(await game.PlayCards("Player 1", match.Item1.Item1, match.Item1.Item2));
            }

            
            // verify we received the event
            Assert.AreEqual(newCardCalled, matches.Count);
        }

        private async Task Game_NewCardPlayedEvent(int player, int row, int column, Game.Card card)
        {
            newCardCalled++; // set the flag we've received something
            await Task.Delay(1); // dumb delay to make warning go away
        }

        private int newBoardCalled = 0;

        [TestMethod]
        public async Task CheckBoardClear()
        {
            Game game = new Game();
            // bind our NewBoardEvent as if we are Room.cs
            game.NewBoardEvent += Game_NewBoardEvent;

            await game.ClearBoard();

            Assert.AreEqual(newBoardCalled, 1);
        }

        private async Task Game_NewBoardEvent(Game.Card[,] cards)
        {
            newBoardCalled++;
            await Task.Delay(1);
        }
    }
}
