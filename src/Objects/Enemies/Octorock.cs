﻿using System;
using SwinGameSDK;
namespace MyGame
{
	public class Octorock : Enemy, ICanAttack
	{
		private bool _stop = false;
		private bool _hasAttacked = false;

		public Octorock (float x, float y, Direction direct, int health) 
			: base ("octorock", "spriteAnimation", direct, x, y, 2, health, 1)
		{
		}

		public Octorock (float x, float y) 
			: this (x, y, Utilities.RandomDirection (), 3) { }

		/// <summary>
		/// Octorocks will only drop rupees of values 1 and 5
		/// </summary>
		public override Collectable Drop ()
		{
			Collectable theDrop = null;
			Random dropChance = new Random ();

			if (dropChance.NextDouble () > 0.8) {
				int value;
				double valueChance = dropChance.NextDouble ();

				if (valueChance > 0.8) {
					value = 5;
				} else {
					value = 1;
				}

				theDrop = new Rupee (_sprite.X, _sprite.Y, value);
			}

			return theDrop;
		}

		/// <summary>
		/// Octorocks will move most times but can stop
		/// </summary>
		protected override void DetermineNextMove ()
		{
			if (_hitTimer.Ticks != 0) {
				Move (_forcedDirect, 7);
				if (_hitTimer.Ticks > 250) {
					_hitTimer.Stop ();
					_forcedDirect = Direction.None;
					_state = SpriteState.Moving;
				}
			} else {
				if (_moveTimer.Ticks > _moveTimerLimit) {
					_direct = Utilities.RandomDirection ();

					if (_direct == Direction.None) {
						do {
							_direct = Utilities.RandomDirection ();
						} while (_direct == Direction.None);
						_stop = true;
						_hasAttacked = false;

					} else {
						_stop = false;
					}

					_moveTimer.Reset ();
					_moveTimerLimit = SwinGame.Rnd (1500) + 500;
				}

				if (!_stop)
					Move (_direct);
			}
		}

		public bool CheckAttack ()
		{
			return (_moveTimer.Ticks > (_moveTimerLimit - 100) && _stop && !_hasAttacked);
		}

		public Projectile Attack ()
		{
			float x = _sprite.Position.X;
			float y = _sprite.Position.Y;

			switch (_direct) {
			case Direction.Up:
				y -= _sprite.Height;
				break;
			case Direction.Down:
				y += _sprite.Height;
				break;
			case Direction.Left:
				x -= _sprite.Width;
				break;
			case Direction.Right:
				x += _sprite.Width;
				break;
			}

			_hasAttacked = true;

			return new RockProjectile (_direct, x, y, 2);
		}
	}
}
