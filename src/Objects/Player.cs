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
		private Screen _currentScreen;
		private Timer _spriteTimer = new Timer ();

		public Player (float x, float y, int currentHealth, int maxHealth, SpriteState state, string bitmapName, Screen firstScreen) : base (bitmapName, "spriteAnimation", Direction.Up, x, y, 3)
		{
			_maxHealth = maxHealth;
			_currentHealth = currentHealth;
			_state = state;
			_currentScreen = firstScreen;
		}

		public Player (float x, float y, Screen firstScreen) : this (x, y, 6, 6, SpriteState.Stationary, "link", firstScreen) { }

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
			_currentHealth -= damage;
			if (_currentHealth < 0) {
				_currentHealth = 0;
			}
		}

		/// <summary>
		/// Increases the players max health.
		/// </summary>
		/// <param name="healthIncrement">Amount of max health gained.</param>
		public void IncreaseMaxHealth (int healthIncrement)
		{
			_maxHealth += healthIncrement;
		}

		public override void Update ()
		{
			base.Update ();
			if (_sprite.AnimationHasEnded) {
				if (_state == SpriteState.Attacking) _state = SpriteState.Stationary;
			}
		}

		private void CheckTimers ()
		{
			
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
	}
}
