using System;
namespace MyGame
{
	public class Rupee : Collectable
	{
		public Rupee (string bmp, string anim, int x, int y, int value)
			: base(bmp, anim, x, y, value)
		{
		}

		public Rupee (int x, int y, int value) : this ("rupee" + value, "spriteAnimation", x, y, value) { }

		public Rupee () { }
		/// <summary>
		/// The rupee will increase the player's rupee count by its value
		/// </summary>
		/// <param name="p">The player</param>
		public override void Collect (Player p)
		{
			p.RupeeCount += _value;
		}
	}
}
