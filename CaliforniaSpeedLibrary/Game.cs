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


        public Card[,] play = new Card[2, 4];
        public List<Card> player1 = new List<Card>();
        public List<Card> player2 = new List<Card>();
        public Card [] Deck = new Card[52];

        public Game() { init_Deck(); }// ConStructor


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
            public int Suit { get; set; }
            public int Face { get; set; }
        }

        /// <summary>
        /// if there is any cards matches 
        /// </summary>
        public void CardsMatching()
        {
            //check if any cards matching 
            // state become disable permanently


        }

        /// <summary>
        /// This is called by the Shuffle Deck
        /// distributed cards each player should have 26 cards 22 in hand and 4 on table
        /// </summary>
        public void DistributeCards()
        { 
            // Pull out the back 8 cards of the deck 
            for(int i= 0; i < Deck.Length-8; i++)
            {
                // This is where we create the user list of cards. 
                // divide the rest between two players
                if (i % 2 == 0)
                {
                    // Then it should be player One Cards
                    player1.Add(Deck[i]);
                }
                else if (i % 2 == 1)
                {
                    // Then this card should go to Player 2
                    player2.Add(Deck[i]);
                }
            }
            // TODO: Fill the "Play" 2d Array with the remaining cards. 


           

           
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
            for (int i = Deck.Length - 1; i >= 0; i--)
            {
                newNumb = rand.Next(i);
                temp = Deck[i];
                Deck[i] = Deck[newNumb];
                Deck[newNumb] = temp;

            }// End Loop 
            // Test Displaying the deck 
            foreach (Card item in Deck)
            {
                Console.WriteLine("Suit: " + item.Suit + " Face: " + item.Face);
            }
            DistributeCards();
        }

        /// <summary>
        /// play game 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool PlayCards(int player, int row, int column)
        {
            return false;
        }


        /// <summary>
        /// clear board and start again
        /// </summary>
        public void ClearBoard()
        {

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
            foreach (int cardSuit in Enum.GetValues(typeof(Suit)))
            {
                // For Each Value of the card
                foreach ( int cardFace in Enum.GetValues(typeof(Face)))
                {
                    Card newCard = new Card();

                    newCard.Suit = cardSuit;
                    newCard.Face = cardFace;

                    Deck[counter] = newCard;
                    counter++;
                }
            }
            ShuffleDeck();
          

        }



    }


}
