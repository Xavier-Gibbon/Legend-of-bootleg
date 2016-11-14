using System;
using System.Collections.Generic;
using SwinGameSDK;
namespace MyGame
{
	public class Screen
	{
		public static int ScreenID;

		private Dictionary<Direction, Screen> _directions = new Dictionary<Direction, Screen>();
		private List<GameObject> _objects = new List<GameObject> ();
		private List<Projectile> _projectiles = new List<Projectile> ();
		private List<Collectable> _collectables = new List<Collectable> ();
		private List<Enemy> _enemies = new List<Enemy> ();
		private Player _thePlayer;
		private List<GameObject> _toDelete = new List<GameObject> ();

		private int _idNumber;

		private Point2D _cameraPosition;

		public Screen (int xPos, int yPos, int idNumber)
		{
			_cameraPosition.X = xPos * 800;
			_cameraPosition.Y = yPos * 600;
			_idNumber = idNumber;
		}
		public Screen (int xPos, int yPos) : this (xPos, yPos, 0) { }

		public void Save (MySqlConnector theConnector)
		{
			string command = "insert into Screen (ScreenID, CameraPositionX, CameraPositionY, ScreenUp, ScreenDown, ScreenLeft, ScreenRight) values (" + ScreenID + ", " + _cameraPosition.X / SwinGame.ScreenWidth() + ", " + _cameraPosition.Y / SwinGame.ScreenHeight() + ", null, null, null, null);";

			theConnector.NonQuery (command);
		
			foreach (GameObject o in _objects) {
				o.Save (theConnector);
				GameObject.ObjectID++;
			}

			ScreenID++;
		}

		public void SavePaths (MySqlConnector theConnector)
		{
			string command = "Update Screen set ";
			string value;

			for (int i = 0; i <= 3; i++) {
				if (_directions.ContainsKey ((Direction)i)) {
					value = _directions[(Direction)i]._idNumber.ToString ();
				} else {
					value = "null";
				}

				command += "Screen" + ((Direction)i) + " = " + value;
				if (i != 3) {
					command += ", ";
				} else {
					command += " ";
				}
			}

			command += "where ScreenID = " + _idNumber + ";";

			theConnector.NonQuery (command);
		}

		public void LoadObjects (MySqlConnector theConnector)
		{
			string command = "select * from object where ScreenID = " + _idNumber + ";";
			List<List<string>> result = theConnector.Select (command);



			for (int i = 0; i < result[0].Count; i++) {
				GameObject o = GameObject.CreateObject (result [9] [i]);

				int j;
				int.TryParse (result [0] [i], out j);

				o.Load (theConnector, j);

				AddObject (o, false);
			}
		}
		/// <summary>
		/// Updates the screens objects.
		/// Some objects will have different checks based on its type
		/// </summary>
		public void UpdateObjects ()
		{
			foreach(GameObject o in _objects) {
				o.Update ();
				CheckObjectIsOnScreen (o);
			}

			if (_thePlayer != null) {
 				foreach (Projectile p in _projectiles) {
					if (_thePlayer.CollidedWith (p)) {
						_thePlayer.DecreaseHealth (p.Damage);
						if (p.CanBeDeleted ()) {
							RemoveObject (p);
						}
					} else {
						foreach (Enemy e in _enemies) {
							if (e.CollidedWith (p)) {
								e.DecreaseHealth (p.Damage, p.direct);
								if (p.CanBeDeleted ()) {
									RemoveObject (p);
								}
							}
						}
					}
				}

				foreach (Collectable c in _collectables) {
					if (_thePlayer.CollidedWith (c)) {
						c.Collect (_thePlayer);
						RemoveObject (c);
					}
				}

				foreach (Enemy e in _enemies) {
					if (e.State == SpriteState.Dead) {
						RemoveObject (e);

						Collectable theDrop = e.Drop ();
						if (theDrop != null)
							AddObject (e.Drop (), false);

					} else {

						if (_thePlayer.CollidedWith (e) && e.State != SpriteState.Hit) {
							_thePlayer.DecreaseHealth (e.Damage);
						}


						if (e as ICanAttack != null) {
							ICanAttack a = e as ICanAttack;

							if (a.CheckAttack ()) {
								Projectile newP = a.Attack ();

								if (newP != null)
									AddObject (newP, false);

							}
						}
					}
				}
			} else {
				_objects [0].Move (Direction.None);
			}


			FinishRemoveObjects ();
		}

		/// <summary>
		/// Checks the object is on screen and acts depending on the object's type
		/// </summary>
		/// <param name="o">The object.</param>
		private void CheckObjectIsOnScreen (GameObject o)
		{
			if (o as Projectile != null) {
				if (o.theSprite.X > _cameraPosition.X + SwinGame.ScreenWidth () || o.theSprite.Y > _cameraPosition.Y + SwinGame.ScreenHeight () || o.theSprite.X + o.theSprite.Width < _cameraPosition.X || o.theSprite.Y + o.theSprite.Height < _cameraPosition.Y) {
					RemoveObject (o);
				}
			} else if (o as Enemy != null) {
				KeepObjectOnScreen (o);
			} else if (o as Player != null) {
				if (KeepObjectOnScreen (o) && _directions.ContainsKey(o.direct) ){
					MoveScreen (o.direct);
				}
			}
		}

