using System;
using SwinGameSDK;
namespace MyGame
{
	public class Sword : Item, ICanBeUsed
	{
		public Sword (string bitmapName) 
			: base (bitmapName)
		{
		}

		/// <summary>
		/// When the sword is used, it creates a sword projectile
		/// </summary>
		/// <param name="p">The player</param>
		public void Use (Player p)
		{
			if (p.State != SpriteState.Attacking) {
				p.StartAttack ();

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
				p.CurrentScreen.AddObject (new SwordProjectile (SwinGame.BitmapName(ItemGraphic.bmp) + "Projectile", p.direct, x, y, 2, p.FullHealth), false);
			}
		}
	}
}
