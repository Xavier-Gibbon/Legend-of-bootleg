using System;
using System.Collections.Generic;
namespace MyGame
{
	public class Inventory
	{
		private List<Item> _items = new List<Item>();

		/// <summary>
		/// Adds the item.
		/// </summary>
		/// <param name="theItem">The item.</param>
		public void AddItem (Item theItem)
		{
			_items.Add (theItem);
		}

		/// <summary>
		/// Fetchs the item at the index.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="index">Index.</param>
		public Item FetchItem (int index)
		{
			return _items [index];
		}

		/// <summary>
		/// Gets the number of items in the inventory.
		/// </summary>
		/// <returns>The number of items</returns>
		public int Count ()
		{
			return _items.Count;
		}
	}
}
