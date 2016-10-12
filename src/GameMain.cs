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

            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 800, 600);

            //Run the game loop
            while(!SwinGame.WindowCloseRequested())
            {
				//Fetch the next batch of UI interaction
			
				ProcessEvents (myPlayer);
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
		}

		/// <summary>
		/// Processes the players input and performs actions based on that.
		/// </summary>
		/// <param name="myPlayer">The player.</param>
		public static void ProcessEvents (Player myPlayer)
		{
			SwinGame.ProcessEvents ();
			Direction tempDirect;

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