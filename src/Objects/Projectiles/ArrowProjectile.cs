using System;
namespace MyGame
{
	public class ArrowProjectile : Projectile
	{
		public ArrowProjectile (Direction direct, int x, int y, int damage) 
			: base ("arrowProjectile", "spriteAnimation", direct, x, y, 5, damage)
		{
		}

		public ArrowProjectile () { }

		/// <summary>
		/// Arrows will always be able to be deleted
		/// </summary>
		/// <returns>true</returns>
		public override bool CanBeDeleted ()
		{
			return true;
		}
	}
}
