using System;
namespace MyGame
{
	public abstract class Collectable : GameObject
	{
		public Collectable (string bmp, string anim, int x, int y)
			: base (bmp, anim, Direction.None, x, y, 0, SpriteState.Stationary) 
		{
		}

		public abstract void Collect (Player p);

		protected override string DetermineNextAnimation ()
		{
			return "collectable";
		} 
	}
}