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
			Player myPlayer = new Player (400, 400);
			Menu myMenu = new Menu ();

			Sword mySword = new Sword ("sword");
			Potion myPotion = new Potion ("potion");

			myPlayer.Inventory.AddItem (mySword);
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

			if (SwinGame.KeyDown (KeyCode.ZKey)) {
				myPlayer.UseFirstItem ();
			}

			myPlayer.Move (tempDirect);
		}

		/// <summary>
		/// Updates the game.
		/// </summary>
		/// <param name="myPlayer">The player.</param>
		public static void UpdateGame (Player myPlayer)
		{
			myPlayer.Update();
		}

		/// <summary>
		/// Draws the game.
		/// </summary>
		/// <param name="myPlayer">The player.</param>
		public static void DrawGame (Player myPlayer)
		{
			SwinGame.ClearScreen (Color.White);
			SwinGame.DrawFramerate (0, 0);

			myPlayer.Draw ();

			SwinGame.RefreshScreen (60);
		}
    }
}