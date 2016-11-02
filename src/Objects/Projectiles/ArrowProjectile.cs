using System;
namespace MyGame
{
	public class ArrowProjectile : Projectile
	{
		public ArrowProjectile (Direction direct, float x, float y, int damage) 
			: base ("arrowProjectile", "spriteAnimation", direct, x, y, 5, damage)
		{
		}
	}
}
