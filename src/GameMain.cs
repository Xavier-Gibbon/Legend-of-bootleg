using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
		private static List<Screen> myScreens = new List<Screen> ();
		private static Player myPlayer;

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
        public static void Main()
        {
			//Load the resouces
			LoadResources ();
			//Initialize objects

			myScreens.Add(new Screen (0, 0, 0));
			myScreens.Add(new Screen (1, 0, 1));
			myScreens.Add(new Screen (0, 1, 2));
			myScreens.Add(new Screen (1, 1, 3));

			myPlayer = new Player (400, 400, myScreens[0]);

			myPlayer.CurrentScreen = myScreens[0];
			myScreens[0].AddObject (myPlayer, false);

			myScreens[0].AddNewPath (myScreens[1], Direction.Right);
			myScreens[0].AddNewPath (myScreens[2], Direction.Down);

			myScreens[3].AddNewPath (myScreens[1], Direction.Up);
			myScreens[3].AddNewPath (myScreens[2], Direction.Left);

			Menu myMenu = new Menu ();

			myScreens[0].AddObject(new Heart (100, 100), true);
			myScreens[0].AddObject(new Rupee (200, 100, 1), true);
			myScreens[0].AddObject(new Rupee (220, 100, 5), true);
			myScreens[0].AddObject(new Rupee (240, 100, 20), true);

			myScreens[1].AddObject(new Darknut (400, 100), true);

			myPlayer.Inventory.AddItem (new Sword ("sword"));
			myPlayer.Inventory.AddItem (new Bow ());
			myPlayer.Inventory.AddItem (new Potion ("potion"));
            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 800, 600);

            //Run the game loop
            while(!SwinGame.WindowCloseRequested())
            {
				//Fetch the next batch of UI interaction
			
				ProcessEvents (myMenu);
				UpdateGame ();
				DrawGame ();
            }
        }

		/// <summary>
		/// Loads the resources.
		/// </summary>
		public static void LoadResources ()
		{
			SwinGame.LoadResourceBundle ("animatedSprites.txt");
			SwinGame.LoadBitmapNamed("arrow", "arrow.png");
			SwinGame.LoadBitmapNamed ("sword", "sword.png");
			SwinGame.LoadBitmapNamed ("bow", "bow.png");
			SwinGame.LoadBitmapNamed ("potion", "potion.png");
			SwinGame.LoadBitmapNamed ("potionUsed", "potionUsed.png");
			SwinGame.LoadFontNamed ("emulogic", "emulogic.ttf", 16);
		}

		/// <summary>
		/// Processes the players input and performs actions based on that.
		/// </summary>
		/// <param name="myMenu">The menu</param>
		public static void ProcessEvents (Menu myMenu)
		{
			SwinGame.ProcessEvents ();
			Direction tempDirect;

			if (SwinGame.KeyTyped (KeyCode.EscapeKey)) {
				SwinGame.SetCameraX (0);
				SwinGame.SetCameraY (0);
				myMenu.GameMenu (myPlayer);

				SwinGame.SetCameraPos (myPlayer.CurrentScreen.CameraPosition);
			}

			if (SwinGame.KeyDown (KeyCode.LeftKey)) {
				tempDirect = Direction.Left;
			} else if (SwinGame.KeyDown (KeyCode.RightKey)) {
				tempDirect = Direction.Right;
			} else if (SwinGame.KeyDown (KeyCode.UpKey)) {
				tempDirect = Direction.Up;
			} else if (SwinGame.KeyDown (KeyCode.DownKey)) {
				tempDirect = Direction.Down;
			} else {
				tempDirect = Direction.None;
			}

			if (SwinGame.KeyTyped (KeyCode.ZKey)) {
				myPlayer.UseFirstItem ();
			} else if (SwinGame.KeyTyped (KeyCode.XKey)) {
				myPlayer.UseSecondItem ();
			}

			if (SwinGame.KeyTyped (KeyCode.OKey)) {
				myPlayer.CurrentScreen.AddObject (new Octorock (200, 200), true);
			}

			if (SwinGame.KeyTyped (KeyCode.PKey)) {
				myPlayer.CurrentScreen.AddObject (new Darknut (200, 200), true);
			}

			if (SwinGame.KeyTyped (KeyCode.SpaceKey)) {
				myPlayer.CurrentScreen.RemoveObject (myPlayer);
				myPlayer.CurrentScreen.FinishRemoveObjects ();
				myPlayer = new Player (20, 20, myScreens[0]);
				myPlayer.CurrentScreen.AddObject (myPlayer, false);
				myPlayer.CurrentScreen.ThePlayer = myPlayer;
			}

			myPlayer.Move (tempDirect);
		}

		/// <summary>
		/// Updates the game.
		/// </summary>
		public static void UpdateGame ()
		{
			myPlayer.CurrentScreen.UpdateObjects();
		}

		/// <summary>
		/// Draws the game.
		/// </summary>
		public static void DrawGame ()
		{
			SwinGame.ClearScreen (Color.Cornsilk);
			SwinGame.DrawFramerate (SwinGame.CameraX(), SwinGame.CameraY());

			myPlayer.CurrentScreen.DrawObjects ();

			SwinGame.RefreshScreen (60);
		}

		/// <summary>
		/// Saves the game.
		/// </summary>
		public static void SaveGame ()
		{
			Screen.ScreenID = 0;
			GameObject.ObjectID = 0;
			Player.PlayerID = 0;
			Item.ItemID = 0;
			Collectable.CollectID = 0;
			Projectile.ProjectID = 0;
			Enemy.EnemyID = 0;

			MySqlConnector myConnector = new MySqlConnector ();
			if (myConnector.OpenConnection ()) {
				try {
					//The query that begins the saving
					myConnector.NonQuery ("set autocommit = false; delete from PlayerInventory; delete from Item; delete from Player; delete from Projectile; delete from Collectable; delete from Enemy; delete from Object; Set foreign_key_checks = 0; delete from Screen; Set foreign_key_checks = 1;");

					foreach (Screen s in myScreens) {
						s.Save (myConnector);
					}

					foreach (Screen s in myScreens) {
						s.SavePaths (myConnector);
					}

					myConnector.NonQuery ("commit;");
				} catch (MySqlException ex) {
					Console.WriteLine ("There was an error with saving the game. Exception: {0}", ex.Message);
					myConnector.NonQuery ("rollback;");
				} finally {
					myConnector.CloseConnection ();
				}

			}
		}

		/// <summary>
		/// Loads the game from the saved data.
		/// </summary>
		public static void LoadGame ()
		{
			MySqlConnector myConnector = new MySqlConnector ();
			Screen.ScreenID = 0;

			List<Screen> temp = new List<Screen> ();
			Player tempP = null;

			if (myConnector.OpenConnection ()) {
				try {
					string command = "select * from screen;";
					List<List<string>> result = myConnector.Select (command);

					for (int i = 0; i < result[0].Count; i++) {
						int x;
						int y;
						int.TryParse(result [1] [i], out x);
						int.TryParse(result [2] [i], out y);

						temp.Add(new Screen (x, y, i));
					}

					for (int i = 0; i < myScreens.Count; i++) {
						for (int j = 3; j < 7; j++) {
							if (result [j] [i] != "") {
								int k;
								int.TryParse(result [j] [i], out k);
								temp[i].AddNewPath (myScreens [k], (Direction)(j - 3));
							}
						}
					}

					foreach (Screen s in temp) {
						s.LoadObjects (myConnector);
					}

					foreach (Screen s in temp) {
						if (s.ThePlayer != null) {
							tempP = s.ThePlayer;
							tempP.LoadItems (myConnector);
							tempP.CurrentScreen = s;
							break;
						}
					}

					myConnector.NonQuery ("commit;");

					myScreens = temp;
					myPlayer = tempP;

				} catch (MySqlException ex) {
					Console.WriteLine ("There was an eror with loading the game. Exception: {0}", ex.Message);
					myConnector.NonQuery ("rollback;");
				} finally {
					myConnector.CloseConnection ();
				}
			}
		}
    }
}