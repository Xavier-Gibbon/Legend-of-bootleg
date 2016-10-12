using System;
using SwinGameSDK;
namespace MyGame
{
	public abstract class GameObject
	{
		protected Sprite _sprite;
		protected Direction _direct;

		public GameObject (Bitmap bmp, AnimationScript anim, Direction direct, float x, float y)
		{
			_sprite = new Sprite (bmp, anim);
			_direct = direct;
			_sprite.Position.X = x;
			_sprite.Position.Y = y;
		}
		public GameObject (string bmp, string anim, Direction direct, float x, float y)
			: this (SwinGame.BitmapNamed (bmp), SwinGame.AnimationScriptNamed (anim), direct, x, y) { }

		/// <summary>
		/// Moves the object in the specified direction
		/// </summary>
		/// <param name="direct">The direction the object will move in</param>
		public abstract void Move (Direction direct);

		/// <summary>
		/// Draws the object
		/// </summary>
		public void Draw ()
		{
			_sprite.Draw ();
		}

		/// <summary>
		/// Performs the action.
		/// </summary>
		public abstract void PerformAction ();

		/// <summary>
		/// Checks if the object has collided with another object
		/// </summary>
		/// <returns><c>true</c>, if it has collided, <c>false</c> if it has not.</returns>
		/// <param name="obj">Object.</param>
		public bool CollidedWith (GameObject obj)
		{
			return false;
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

		protected abstract string DetermineNextAnimation (); 


		public Point2D Position {
			get {
				return _sprite.Position;
			}
		}

		public Sprite theSprite {
			get {
				return _sprite;
			}
		}
	}
}
