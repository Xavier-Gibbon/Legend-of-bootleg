using System;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
			//Load the resouces
			LoadResources ();
			//Initialize objects
			Screen myScreen = new Screen (0, 0);
			Screen myScreen2 = new Screen (1, 0);
			Screen myScreen3 = new Screen (0, 1);
			Screen myScreen4 = new Screen (1, 1);

			Player myPlayer = new Player (400, 400, myScreen);

			myPlayer.CurrentScreen = myScreen;
			myScreen.AddObject (myPlayer, false);

			myScreen.AddNewPath (myScreen2, Direction.Right);
			myScreen.AddNewPath (myScreen3, Direction.Down);

			myScreen4.AddNewPath (myScreen2, Direction.Up);
			myScreen4.AddNewPath (myScreen3, Direction.Left);

			Menu myMenu = new Menu ();

			Sword mySword = new Sword ("sword");
			Bow myBow = new Bow ();
			Potion myPotion = new Potion ("potion");

			myScreen.AddObject(new Heart (100, 100), true);
			myScreen.AddObject(new Rupee (200, 100, 1), true);
			myScreen.AddObject(new Rupee (220, 100, 5), true);
			myScreen.AddObject(new Rupee (240, 100, 20), true);

			myScreen2.AddObject(new Darknut (400, 100), true);

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
				DrawGame (myPlayer, myScreen);
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

			if (SwinGame.KeyTyped (KeyCode.SpaceKey)) {
				myPlayer.CurrentScreen.AddObject (new Octorock (200, 200), true);
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
		public static void DrawGame (Player myPlayer, Screen myScreen)
		{
			SwinGame.ClearScreen (Color.Cornsilk);
			SwinGame.DrawFramerate (SwinGame.CameraX(), SwinGame.CameraY());

			myPlayer.CurrentScreen.DrawObjects ();

			SwinGame.RefreshScreen (60);
		}
    }
}