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

				//Clear the screen and draw the framerate

            }
        }

		public static void LoadResources ()
		{
			SwinGame.LoadResourceBundle ("animatedSprites.txt");
		}

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

		public static void UpdateGame (Player myPlayer)
		{
			myPlayer.Update();
		}

		public static void DrawGame (Player myPlayer)
		{
			SwinGame.ClearScreen (Color.White);
			SwinGame.DrawFramerate (0, 0);

			myPlayer.Draw ();

			SwinGame.RefreshScreen (60);
		}
    }
}