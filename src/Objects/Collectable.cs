using System;
using System.Collections.Generic;
namespace MyGame
{
	public abstract class Collectable : GameObject
	{
		public static int CollectID;
		protected int _value;

		public Collectable (string bmp, string anim, int x, int y, int value)
			: base (bmp, anim, Direction.None, x, y, 0, SpriteState.Stationary) 
		{
			_value = value;
		}

		public Collectable () { }

		public override void Save (MySqlConnector theConnector)
		{
			base.Save (theConnector);

			string command = "insert into Collectable (CollectableID, ObjectID, Value) values (" + CollectID + ", " + ObjectID + ", " +  _value + ");";

			theConnector.NonQuery (command);

			CollectID++;
		}

		public override void Load (MySqlConnector theConnector, int ObjectID)
		{
			base.Load (theConnector, ObjectID);

			string command = "select * from Collectable where ObjectID = " + ObjectID + ";";
			List<List<string>> data = theConnector.Select (command);

			int i;

			int.TryParse (data [2] [0], out i);
			_value = i;
		}

		/// <summary>
		/// Has the player collect the object
		/// </summary>
		/// <param name="p">The player.</param>
		public abstract void Collect (Player p);

		/// <summary>
		/// Most collectables will have the same animation
		/// </summary>
		/// <returns>The next animation.</returns>
		protected override string DetermineNextAnimation ()
		{
			return "collectable";
		} 
	}
}