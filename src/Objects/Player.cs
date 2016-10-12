using System;
using SwinGameSDK;
namespace MyGame
{
	public class Player : GameObject
	{
		private int _health;

		public Player (float x, float y) : base ("link", "spriteAnimation", Direction.Up, x, y)
		{
			_health = 6;
		}

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
			Vector tempVector = new Vector ();
			tempVector.X = 0;
			tempVector.Y = 0;
			if (direct == Direction.Up) {
				tempVector.Y = -speed;
			} else if (direct == Direction.Down) {
				tempVector.Y = speed;
			} else if (direct == Direction.Left) {
				tempVector.X = -speed;
			} else if (direct == Direction.Right) {
				tempVector.X = speed;
			}

			_sprite.Velocity = tempVector;

			if (direct != Direction.None) {
				_direct = direct;
			}

			_sprite.Move ();
		}

		/// <summary>
		/// Will use a specified item
		/// </summary>
		public override void PerformAction ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Creates a string to create an animation
		/// </summary>
		/// <returns>The next animation name</returns>
		protected override string DetermineNextAnimation ()
		{
			string result = "link";
			result += _direct.ToString ();
			if (_sprite.Velocity.X != 0 || _sprite.Velocity.Y != 0) {
				result += "Move";
			}
			return result;
		}
	}
}
