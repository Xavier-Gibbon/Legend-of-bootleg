using System;
namespace MyGame
{
	public class RockProjectile : Projectile
	{
		public RockProjectile (Direction direct, int x, int y, int damage)
			: base ("rock", "spriteAnimation", direct, x, y, 5, damage)
		{
		}

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
