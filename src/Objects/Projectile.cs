using System;
namespace MyGame
{
	public abstract class Projectile : GameObject
	{
		private int _damage;

		public Projectile (string bmp, string anim, Direction direct, int x, int y, int speed, int damage) 
			: base (bmp, anim, direct, x, y, speed, SpriteState.Moving)
		{
			_damage = damage;
		}

		public override void Update ()
		{
			Move (_direct);
			base.Update ();
		}

		protected override string DetermineNextAnimation ()
		{
			return "projectile" + direct;
		}

		public int Damage {
			get {
				return _damage;
			}
		}

		public int Speed {
			get {
				return _speed;
			}
		}

		public abstract bool CanBeDeleted ();
	}
}
