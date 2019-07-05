using System;
using System.Collections.Generic;

namespace CaliforniaSpeedLibrary
{
  public class Game
  {


        public Card[,] play = new Card[2, 4];
        public List<Card> player1 = new List<Card>();
        public List<Card> player2 = new List<Card>();


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
            King,
        }
        public class Card
        { 
            public Suit Suit { get; set; }
            public Face Face { get; set; }
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
        public void DistreputeCards()
        {

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



    }


}
