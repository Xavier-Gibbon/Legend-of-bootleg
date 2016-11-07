using System;
namespace MyGame
{
	public class Heart : Collectable
	{
		public Heart (int x, int y) 
			: base("heart", "spriteAnimation", x, y)
		{
		}

		public override void Collect (Player p)
		{
			p.IncreaseHealth (2);
		}
	}
}
