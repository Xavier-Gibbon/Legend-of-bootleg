﻿using System;
namespace MyGame
{
	public class Bow : Item, ICanBeUsed
	{
		private int _arrowCount;
		public Bow (string bmp) 
			: base (bmp)
		{
			_arrowCount = 30;
		}
		public Bow () : this ("bow") {}

		/// <summary>
		/// The bow will create an arrow projectile depending on the players position and direction
		/// </summary>
		/// <param name="p">The player.</param>
		public void Use (Player p)
		{
			if (_arrowCount != 0) {
				_arrowCount--;
				int x = (int) p.theSprite.Position.X;
				int y = (int) p.theSprite.Position.Y;
				switch (p.direct) {
				case Direction.Up:
					y -= p.theSprite.Height;
					break;
				case Direction.Down:
					y += p.theSprite.Height;
					break;
				case Direction.Left:
					x -= p.theSprite.Width;
					break;
				case Direction.Right:
					x += p.theSprite.Width;
					break;
				}
				p.CurrentScreen.AddObject (new ArrowProjectile (p.direct, x, y, 3), false);
			}
		}
	}
}
