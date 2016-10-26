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

		public void Use (Player p)
		{
			if (!_used) {
				p.IncreaseHealth (6);
				_used = true;
				ItemGraphic.bmp = SwinGame.BitmapNamed ("potionUsed");
			}
		}

		public void Fill ()
		{
			_used = true;
			ItemGraphic.bmp = SwinGame.BitmapNamed ("potion");
		}
	}
}
