using System;
using NUnit.Framework;
namespace MyGame
{
	[TestFixture]
	public class TestCollectable
	{
		Player myPlayer;
		Screen myScreen;
		Rupee myRupee;
		Heart myHeart;

		[SetUp]
		public void SetUp ()
		{
			myScreen = new Screen (0, 0);
			myPlayer = new Player (0, 0, myScreen);
			myRupee = new Rupee (0, 0, 5);
			myHeart = new Heart (0, 0);

			myPlayer.DecreaseHealth (3);
		}

		[TearDown]
		public void TearDown ()
		{
			myScreen = null;
			myPlayer = null;
			myRupee = null;
			myHeart = null;
		}

		[Test]
		public void TestHeartCollect ()
		{
			myHeart.Collect (myPlayer);

			Assert.AreEqual (myPlayer.CurrentHealth, 5, "Test to see if the heart object increases the player's health by one");
		}

		[Test]
		public void TestRupeeCollect ()
		{
			myRupee.Collect (myPlayer);

			Assert.AreEqual (myPlayer.RupeeCount, 5, "Test to see if the rupee object increases the player's rupee count by its value");
		}
	}
}
