using System;
namespace MyGame
{
	public abstract class Projectile : GameObject
	{
		protected int _damage;

		public Projectile (string bmp, string anim, Direction direct, int x, int y, int speed, int damage) 
			: base (bmp, anim, direct, x, y, speed, SpriteState.Moving)
		{
			_damage = damage;
		}

		/// <summary>
		/// Projectiles will move when they update
		/// </summary>
		public override void Update ()
		{
			Move (_direct);
			base.Update ();
		}

		/// <summary>
		/// Most projectiles will have the same animation
		/// </summary>
		/// <returns>The next animation.</returns>
		protected override string DetermineNextAnimation ()
		{
			return "projectile" + direct;
		}

		/// <summary>
		/// Determines if the projectile can be deleted, usually called when the projectile has collided
		/// </summary>
		/// <returns><c>true</c>, if the projectile can be deleted, <c>false</c> if it cant.</returns>
		public abstract bool CanBeDeleted ();

		public int Damage {
			get {
				return _damage;
			}
		}
	}
}
