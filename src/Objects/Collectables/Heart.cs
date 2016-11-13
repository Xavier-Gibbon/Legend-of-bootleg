using System;
namespace MyGame
{
	public class Heart : Collectable
	{
		public Heart (string bmp, string anim, int x, int y) 
			: base(bmp, anim, x, y)
		{
		}

		public Heart (int x, int y) : this ("heart", "spriteAnimation", x, y) { }

		/// <summary>
		/// The heart will increase the players health by 2
		/// </summary>
		/// <param name="p">The player.</param>
		public override void Collect (Player p)
		{
			p.IncreaseHealth (2);
		}
	}
}
