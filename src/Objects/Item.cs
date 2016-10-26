using System;
using SwinGameSDK;
namespace MyGame
{
	public abstract class Item
	{
		MenuItem _itemGraphic;

		public Item (Bitmap bmp)
		{
			_itemGraphic = new MenuItem (bmp);
		}
		public Item (string bmpName) 
			: this (SwinGame.BitmapNamed (bmpName)) { }

		public MenuItem ItemGraphic {
			get {
				return _itemGraphic;
			}
		}
	}
}
