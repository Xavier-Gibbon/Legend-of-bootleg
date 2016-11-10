using System;
using SwinGameSDK;
namespace MyGame
{
	public abstract class GameObject
	{
		protected Sprite _sprite;
		protected Direction _direct;
		protected SpriteState _state;
		protected int _speed;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MyGame.GameObject"/> class.
		/// </summary>
		/// <param name="bmp">The objects Bitmap.</param>
		/// <param name="anim">The objects Animation script.</param>
		/// <param name="direct">The objects Direction.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="speed">The objects Speed.</param>
		public GameObject (Bitmap bmp, AnimationScript anim, Direction direct, float x, float y, int speed)
		{
			_sprite = new Sprite (bmp, anim);
			_speed = speed;
			_direct = direct;
			_sprite.Position.X = x;
			_sprite.Position.Y = y;
		}
		public GameObject (string bmp, string anim, Direction direct, float x, float y, int speed)
			: this (SwinGame.BitmapNamed (bmp), SwinGame.AnimationScriptNamed (anim), direct, x, y, speed) { }

		/// <summary>
		/// Moves the object in the specified direction
		/// </summary>
		/// <param name="direct">The direction the object will move in</param>
		public virtual void Move (Direction direct)
		{
			Move (direct, _speed);
		}

		/// <summary>
		/// Move the object in the specified direction and speed.
		/// </summary>
		/// <param name="direct">The direction the object will move in</param>
		/// <param name="speed">The speed at which the object will move at.</param>
		public virtual void Move (Direction direct, int speed)
		{
			if (_state != SpriteState.Attacking) {
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

				if (_state != SpriteState.Hit) {
					if (direct != Direction.None) {
						_direct = direct;
						_state = SpriteState.Moving;
					} else {
						_state = SpriteState.Stationary;
					}
				}

				_sprite.Move ();
			}
		}

		/// <summary>
		/// Draws the object
		/// </summary>
		public void Draw ()
		{
			_sprite.Draw ();
		}

		/// <summary>
		/// Checks if the object has collided with another object
		/// </summary>
		/// <returns><c>true</c>, if it has collided, <c>false</c> if it has not.</returns>
		/// <param name="obj">Object.</param>
		public bool CollidedWith (GameObject obj)
		{
			return SwinGame.SpriteCollision (_sprite, obj.theSprite);
		}


		/// <summary>
		/// Updates the object
		/// </summary>
		public virtual void Update ()
		{
			string anim = DetermineNextAnimation ();

			if (anim == _sprite.animationName ()) {
				_sprite.UpdateAnimation ();
			} else {
				_sprite.StartAnimation (anim);
			}
		}

		/// <summary>
		/// Determines the next animation for the object.
		/// </summary>
		/// <returns>The next animation.</returns>
		protected abstract string DetermineNextAnimation (); 

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <value>The position.</value>
		public Point2D Position {
			get {
				return _sprite.Position;
			}
		}

		/// <summary>
		/// Gets the sprite.
		/// </summary>
		/// <value>The sprite.</value>
		public Sprite theSprite {
			get {
				return _sprite;
			}
		}

		/// <summary>
		/// Gets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public Direction direct {
			get {
				return _direct;
			}
		}

		/// <summary>
		/// Gets or sets the sprite state.
		/// </summary>
		/// <value>The state.</value>
		public SpriteState State {
			get {
				return _state;
			}

			set {
				_state = value;
			}
		}
	}
}
