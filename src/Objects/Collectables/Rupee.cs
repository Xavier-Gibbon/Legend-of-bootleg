using System;
namespace MyGame
{
	public class Rupee : Collectable
	{
		private int _value;
		public Rupee (string bmp, string anim, int x, int y, int value)
			: base(bmp, anim, x, y)
		{
			_value = value;
		}

		public Rupee (int x, int y, int value) : this ("rupee" + value, "spriteAnimation", x, y, value) { }

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
