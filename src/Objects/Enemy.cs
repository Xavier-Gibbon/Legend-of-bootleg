using System;
using SwinGameSDK;
namespace MyGame
{
	public abstract class Enemy : GameObject
	{
		protected int _health;
		private int _damage;
		protected Direction _forcedDirect;
		protected Timer _hitTimer = new Timer ();
		protected Timer _moveTimer = new Timer ();
		protected int _moveTimerLimit;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MyGame.Enemy"/> class.
		/// </summary>
		/// <param name="bmp">The Enemy's Bitmap.</param>
		/// <param name="anim">The Enemy's Animation.</param>
		/// <param name="direct">The Enemy's Direction.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="speed">The Enemy's Speed.</param>
		/// <param name="health">The Enemy's Health.</param>
		public Enemy (string bmp, string anim, Direction direct, int x, int y, int speed, SpriteState state, int health, int damage) 
			: base(bmp, anim, direct, x, y, speed, state)
		{
			_health = health;
			_damage = damage;
			_moveTimer.Start ();
			_moveTimerLimit = SwinGame.Rnd (1500) + 500;
		}

		/// <summary>
		/// Returns the collectable that the enemy will drop
		/// </summary>
		public abstract Collectable Drop ();

		/// <summary>
		/// Decreases the enemy's health.
		/// </summary>
		/// <param name="damage">The Damage taken.</param>
		/// <param name="direct">The Direction from the source of the damage.</param>
		public virtual void DecreaseHealth (int damage, Direction direct)
		{
			if (_state != SpriteState.Hit) {
				_health -= damage;
				_forcedDirect = direct;

				if (_health <= 0) {
					_state = SpriteState.Dead;
				} else {
					_state = SpriteState.Hit;
					_hitTimer.Start ();
				}
			}
		}

		/// <summary>
		/// Updates the enemy. An enemy will move when it is updated
		/// </summary>
		public override void Update ()
		{
			DetermineNextMove ();
			base.Update ();
		}

		protected override string DetermineNextAnimation ()
		{
			return "enemy" + _direct;
		}

		/// <summary>
		/// Determines the movement of the enemy.
		/// </summary>
		protected abstract void DetermineNextMove ();

		public int Damage {
			get {
				return _damage;
			}

			set {
				_damage = value;
			}
		}
	}
}
