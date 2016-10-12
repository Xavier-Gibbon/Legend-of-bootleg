using System;
using SwinGameSDK;
using NUnit.Framework;
namespace MyGame
{
	[TestFixture]
	public class TestPlayer
	{
		Player myPlayer;
		[SetUp]
		public void SetUp ()
		{
			myPlayer = new Player (400, 400);
		}

		[TearDown]
		public void TearDown ()
		{
			myPlayer = null;
		}

		[Test]
		public void TestPlayerMove ()
		{
			Point2D expected = new Point2D ();
			expected.X = 397;
			expected.Y = 400;

			myPlayer.Move (Direction.Left);
			Point2D actual = myPlayer.Position;

			Assert.AreEqual (expected, actual, "Test to see if the player can move properly");

		}

		[Test]
		public void TestPlayerUpdate ()
		{
			myPlayer.Move (Direction.Up);
			myPlayer.Update ();

			string actual = myPlayer.theSprite.animationName ();

			Assert.AreEqual ("linkUpMove", actual, "Test to see if the player can be updated");
		}
	}
}
