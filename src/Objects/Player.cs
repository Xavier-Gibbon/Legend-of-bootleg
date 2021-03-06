﻿using System;
using System.Collections.Generic;
using SwinGameSDK;
namespace MyGame
{
	public class Player : GameObject
	{
		public static int PlayerID;

		private int _maxHealth;
		private int _currentHealth;
		private int _rupeeCount;
		private Inventory _inventory = new Inventory();
		private ICanBeUsed _equippedItem1;
		private ICanBeUsed _equippedItem2;
		private Screen _currentScreen;
		private Timer _attackTimer = new Timer ();
		private Timer _hitTimer = new Timer ();

		public Player (int x, int y, int currentHealth, int maxHealth, SpriteState state, string bitmapName, string anim, Screen firstScreen, int rupeeCount) 
			: base (bitmapName, anim, Direction.Up, x, y, 3, state)
		{
			_maxHealth = maxHealth;
			_currentHealth = currentHealth;
			_state = state;
			_currentScreen = firstScreen;
			_rupeeCount = rupeeCount;
		}

		public Player (int x, int y, Screen firstScreen) : this (x, y, 6, 6, SpriteState.Stationary, "link", "spriteAnimation", firstScreen, 0) { }

		public Player () { }

		public override void Save (MySqlConnector theConnector)
		{
			base.Save (theConnector);

			string command = "Insert into Player (PlayerID, ObjectID, CurrentHealth, MaxHealth, RupeeCount) values (" + PlayerID + ", " + ObjectID + ", " + _currentHealth + ", " + _maxHealth + ", " + _rupeeCount + ");";

			theConnector.NonQuery (command);

			for (int i = 0; i < _inventory.Count (); i++) {
				_inventory.FetchItem (i).Save(theConnector);

				command = "insert into PlayerInventory (PlayerID, ItemID, EquippedSlot) values (" + PlayerID + ", " + Item.ItemID + ", ";
				if (_equippedItem1 == _inventory.FetchItem (i)) {
					command += "1";
				} else if (_equippedItem2 == _inventory.FetchItem (i)) {
					command += "2";
				} else {
					command += "0";
				}

				command += ");";

				theConnector.NonQuery (command);
				Item.ItemID++;
			}

			PlayerID++;
		}

		public override void Load (MySqlConnector theConnector, int ObjectID)
		{
			base.Load (theConnector, ObjectID);

			string command = "select * from Player where ObjectID = " + ObjectID + ";";
			List<List<string>> data = theConnector.Select (command);


			int i;

			int.TryParse (data [2] [0], out i);
			_currentHealth = i;

			int.TryParse (data [3] [0], out i);
			_maxHealth = i;

			int.TryParse (data [4] [0], out i);
			_rupeeCount = i;
		}

		public void LoadItems (MySqlConnector theConnector)
		{
			string command = "select * from PlayerInventory natural join Item;";
			List<List<string>> data = theConnector.Select (command);

			for (int i = 0; i < data [0].Count; i++) {
				Item theItem = Item.CreateItem (data [3] [i]);

				_inventory.AddItem (theItem);

				if (data [2] [i] == "1") {
					EquipFirstItem (theItem as ICanBeUsed);
				} else if (data [2] [i] == "2") {
					EquipSecondItem (theItem as ICanBeUsed);
				}
			}
		}

		/// <summary>
		/// Uses the first equipped item
		/// </summary>
		public void UseFirstItem ()
		{
			if (_equippedItem1 != null) {
				_equippedItem1.Use (this);
			}
		}

		/// <summary>
		/// Uses the second equipped item.
		/// </summary>
		public void UseSecondItem ()
		{
			if (_equippedItem2 != null) {
				_equippedItem2.Use (this);
			}
		}

		/// <summary>
		/// Equips the item.
		/// </summary>
		/// <param name="theItem">The item.</param>
		public void EquipFirstItem (ICanBeUsed theItem)
		{
			_equippedItem1 = theItem;
		}

