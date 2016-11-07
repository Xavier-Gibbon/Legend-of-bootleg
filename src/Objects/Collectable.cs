using System;
namespace MyGame
{
	public abstract class Collectable : GameObject
	{
		public Collectable (string bmp, string anim, float x, float y)
			: base (bmp, anim, Direction.None, x, y, 0) 
		{
		}

		public abstract void Collect (Player p);

		protected override string DetermineNextAnimation ()
		{
			return "collectable";
		} 
	}
}