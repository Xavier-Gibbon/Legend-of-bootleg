using System;
namespace MyGame
{
	public class RockProjectile : Projectile
	{
		public RockProjectile (Direction direct, int x, int y, int damage)
			: base ("rock", "spriteAnimation", direct, x, y, 5, damage)
		{
		}

		public RockProjectile () { }

		/// <summary>
		/// The rock projectile will always be deleted
		/// </summary>
		/// <returns>true</returns>
		public override bool CanBeDeleted ()
		{
			return true;
		}

		protected override string DetermineNextAnimation ()
		{
			return "rock";
		}
	}
}
