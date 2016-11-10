using System;
using SwinGameSDK;
namespace MyGame
{
	public class Darknut : Enemy
	{
		public Darknut (float x, float y, Direction direct, int health) 
			: base ("darknut", "spriteAnimation", direct, x, y, 2, health, 2)
		{
		}

		public Darknut (float x, float y) 
			: this (x, y, Utilities.RandomDirection (), 6) {}

		/// <summary>
		/// A darknut will not take damage if it hits it from the front
		/// </summary>
		/// <param name="damage">The amount of damage taken.</param>
		/// <param name="direct">The direction that the damage came from.</param>
		public override void DecreaseHealth (int damage, Direction direct)
		{
			if (_direct != Utilities.ReverseDirection (direct)) {
				base.DecreaseHealth (damage, direct);
			}
		}

		/// <summary>
		/// The darknut can drop either a heart or a rupee of any value
		/// </summary>
		public override Collectable Drop ()
		{
			Collectable theDrop = null;
			Random dropChance = new Random();

			if (dropChance.NextDouble () > 0.7) {
				theDrop = new Heart (_sprite.X, _sprite.Y);
			} else if (dropChance.NextDouble () > 0.75) {
				int value;
				double valueChance = dropChance.NextDouble ();

				if (valueChance > 0.9) {
					value = 20;
				} else if (valueChance > 0.6) {
					value = 5;
				} else {
					value = 1;
				}

				theDrop = new Rupee (_sprite.X, _sprite.Y, value);
			}

			return theDrop;
		}

		/// <summary>
		/// A darknut will move constantly.
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
					do {
						_direct = Utilities.RandomDirection ();
					} while (_direct == Direction.None);

					_moveTimer.Reset ();
					_moveTimerLimit = SwinGame.Rnd (1500) + 500;
				}

				Move (_direct);
			}
						
		}
	}
}
