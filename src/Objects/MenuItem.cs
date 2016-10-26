using System;
using SwinGameSDK;
namespace MyGame
{
	/// <summary>
	/// A class made to be drawn onto a menu
	/// </summary>
	public class MenuItem
	{
		private Bitmap _bmp;
		public MenuItem (Bitmap bmp)
		{
			_bmp = bmp;
		}

		public void Draw (int x, int y)
		{
			SwinGame.DrawBitmap (_bmp, x, y);
		}

		public Bitmap bmp {
			get 
			{
				return _bmp;
			}
			set 
			{
				_bmp = value;
			}
		}
	}
}
