using System;
using System.Collections.Generic;

namespace CaliforniaSpeedLibrary
{
  public class Game
  {


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
            // state become disable permenentaly


        }

        /// <summary>
        /// distripiute cards each player should have 26 cards 22 in hand and 4 on table
        /// </summary>
        public void DistributeCards()
        {
            Random rand = new Random();
            int newNumb = 0;
            Card temp; 
            // Build the Deck of Cards make sure that the cards are not duplicated
            // Shuffle the cards. 
            for (int i =Deck.Length; i>0; i--)
            {
                newNumb = rand.Next(i + 1);
                temp = Deck[i];
                Deck[i] = Deck[newNumb];
                Deck[newNumb] = temp;

            }

            // Pull out 8 random cards 

            // divide the rest between two players 
        }

        /// <summary>
        /// play game 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool PlayCards(int player, Card card)
        {
            return false;
        }


        /// <summary>
        /// clear board and start again
        /// </summary>
        public void ClearBoard()
        {

        }

        public void checkWinner()
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
            // Test Displaying the deck 
           foreach (Card item in Deck)
            {
                Console.WriteLine("Suit: " + item.Suit + " Face: " + item.Face);
            }


        }



    }


}
