using System;
using NUnit.Framework;
namespace MyGame
{
	[TestFixture]
	public class TestProjectile
	{
		Player myPlayer;
		Screen dummy;
		SwordProjectile mySword;
		ArrowProjectile myArrow;

		[SetUp]
		public void SetUp ()
		{
			dummy = new Screen (0, 0);
			myPlayer = new Player (0, 0, dummy);
			mySword = new SwordProjectile (null, Direction.None, 0, 0, 2, true);
			myArrow = new ArrowProjectile (Direction.None, 0, 0, 1);
		}

		[TearDown]
		public void TearDown ()
		{
			dummy = null;
			myPlayer = null;
			mySword = null;
			myArrow = null;
		}

		[Test]
		public void TestSwordDamage ()
		{
			myPlayer.DecreaseHealth (mySword.Damage);

			Assert.AreEqual (4, myPlayer.CurrentHealth, "Test to see if the sword projectile can damage the player");
		}

		[Test]
		public void TestArrowDamage ()
		{
			myPlayer.DecreaseHealth (myArrow.Damage);

			Assert.AreEqual (5, myPlayer.CurrentHealth, "Test to see if the arrow projectile can damage the player");
		}
	}
}
