using System;
namespace MyGame
{
	public class Bow : Item, ICanBeUsed
	{
		private int _arrowCount;
		public Bow () 
			: base ("bow")
		{
			_arrowCount = 30;
		}

		public void Use (Player p)
		{
			if (_arrowCount != 0) {
				_arrowCount--;
				float x = p.theSprite.Position.X;
				float y = p.theSprite.Position.Y;
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
				p.CurrentScreen.AddObject (new ArrowProjectile (p.direct, x, y, 3));
			}
		}
	}
}