		/// <summary>
		/// Keeps the object on screen.
		/// </summary>
		/// <returns><c>true</c>, if the object was made to stay on screen, <c>false</c> if the object was already on screen.</returns>
		/// <param name="o">The object.</param>
		private bool KeepObjectOnScreen (GameObject o)
		{
			bool result = false;

			if (o.theSprite.X + o.theSprite.Width > _cameraPosition.X + SwinGame.ScreenWidth ()) {
				o.theSprite.X = _cameraPosition.X + SwinGame.ScreenWidth () - o.theSprite.Width;
				result = true;
			} else if (o.theSprite.Y + o.theSprite.Height > _cameraPosition.Y + SwinGame.ScreenHeight ()) {
				o.theSprite.Y = _cameraPosition.Y + SwinGame.ScreenHeight () - o.theSprite.Height;
				result = true;
			} else if (o.theSprite.X < _cameraPosition.X) {
				o.theSprite.X = _cameraPosition.X;
				result = true;
			} else if (o.theSprite.Y < _cameraPosition.Y) {
				o.theSprite.Y = _cameraPosition.Y;
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Moves the camera from one screen to the next. 
		/// This happens when the player moves off screen and there is a screen that the player can move to.
		/// </summary>
		/// <param name="theDirection">The direction that the player is moving in.</param>
		private void MoveScreen (Direction theDirection)
		{
			Screen theScreen = _directions [theDirection];
			int x = 0;
			int y = 0;

			theScreen.AddObject(_thePlayer, false);
			_thePlayer.CurrentScreen = theScreen;

			switch (theDirection) {
			case Direction.Up:
				y = -30;
				break;
			case Direction.Down:
				y = 30;
				break;
			case Direction.Left:
				x = -40;
				break;
			case Direction.Right:
				x = 40;
				break;
			}

			for (int i = 0; i < 20; i++) {
				SwinGame.MoveCameraBy (x, y);
				SwinGame.ClearScreen (Color.Cornsilk);

				_thePlayer.Move (theDirection, 2);
				_thePlayer.Update ();

				DrawObjects ();
				theScreen.DrawObjects ();
				SwinGame.RefreshScreen (60);
			}

			//SwinGame.SetCameraX (theScreen.CameraPosition.X);
			//SwinGame.SetCameraY (theScreen.CameraPosition.Y);
			RemoveObject(_thePlayer);
		}

		/// <summary>
		/// Draws the screen objects.
		/// </summary>
		public void DrawObjects ()
		{
			foreach (GameObject o in _objects) {
				o.Draw ();
			}

			if (_thePlayer != null) {
				SwinGame.DrawText (_thePlayer.CurrentHealth.ToString (), Color.Red, _cameraPosition.X, _cameraPosition.Y + 20);
				SwinGame.DrawText (_thePlayer.RupeeCount.ToString (), Color.Blue, _cameraPosition.X, _cameraPosition.Y + 40);
				SwinGame.DrawText (_thePlayer.direct.ToString (), Color.Green, _cameraPosition.X, _cameraPosition.Y + 80);
			} else {
				SwinGame.DrawText (_objects.Count.ToString (), Color.Black, 0, 20);
				for (int i = 0; i < _objects.Count; i++) {
					SwinGame.DrawText (_objects [i].GetType ().ToString (), Color.Black, 0, 30 + 10 * i);
				}
			}
		}

		/// <summary>
		/// Adds the new path.
		/// </summary>
		/// <param name="theScreen">The screen to be added.</param>
		/// <param name="direct">The direction to get to the next screen.</param>
		public void AddNewPath (Screen theScreen, Direction direct)
		{
			if (!_directions.ContainsValue(theScreen)) {
				_directions [direct] = theScreen;
				theScreen.AddNewPath (this, Utilities.ReverseDirection (direct));
			}
		}

		/// <summary>
		/// Adds an object.
		/// </summary>
		/// <param name="theObject">The object to be added.</param>
		/// <param name="AddOffset">Whether or not the object's position should be affected by the screen's camera position</param>
		public void AddObject (GameObject theObject, bool AddOffset)
		{
			if (AddOffset) {
				theObject.theSprite.X += _cameraPosition.X;
				theObject.theSprite.Y += _cameraPosition.Y;
			}

			_objects.Add (theObject);

			if (theObject as Projectile != null) {
				_projectiles.Add (theObject as Projectile);
			} else if (theObject as Collectable != null) {
				_collectables.Add (theObject as Collectable);
			} else if (theObject as Enemy != null) {
				_enemies.Add (theObject as Enemy);
			} else if (theObject as Player != null) {
				_thePlayer = theObject as Player;
			}
		}

		/// <summary>
		/// Adds an object to be removed later.
		/// </summary>
		/// <param name="theObject">The object to be removed.</param>
		public void RemoveObject (GameObject theObject)
		{
			if (_objects.Contains (theObject)) {
				_toDelete.Add (theObject);
			}
		}

		/// <summary>
		/// Finishs removing objects from their respecive lists.
		/// </summary>
		public void FinishRemoveObjects ()
		{
			foreach (GameObject o in _toDelete) {
				_objects.Remove (o);

				if (o.GetType () == typeof (Player)) {
					_thePlayer = null;
				} else if (o as Projectile != null) {
					_projectiles.Remove (o as Projectile);
				} else if (o as Collectable != null) {
					_collectables.Remove (o as Collectable);
				} else if (o as Enemy != null) {
					_enemies.Remove (o as Enemy);
				}
			}

			_toDelete = new List<GameObject> ();
		}

		/// <summary>
		/// Gets or sets the player.
		/// </summary>
		/// <value>The player.</value>
		public Player ThePlayer {
			get {
				return _thePlayer;
			}

			set {
				_thePlayer = value;
			}
		}

		/// <summary>
		/// Gets the camera position.
		/// </summary>
		/// <value>The camera position.</value>
		public Point2D CameraPosition {
			get {
				return _cameraPosition;
			}
		}

		/// <summary>
		/// Gets the directions of the next screens.
		/// </summary>
		/// <value>The directions.</value>
		public Dictionary<Direction, Screen> Directions {
			get {
				return _directions;
			}
		}
	}
}