		/// <summary>
		/// Equips the second item.
		/// </summary>
		/// <param name="theItem">The item.</param>
		public void EquipSecondItem (ICanBeUsed theItem)
		{
			_equippedItem2 = theItem;
		}

		/// <summary>
		/// The player will still be able to move even when hit.
		/// </summary>
		/// <param name="direct">Direct.</param>
		/// <param name="speed">Speed.</param>
		public override void Move (Direction direct, int speed)
		{
			base.Move (direct, speed);
			if (_state == SpriteState.Hit) {
				if (direct != Direction.None)
					_direct = direct;

			}
		}

		/// <summary>
		/// Makes the player attack. Mostly changes the state of the player.
		/// </summary>
		public void StartAttack ()
		{
			_state = SpriteState.Attacking;
			_attackTimer.Start ();
		}

		/// <summary>
		/// Increases the players health.
		/// </summary>
		/// <param name="healthIncrement">Amount of health gained.</param>
		public void IncreaseHealth (int healthIncrement)
		{
			_currentHealth += healthIncrement;
			if (_currentHealth > _maxHealth) {
				_currentHealth = _maxHealth;
			}
		}

		/// <summary>
		/// Decreases the players health.
		/// </summary>
		/// <param name="damage">Amount of damage.</param>
		public void DecreaseHealth (int damage)
		{
			if (_state != SpriteState.Hit) {
				_currentHealth -= damage;
				if (_currentHealth < 0) {
					_currentHealth = 0;
				}
				_hitTimer.Start ();
				_state = SpriteState.Hit;
			}
		}

		/// <summary>
		/// Increases the players max health.
		/// </summary>
		/// <param name="healthIncrement">Amount of max health gained.</param>
		public void IncreaseMaxHealth (int healthIncrement)
		{
			_maxHealth += healthIncrement;
			IncreaseHealth (healthIncrement);
		}

		/// <summary>
		/// Update the player.
		/// </summary>
		public override void Update ()
		{
			base.Update ();
			CheckTimers ();
		}

		/// <summary>
		/// Checks the players 2 timers.
		/// </summary>
		private void CheckTimers ()
		{
			if (_attackTimer.Ticks > 200) {
				_attackTimer.Stop ();
				if (_state == SpriteState.Attacking)
					_state = SpriteState.Stationary;
			}

			if (_hitTimer.Ticks > 1000) {
				_hitTimer.Stop ();

				if (_state == SpriteState.Hit)
					_state = SpriteState.Stationary;
			}
		}

		/// <summary>
		/// Creates a string to create an animation
		/// </summary>
		/// <returns>The next animation name</returns>
		protected override string DetermineNextAnimation ()
		{
			string result = "link";
			result += _direct.ToString ();

			switch (_state) {
			case SpriteState.Attacking:
				result += "Attack";
				break;
			case SpriteState.Moving:
				result += "Move";
				break;
			}

			return result;
		}


		/// <summary>
		/// Gets the players inventory.
		/// </summary>
		/// <value>The players inventory.</value>
		public Inventory Inventory {
			get {
				return _inventory;
			}
		}

		/// <summary>
		/// Gets the players current health.
		/// </summary>
		/// <value>The players current health.</value>
		public int CurrentHealth {
			get {
				return _currentHealth;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this player has full health.
		/// </summary>
		/// <value><c>true</c> if full health; otherwise, <c>false</c>.</value>
		public bool FullHealth {
			get {
				return _currentHealth == _maxHealth;
			}
		}

		/// <summary>
		/// Gets or sets the players current screen.
		/// </summary>
		/// <value>The players current screen.</value>
		public Screen CurrentScreen {
			get {
				return _currentScreen;
			}

			set {
				_currentScreen = value;
			}
		}

		/// <summary>
		/// Gets or sets the rupee count.
		/// </summary>
		/// <value>The players rupee count.</value>
		public int RupeeCount {
			get {
				return _rupeeCount;
			}

			set {
				_rupeeCount = value;
			}
		}
	}
}
