using System;
namespace MyGame
{
	public class Heart : Collectable
	{
		public Heart (float x, float y) 
			: base("heart", "spriteAnimation", x, y)
		{
		}

		public override void Collect (Player p)
		{
			p.IncreaseHealth (2);
		}
	}
}
