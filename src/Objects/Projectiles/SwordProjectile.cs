﻿using System;
using SwinGameSDK;
namespace MyGame
{
	public class SwordProjectile : Projectile
	{
		private Timer _timeAlive = new Timer ();
		private bool _canShoot;

		public SwordProjectile (string BitmapName, Direction direct, float x, float y, int damage, bool canShoot) 
			: base (BitmapName, "spriteAnimation", direct, x, y, 5, damage)
		{
			_timeAlive.Start ();
			_canShoot = canShoot;
		}

		public override void Move (Direction direct, int speed)
		{
			if (_timeAlive.Ticks < 375) {
				base.Move (direct, 0);
			} else if (_canShoot) {
				base.Move (direct, speed);
			} else {
				base.Move (direct, 999999);
			}
		}
	}
}