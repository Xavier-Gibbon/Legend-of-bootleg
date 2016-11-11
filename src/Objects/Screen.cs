using System;
using System.Collections.Generic;
using SwinGameSDK;
namespace MyGame
{
	public class Screen
	{
		private Dictionary<Direction, Screen> _directions = new Dictionary<Direction, Screen>();
		private List<GameObject> _objects = new List<GameObject> ();
		private List<Projectile> _projectiles = new List<Projectile> ();
		private List<Collectable> _collectables = new List<Collectable> ();
		private List<Enemy> _enemies = new List<Enemy> ();
		private Player _thePlayer;
		private List<GameObject> _toDelete = new List<GameObject> ();

		private Point2D _cameraPosition;

		public Screen (int xPos, int yPos)
		{
			_cameraPosition.X = xPos * 800;
			_cameraPosition.Y = yPos * 600;
		}

		/// <summary>
		/// Updates the screens objects.
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


				FinishRemoveObjects ();
			}
		}

		/// <summary>
		/// Checks the object is on screen and acts depending on the object's type
		/// </summary>
		/// <param name="o">O.</param>
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

			SwinGame.DrawText (_thePlayer.CurrentHealth.ToString(), Color.Red, _cameraPosition.X, _cameraPosition.Y + 20);
			SwinGame.DrawText (_thePlayer.RupeeCount.ToString (), Color.Blue, _cameraPosition.X, _cameraPosition.Y + 40);
			SwinGame.DrawText (_thePlayer.direct.ToString (), Color.Green, _cameraPosition.X, _cameraPosition.Y + 80);
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

		public Player ThePlayer {
			get {
				return _thePlayer;
			}

			set {
				_thePlayer = value;
			}
		}

		public Point2D CameraPosition {
			get {
				return _cameraPosition;
			}
		}

		public Dictionary<Direction, Screen> Directions {
			get {
				return _directions;
			}

			set {
				_directions = value;
			}
		}
	}
}
