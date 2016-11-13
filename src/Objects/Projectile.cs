using System;
using System.Collections.Generic;
namespace MyGame
{
	public abstract class Projectile : GameObject
	{
		public static int ProjectID;
		protected int _damage;

		public Projectile (string bmp, string anim, Direction direct, int x, int y, int speed, int damage) 
			: base (bmp, anim, direct, x, y, speed, SpriteState.Moving)
		{
			_damage = damage;
		}

		public Projectile () { }

		public override void Save (MySqlConnector theConnector)
		{
			base.Save (theConnector);

			string command = "insert into Projectile (ProjectileID, ObjectID, Damage) values (" + ProjectID + ", " + ObjectID + ", " + _damage + ");";

			theConnector.NonQuery (command);
			ProjectID++;
		}

		public override void Load (MySqlConnector theConnector, int ObjectID)
		{
			base.Load (theConnector, ObjectID);

			string command = "select * from Projectile where ObjectID = " + ObjectID + ";";
			List<List<string>> data = theConnector.Select (command);

			int i;

			int.TryParse (data [2] [0], out i);
			_damage = i;
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
