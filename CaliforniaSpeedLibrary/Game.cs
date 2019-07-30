using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaliforniaSpeedLibrary
{
  public class Game
  {
        public delegate Task NewCardDelegate(int player, int row, int column, Card card);
        /// <summary>
        /// Event called when a new card has been played
        /// </summary>
        public event NewCardDelegate NewCardPlayedEvent;

        public delegate Task NewBoardDelegate(Card[,] cards);
        /// <summary>
        /// Called when the entire board changes and needs to be updated
        /// </summary>
        public event NewBoardDelegate NewBoardEvent;

        public delegate Task NewWinnerDelegate(string winMessage);
        /// <summary>
        /// Called when a new winner is added
        /// </summary>
        public event NewWinnerDelegate NewWinnerEvent;


        public Deck[,] playgameBoard = new Deck[2, 4];
        public Deck player1 = new Deck();
        public Deck player2 = new Deck();
        public string player1Id = null;
        public string player2Id = null;
        public Card [] wholeDeck = new Card[52];

        public Game()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    playgameBoard[i,j] = new Deck();
                }
            }
            init_Deck();
        }// Constructor


        public enum Suit
        {
            Club,
            Diamond,
            Heart,
            Spade
        }
        public enum Face
        {
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }
        public class Card
        { 
            public Suit Suit { get; set; }
            public Face Face { get; set; }
        }

        public class Deck
        {
            public List<Card> cardList = new List<Card>();
            public bool matchPresent { get; set; }


            public Deck()
            {
               cardList.Clear();
               matchPresent = false;

                
            }

            public Card getLastCard()
            {
                if (cardList.Count > 0)
                {
                    return cardList[cardList.Count - 1];
                }
                else
                {
                    return null;
                }
            }

            public override string ToString()
            {
                Card card = getLastCard();
                if (card != null)
                {
                    return $"face {card.Face} suit {card.Suit}";
                }
                else
                {
                    return "Bad deck";
                }
            }

        }

    


        /// <summary>
        /// This is called by the Shuffle Deck
        /// distributed cards each player should have 26 cards 22 in hand and 4 on table
        /// </summary>
        public void DistributeCards()
        {
            int counter = wholeDeck.Length - 8;
            // Pull out the back 8 cards of the deck 
            for(int i= 0; i < wholeDeck.Length-8; i++)
            {
                // This is where we create the user list of cards. 
                // divide the rest between two players
                if (i % 2 == 0)
                {
                    // Then it should be player One Cards
                    player1.cardList.Add(wholeDeck[i]);
                }
                else if (i % 2 == 1)
                {
                    // Then this card should go to Player 2
                    player2.cardList.Add(wholeDeck[i]);
                }
            }
            //  Fill the "Play" 2d Array with the remaining cards. 
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Deck boardCell = new Deck();
                    boardCell.cardList.Add(wholeDeck[counter]);
                    playgameBoard[i, j] = boardCell;
                    counter++;
                }
            }
            setMatchingFlags();
           
        }
        /// <summary>
        /// Handles the card distribution of each player for one row. 
        /// Takes place after Stale mate and each player has added the four decks in from of them. 
        /// </summary>
        public async void reDistributeCards()
        {
            // Pull out the back 8 cards of the deck 

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(i == 0)
                    {
                        // Player Ones side                       
                        playgameBoard[i, j].cardList.Clear();
                        playgameBoard[i, j].cardList.Add(player1.cardList[player1.cardList.Count - 1]);
                        player1.cardList.RemoveAt(player1.cardList.Count - 1);
                    }
                    else if (i == 1){
                        // Handles Players 2 cards
                        playgameBoard[i, j].cardList.Clear();
                        playgameBoard[i, j].cardList.Add(player2.cardList[player2.cardList.Count - 1]);
                        player2.cardList.RemoveAt(player2.cardList.Count - 1);
                    }
                   
                }

            }
            await NewBoardEvent(getAsCards());
            setMatchingFlags();

        }
        /// <summary>
        /// This Is called by Init Deck. 
        /// Shuffle the deck of cards and test the output. 
        /// </summary>
        private void ShuffleDeck()
        {
            Random rand = new Random();
            int newNumb = 0;
            Card temp;
            // Build the Deck of Cards make sure that the cards are not duplicated
            // Shuffle the cards. 
            for (int i = wholeDeck.Length - 1; i >= 0; i--)
            {
                newNumb = rand.Next(i);
                temp = wholeDeck[i];
                wholeDeck[i] = wholeDeck[newNumb];
                wholeDeck[newNumb] = temp;

            }// End Loop gi
            // Test Displaying the deck 
            //foreach (Card item in wholeDeck)
            //{
            //    Console.WriteLine("Suit: " + item.Suit + " Face: " + item.Face);
            //}
           DistributeCards();
        }
        /// <summary>
        /// Called After Stalemate is found. Shuffle All Cards in Players Deck. 
        /// /// </summary>
        private void ShuffleDeck(Deck player)
        {
            Random rand = new Random();
            int newNumb = 0;
            Card temp;
            // Build the Deck of Cards make sure that the cards are not duplicated
            // Shuffle the cards. 
            for (int i = player.cardList.Count- 1; i >= 0; i--)
            {
                newNumb = rand.Next(i);
                temp = player.cardList[i];
                player.cardList[i] = player.cardList[newNumb];
                player.cardList[newNumb] = temp;

            }// End Loop 
             
        }
        /// <summary>
        /// Adds all the card to players card stack. 
        /// </summary>
        private void gatherCards()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0)
                    {
                        // Player Ones side  
                        foreach (Card cellCard in playgameBoard[i,j].cardList)
                        {
                            player1.cardList.Add(cellCard);
                        }
                                               
                    }
                    else if (i == 1)
                    {
                        // Handles Players 2 cards
                        foreach (Card cellCard in playgameBoard[i, j].cardList)
                        {
                            player2.cardList.Add(cellCard);
                        }
                    }

                }

            }
        }


        /// <summary>
        /// play game 
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public async Task<bool> PlayCards(string connectionId, int row, int column)
        {
            int player = -1;
            if (player1Id == connectionId)
            {
                player = 0;
            }
            else if (player2Id == connectionId)
            {
                player = 1;
            }
            else
            {
                Console.WriteLine("Error, Failed to find connection ID for player");
                return false;
            }

            if (playgameBoard[row, column].matchPresent == true) { 
                //if(connectionId == player1Id)
                if(player == 0)
                {
                    playgameBoard[row, column].cardList.Add(player1.cardList[player1.cardList.Count - 1]);
                    player1.cardList.RemoveAt(player1.cardList.Count - 1);
                    playgameBoard[row, column].matchPresent = false;
                   
                }
                //if(connectionId == player2Id)
                else if (player == 1)
                {
                    playgameBoard[row, column].cardList.Add(player2.cardList[player2.cardList.Count - 1]);
                    player2.cardList.RemoveAt(player2.cardList.Count - 1);
                    playgameBoard[row, column].matchPresent = false;
                }
                if (CheckWinner())
                {
                    await ClearBoard();
                }
                setMatchingFlags();
                stalemate();
                var list = playgameBoard[row, column].cardList;
                await NewCardPlayedEvent(player, row, column, list[list.Count - 1]);
                printDebug();
                return true;
            }
            else
            {
                return false;
	        }
	    }

        /// <summary>
        /// I Loop through every Card on the game board 
        /// I first compare if my flag is already true 
        /// If not then I check to see if I am comparing against the same card. 
        /// If Not then I look for a matching Face. 
        /// </summary>
        private void setMatchingFlags()
        {
            // Loop Through the board setting flags
            foreach (Deck Cell in playgameBoard)
            {
                //Checking if Card was matched already.
                if (!Cell.matchPresent)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            var compCell = playgameBoard[i, j];
                            if (!object.ReferenceEquals(compCell, Cell))
                            {
                                var currCard = Cell.getLastCard();
                                var compCard = compCell.getLastCard();
                                if (currCard != null && compCell != null)
                                {
                                    if (currCard.Face == compCard.Face)
                                    {
                                        Cell.matchPresent = true;
                                        playgameBoard[i, j].matchPresent = true;
                                        Console.WriteLine($"Match i: {i} j: {j}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR: Card list was empty");
                                }
                            }
                        }// End Inner
                    }// End Outer
                }// End If 
            }// End For Each
        }


        public Card[,] getAsCards()
        {
            Card[,] cards = new Card[2, 4];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var cardsList = playgameBoard[i, j].cardList;
                    cards[i, j] = cardsList[cardsList.Count - 1];
                }
            }
            return cards;
        }

        /// <summary>
        /// clear board and start again
        /// </summary>
        public async Task ClearBoard()
        {
            foreach (Deck boardSell in playgameBoard)
            {
                boardSell.cardList.Clear();
            }
            
            player1.cardList.Clear();
            player2.cardList.Clear();

            
            ShuffleDeck();

            var cards = getAsCards();
            await NewBoardEvent(cards);
        }

        public bool CheckWinner()
        {
            /* player cover cards
            * no matches state
            * player wins/lose        
            */
            bool hasWinner = false;

            if (player1.cardList.Count == 0)
            {
                hasWinner = true;
                NewWinnerEvent("player1 won the game");
            }
            else if (player2.cardList.Count == 0)
            {
                hasWinner = true;
                NewWinnerEvent("player2 won the game");
            }
            return hasWinner;

        }

        /// <summary>
        /// BUilds deck of cards. 
        /// </summary>
        private void init_Deck()
        {
            int counter = 0;
            // Need to go through the Suit enum and then loop through the Card Enum. Builds a deck of Unique cards
            foreach (Suit cardSuit in Enum.GetValues(typeof(Suit)))
            {
                // For Each Value of the card
                foreach (Face cardFace in Enum.GetValues(typeof(Face)))
                {
                    Card newCard = new Card();

                    newCard.Suit = cardSuit;
                    newCard.Face = cardFace;

                    wholeDeck[counter] = newCard;
                    counter++;
                }
            }
            ShuffleDeck();
          

        }

        /// <summary>
        /// Check for stalemate. if stalemate found then combine cards and reshuffle and redistribute. 
        /// </summary>
        private void stalemate()
        {
            bool inStalemate = true;
            // Loop Through the board setting flags
            foreach (Deck Cell in playgameBoard)
            {
                if (!Cell.matchPresent)
                {
                    inStalemate = true;
                }else if (Cell.matchPresent)
                {
                    inStalemate = false;
                    break;
                }
                
            }// End For Each

            // If match Found is still False add All Cards and resuffle. 
            if (inStalemate)
            {
                Console.WriteLine("Stalemate encountered, redistributing cards");
                gatherCards();
                ShuffleDeck(player1);
                ShuffleDeck(player2);
                reDistributeCards();
            }
            
        }

        /// <summary>
        /// Attempts to join the game
        /// </summary>
        /// <param name="connectionId">Connection ID of the incoming player</param>
        /// <returns>True if the player successfully joined the game</returns>
        public bool JoinGame(String connectionId)
        {
            bool success = false;
            if (player1Id == null)
            {
                player1Id = connectionId;
                success = true;
            }
            else if (player2Id == null)
            {
                player2Id = connectionId;
                success = true;
            }

            return success;
        }

        private void printDebug()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var cell = playgameBoard[i, j];
                    var card = cell.cardList[cell.cardList.Count - 1];
                    Console.WriteLine($"i: {i} j: {j} Top card: {card.Face}, has match: {cell.matchPresent}");
                }
            }
        }

    }


}
