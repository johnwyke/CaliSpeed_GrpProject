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


        public Deck[,] playgameBoard = new Deck[2, 4];
        public Deck player1 = new Deck();
        public Deck player2 = new Deck();
        public Card [] wholeDeck = new Card[52];

        public Game()
        {
            init_Deck();
        }// ConStructor


        public enum Suit
        {
            Heart,
            Diamond,
            Spade,
            Club
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
            // TODO: Fill the "Play" 2d Array with the remaining cards. 
            foreach (Deck Cell in playgameBoard)
            {
                Deck boardCell = new Deck();
                boardCell.cardList.Add(wholeDeck[counter]);
                counter++;
            }
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

            }// End Loop 
            // Test Displaying the deck 
            //foreach (Card item in wholeDeck)
            //{
            //    Console.WriteLine("Suit: " + item.Suit + " Face: " + item.Face);
            //}
           DistributeCards();
        }

        /// <summary>
        /// play game 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool PlayCards()
        {
            // build card object

            // Very last thing I do is Reset all Flags
            setMatchingFlags();
            return false;
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
                            if (Cell.cardList[Cell.cardList.Count - 1].Face != playgameBoard[i, j].cardList[playgameBoard[i, j].cardList.Count - 1].Face &&
                                Cell.cardList[Cell.cardList.Count - 1].Suit != playgameBoard[i, j].cardList[playgameBoard[i, j].cardList.Count - 1].Suit)
                            {
                                // Then They are Not the same Card.
                                if (Cell.cardList[Cell.cardList.Count - 1].Face == playgameBoard[i, j].cardList[playgameBoard[i, j].cardList.Count - 1].Face)
                                {
                                    playgameBoard[i, j].matchPresent = true;
                                }
                                else
                                    playgameBoard[i, j].matchPresent = false;
                            }

                        }// End Inner
                    }// End Outer
                }// End If 
            }// End For Each
        }


        /// <summary>
        /// clear board and start again
        /// </summary>
        public void ClearBoard()
        {
            foreach (Deck boardSell in playgameBoard)
            {
                boardSell.cardList.Clear();
            }
            
            player1.cardList.Clear();
            player2.cardList.Clear();
            ShuffleDeck();

        }

        public void CheckWinner()
        {
             /* player cover cards
             * no matches state
             * player wins/lose        
             */

        }

        //
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



    }


}
