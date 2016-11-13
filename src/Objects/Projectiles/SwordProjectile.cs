using System;
using SwinGameSDK;
namespace MyGame
{
	public class SwordProjectile : Projectile
	{
		private Timer _timeAlive = new Timer ();
		private bool _canShoot;

		public SwordProjectile (string BitmapName, Direction direct, int x, int y, int damage, bool canShoot) 
			: base (BitmapName, "spriteAnimation", direct, x, y, 5, damage)
		{
			_timeAlive.Start ();
			_canShoot = canShoot;
		}

		/// <summary>
		/// The sword projectile will move only if it can shoot and its been alive for more than 200 milliseconds
		/// </summary>
		/// <param name="direct">The direction.</param>
		/// <param name="speed">The speed.</param>
		public override void Move (Direction direct, int speed)
		{
			if (_timeAlive.Ticks < 200) {
				base.Move (direct, 0);
			} else if (_canShoot) {
				base.Move (direct, speed);
			} else {
				base.Move (direct, 999999);
			}
		}

		/// <summary>
		/// The sword projectile can only be deleted after it has been alive for more than 200 milliseconds
		/// </summary>
		/// <returns><c>true</c>, if the sword projectile has been alive after 200 milliseconds, <c>false</c> otherwise.</returns>
		public override bool CanBeDeleted ()
		{
			return (_timeAlive.Ticks > 200);
		}
	}
}
