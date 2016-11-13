using System;
using System.Collections.Generic;
using SwinGameSDK;
namespace MyGame
{
	public abstract class Enemy : GameObject
	{
		public static int EnemyID;

		protected int _health;
		protected int _damage;
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

		public Enemy () 
		{
			_moveTimer.Start ();
			_moveTimerLimit = SwinGame.Rnd (1500) + 500;
		}

		public override void Save (MySqlConnector theConnector)
		{
			base.Save (theConnector);

			string command = "insert into Enemy (EnemyID, ObjectID, Health, Damage) values (" + EnemyID + ", " + ObjectID + ", " + _health + ", " + _damage + ");";

			theConnector.NonQuery (command);
			EnemyID++;
		}

		public override void Load (MySqlConnector theConnector, int ObjectID)
		{
			base.Load (theConnector, ObjectID);

			string command = "select * from Enemy where ObjectID = " + ObjectID + ";";
			List<List<string>> data = theConnector.Select (command);

			int i;

			int.TryParse (data [2] [0], out i);
			_health = i;

			int.TryParse (data [3] [0], out i);
			_damage = i;
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

		/// <summary>
		/// Most enemies will have the same animation.
		/// </summary>
		/// <returns>The next animation.</returns>
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
		}
	}
}
