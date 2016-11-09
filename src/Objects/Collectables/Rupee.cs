using System;
namespace MyGame
{
	public class Rupee : Collectable
	{
		private int _value;
		public Rupee (float x, float y, int value)
			: base("rupee" + value, "spriteAnimation", x, y)
		{
			_value = value;
		}

		public override void Collect (Player p)
		{
			p.RupeeCount += _value;
		}
	}
}
