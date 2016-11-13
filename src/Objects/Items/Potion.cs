using System;
using SwinGameSDK;
namespace MyGame
{
	public class Potion : Item, ICanBeUsed
	{
		private bool _used;
		public Potion (string bitmapName) 
			: base (bitmapName)
		{
			_used = false;
		}

		/// <summary>
		/// The potion will increase the players health, but only if it hasn't been used up yet
		/// </summary>
		/// <param name="p">P.</param>
		public void Use (Player p)
		{
			if (!_used) {
				p.IncreaseHealth (6);
				_used = true;
				ItemGraphic.bmp = SwinGame.BitmapNamed ("potionUsed");
			}
		}

		/// <summary>
		/// Fill the potion up.
		/// </summary>
		public void Fill ()
		{
			_used = true;
			ItemGraphic.bmp = SwinGame.BitmapNamed ("potion");
		}
	}
}
