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

		/// <summary>
		/// Draw the bitmap.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void Draw (int x, int y)
		{
			SwinGame.DrawBitmap (_bmp, x, y);
		}

		/// <summary>
		/// Gets or sets the bmp.
		/// </summary>
		/// <value>The bmp.</value>
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
