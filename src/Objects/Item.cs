using System;
using SwinGameSDK;
namespace MyGame
{
	public abstract class Item
	{
		MenuItem _itemGraphic;
		public static int ItemID;

		public Item (Bitmap bmp)
		{
			_itemGraphic = new MenuItem (bmp);
		}

		public static Item CreateItem (string type)
		{
			return (Item)Activator.CreateInstance (Type.GetType (type));
		}

		public void Save (MySqlConnector theConnector)
		{
			string command = "insert into Item (ItemID, ItemType) values (" + ItemID + ", '" + this.GetType() + "');";

			theConnector.NonQuery (command);
		}

		public Item (string bmpName) 
			: this (SwinGame.BitmapNamed (bmpName)) { }

		/// <summary>
		/// Gets the graphic of the item
		/// </summary>
		/// <value>The item graphic.</value>
		public MenuItem ItemGraphic {
			get {
				return _itemGraphic;
			}
		}
	}
}
