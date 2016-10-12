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

		//Moves the player
		public override void Move (Direction direct)
		{
			Move (direct, 3);
		}
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

		protected override string DetermineNextAnimation ()
		{
			string result = "link";
			result += _direct.ToString ();
			if (_sprite.Velocity.X != 0 || _sprite.Velocity.Y != 0) {
				result += "Move";
			}
			return result;
		}

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
