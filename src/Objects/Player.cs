using System;
using SwinGameSDK;
namespace MyGame
{
	public class Player : GameObject
	{
		private int _maxHealth;
		private int _currentHealth;
		private Inventory _inventory = new Inventory();
		private ICanBeUsed _equippedItem1;
		private ICanBeUsed _equippedItem2;
		private SpriteState _state;

		public Player (float x, float y, int currentHealth, int maxHealth, SpriteState state, string bitmapName) : base (bitmapName, "spriteAnimation", Direction.Up, x, y)
		{
			_maxHealth = maxHealth;
			_currentHealth = currentHealth;
			_state = state;
		}

		public Player (float x, float y) : this (x, y, 6, 6, SpriteState.Stationary, "link") { }

		/// <summary>
		/// Moves the player in the specified direction
		/// </summary>
		/// <param name="direct">The direction the player will move</param>
		public override void Move (Direction direct)
		{
			Move (direct, 3);
		}

		/// <param name="speed">The speed of which the player moves</param>
		public void Move (Direction direct, int speed)
		{
			if (_state != SpriteState.Attacking) 
			{
				Vector tempVector = new Vector ();
				tempVector.X = 0;
				tempVector.Y = 0;

				switch (direct) {
				case Direction.Up:
					tempVector.Y = -speed;
					break;
				case Direction.Down:
					tempVector.Y = speed;
					break;
				case Direction.Left:
					tempVector.X = -speed;
					break;
				case Direction.Right:
					tempVector.X = speed;
					break;
				}

				_sprite.Velocity = tempVector;

				if (direct != Direction.None) {
					_direct = direct;
					_state = SpriteState.Moving;
				} else {
					_state = SpriteState.Stationary;
				}

				_sprite.Move ();
			}
		}

		/// <summary>
		/// Will use a specified item
		/// </summary>
		public override void PerformAction ()
		{
			throw new NotImplementedException ();
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

		public void IncreaseHealth (int healthIncrement)
		{
			_currentHealth += healthIncrement;
			if (_currentHealth > _maxHealth) {
				_currentHealth = _maxHealth;
			}
		}

		public void DecreaseHealth (int damage)
		{
			_currentHealth -= damage;
			if (_currentHealth < 0) {
				_currentHealth = 0;
			}
		}

		public void IncreaseMaxHealth (int healthIncrement)
		{
			_maxHealth += healthIncrement;
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



		public Inventory Inventory {
			get {
				return _inventory;
			}

			set {
				_inventory = value;
			}
		}

		public SpriteState State {
			get {
				return _state;
			}

			set {
				_state = value;
			}
		}

		public int CurrentHealth {
			get {
				return _currentHealth;
			}
		}
	}
}
