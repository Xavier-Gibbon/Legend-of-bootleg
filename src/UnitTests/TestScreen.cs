using System;
using NUnit.Framework;
namespace MyGame
{
	[TestFixture]
	public class TestScreen
	{
		Screen myScreen;
		Screen myScreen2;

		[SetUp]
		public void SetUp ()
		{
			myScreen = new Screen (0, 1);
			myScreen2 = new Screen (0, 0);
		}

		[TearDown]
		public void TearDown ()
		{
			myScreen = null;
			myScreen2 = null;
		}

		[Test]
		public void TestAddPath ()
		{
			myScreen.AddNewPath (myScreen2, Direction.Left);

			Assert.AreEqual (myScreen2, myScreen.Directions [Direction.Left]);
		}
	}
}
