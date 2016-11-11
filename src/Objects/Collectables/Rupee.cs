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
		public override void Collect (Player p)
		{
			p.RupeeCount += _value;
		}
	}
}
