using System;
namespace MyGame
{
	public class RockProjectile : Projectile
	{
		public RockProjectile (Direction direct, float x, float y, int damage)
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
