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

		public abstract void Move (Direction direct);

		public void Draw ()
		{
			_sprite.Draw ();
		}

		public void PerformAction ()
		{
		}

		public bool CollidedWith (GameObject obj)
		{
			return false;
		}

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
