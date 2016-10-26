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

		public void Use (Player p)
		{
			p.State = SpriteState.Attacking;
		}
	}
}
