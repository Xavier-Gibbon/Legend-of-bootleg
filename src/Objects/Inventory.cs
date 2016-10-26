using System;
using System.Collections.Generic;
namespace MyGame
{
	public class Inventory
	{
		private List<Item> _items = new List<Item>();

		public void AddItem (Item theItem)
		{
			_items.Add (theItem);
		}

		public Item FetchItem (int index)
		{
			return _items [index];
		}

		public int Count ()
		{
			return _items.Count;
		}
	}
}
