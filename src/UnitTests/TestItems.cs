﻿using System;
using NUnit.Framework;
namespace MyGame
{
	[TestFixture]
	public class TestItems
	{
		Player myPlayer;
		Sword mySword;
		Potion myPotion;

		[SetUp]
		public void SetUp ()
		{
			myPlayer = new Player (0, 0, 6, 10, SpriteState.Stationary, "link");
			mySword = new Sword("sword");
			myPotion = new Potion ("potion");
		}

		[TearDown]
		public void TearDown ()
		{
			myPlayer = null;
			mySword = null;
			myPotion = null;
		}

		[Test]
		public void TestSwordUse ()
		{
			mySword.Use (myPlayer);

			Assert.AreEqual (SpriteState.Attacking, myPlayer.State, "Test to see if the sword can be used");
		}

		[Test]
		public void TestPotionUse ()
		{
			myPotion.Use (myPlayer);

			Assert.AreEqual (10, myPlayer.CurrentHealth, "Test to see if the potion can be used");
		}

		[Test]
		public void TestPlayerUse ()
		{
			myPlayer.EquipFirstItem (mySword);
			myPlayer.UseFirstItem ();

			Assert.AreEqual (SpriteState.Attacking, myPlayer.State, "Test to see if the player can use equipped items");
		}
	}
}
