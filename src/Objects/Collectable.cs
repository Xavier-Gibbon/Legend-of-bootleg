using System;
namespace MyGame
{
	public abstract class Collectable : GameObject
	{
		public Collectable (string bmp, string anim, int x, int y)
			: base (bmp, anim, Direction.None, x, y, 0, SpriteState.Stationary) 
		{
		}

		/// <summary>
		/// Has the player collect the object
		/// </summary>
		/// <param name="p">The player.</param>
		public abstract void Collect (Player p);

		/// <summary>
		/// Most collectables will have the same animation
		/// </summary>
		/// <returns>The next animation.</returns>
		protected override string DetermineNextAnimation ()
		{
			return "collectable";
		} 
	}
}