using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
		private static List<Screen> myScreens = new List<Screen> ();
		private static Player myPlayer;

        public static void Main()
        {
			//Load the resouces
			LoadResources ();
			//Initialize objects

			MySqlConnector myMySqlConnector = new MySqlConnector ();
			myScreens.Add(new Screen (0, 0));
			myScreens.Add(new Screen (1, 0));
			myScreens.Add(new Screen (0, 1));
			myScreens.Add(new Screen (1, 1));

			myPlayer = new Player (400, 400, myScreens[1]);

			myPlayer.CurrentScreen = myScreens[0];
			myScreens[0].AddObject (myPlayer, false);

			myScreens[0].AddNewPath (myScreens[1], Direction.Right);
			myScreens[0].AddNewPath (myScreens[2], Direction.Down);

			myScreens[3].AddNewPath (myScreens[1], Direction.Up);
			myScreens[3].AddNewPath (myScreens[2], Direction.Left);

			Menu myMenu = new Menu ();

			Sword mySword = new Sword ("sword");
			Bow myBow = new Bow ();
			Potion myPotion = new Potion ("potion");

			myScreens[0].AddObject(new Heart (100, 100), true);
			myScreens[0].AddObject(new Rupee (200, 100, 1), true);
			myScreens[0].AddObject(new Rupee (220, 100, 5), true);
			myScreens[0].AddObject(new Rupee (240, 100, 20), true);

			myScreens[1].AddObject(new Darknut (400, 100), true);

			myPlayer.Inventory.AddItem (mySword);
			myPlayer.Inventory.AddItem (myBow);
			myPlayer.Inventory.AddItem (myPotion);
            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 800, 600);

            //Run the game loop
            while(!SwinGame.WindowCloseRequested())
            {
				//Fetch the next batch of UI interaction
			
				ProcessEvents (myPlayer, myMenu);
				UpdateGame (myPlayer);
				DrawGame (myPlayer);
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
		/// <param name="myPlayer">The player.</param>
		public static void ProcessEvents (Player myPlayer, Menu myMenu)
		{
			SwinGame.ProcessEvents ();
			Direction tempDirect;

			if (SwinGame.KeyTyped (KeyCode.EscapeKey)) {
				myMenu.GameMenu (myPlayer);
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
			myPlayer.Move (tempDirect);
		}

		/// <summary>
		/// Updates the game.
		/// </summary>
		/// <param name="myPlayer">The player.</param>
		public static void UpdateGame (Player myPlayer)
		{
			myPlayer.CurrentScreen.UpdateObjects();
		}

		/// <summary>
		/// Draws the game.
		/// </summary>
		/// <param name="myPlayer">The player.</param>
		public static void DrawGame (Player myPlayer)
		{
			SwinGame.ClearScreen (Color.Cornsilk);
			SwinGame.DrawFramerate (SwinGame.CameraX(), SwinGame.CameraY());

			myPlayer.CurrentScreen.DrawObjects ();

			SwinGame.RefreshScreen (60);
		}

		public static void SaveGame ()
		{
		}

		public static void LoadGame ()
		{
		}
    }
}