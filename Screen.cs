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
		private Player _thePlayer;
		private List<GameObject> _toDelete = new List<GameObject> ();

		private Point2D _cameraPosition;

		public Screen (int xPos, int yPos)
		{
			_cameraPosition.X = xPos * SwinGame.ScreenWidth ();
			_cameraPosition.Y = yPos * SwinGame.ScreenHeight ();
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

			foreach (Projectile p in _projectiles) {
				if (_thePlayer.CollidedWith (p)) {
					_thePlayer.DecreaseHealth (p.Damage);
					RemoveObject (p);
				}
			}

			FinishRemoveObjects ();
		}

		private void CheckObjectIsOnScreen (GameObject o)
		{
			if (o as Projectile != null) {
				if (o.theSprite.X > _cameraPosition.X + SwinGame.ScreenWidth () || o.theSprite.Y > _cameraPosition.Y + SwinGame.ScreenHeight () || o.theSprite.X + o.theSprite.Width < _cameraPosition.X || o.theSprite.Y + o.theSprite.Height < _cameraPosition.Y) {
					RemoveObject (o);
				}
			} else if(o.GetType() == typeof (Player)) {
				if (o.theSprite.X + o.theSprite.Width > _cameraPosition.X + SwinGame.ScreenWidth ()) {
					o.theSprite.X = _cameraPosition.X + SwinGame.ScreenWidth () - o.theSprite.Width;
				} else if (o.theSprite.Y + o.theSprite.Height > _cameraPosition.Y + SwinGame.ScreenHeight ()) {
					o.theSprite.Y = _cameraPosition.Y + SwinGame.ScreenHeight () - o.theSprite.Height;
				} else if (o.theSprite.X < _cameraPosition.X) {
					o.theSprite.X = _cameraPosition.X;
				} else if (o.theSprite.Y < _cameraPosition.Y) {
					o.theSprite.Y = _cameraPosition.Y;
				}
			} 
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
		}

		/// <summary>
		/// Gets the next screen according to the direction given.
		/// </summary>
		/// <returns>The next screen.</returns>
		/// <param name="direct">The direction to find the next screen.</param>
		public Screen GetNextScreen (Direction direct)
		{
			return _directions [direct];
		}

		/// <summary>
		/// Adds the new path.
		/// </summary>
		/// <param name="theScreen">The screen to be added.</param>
		/// <param name="direct">The direction to get to the next screen.</param>
		public void AddNewPath (Screen theScreen, Direction direct)
		{
			if (!_directions.ContainsValue (theScreen)) {
				_directions.Add (direct, theScreen);
				theScreen.AddNewPath (this, Utilities.ReverseDirection (direct));
			}
		}

		/// <summary>
		/// Adds an object.
		/// </summary>
		/// <param name="theObject">The object to be added.</param>
		public void AddObject (GameObject theObject)
		{
			_objects.Add (theObject);

			if (theObject as Projectile != null) {
				_projectiles.Add (theObject as Projectile);
			} else if (theObject.GetType () == typeof (Player)) {
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
				}
			}

			_toDelete = new List<GameObject> ();
		}
	}
}
