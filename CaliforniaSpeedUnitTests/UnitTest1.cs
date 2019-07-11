using Microsoft.VisualStudio.TestTools.UnitTesting;
using CaliforniaSpeedLibrary;

namespace CaliforniaSpeedUnitTests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
        Game game = new Game();
    }
        /// <summary>
        /// This method is the test the amount of cards that a player has in their hand at one time.
        /// If the Players tries to pull more than 5 cards it returns false else returns true. 
        /// </summary>
       /* public bool checkPlayerCardCount(ref List<Card> myHand)
        {
            //check the counr of my hand
            if (myHand.Count >= 5)
                return false;
            
            //Default return 
            return true;
        }
        */


  }
}
