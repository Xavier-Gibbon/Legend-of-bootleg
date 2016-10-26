using System;
using System.Collections.Generic;
using SwinGameSDK;
namespace MyGame
{
	public class Menu
	{
		private int _option;
		private bool _exit;

		/// <summary>
		/// The loop to process and draw the game menu
		/// </summary>
		/// <param name="p">The player</param>
		public void GameMenu (Player p)
		{
			_option = 0;
			_exit = false;
			do {
				ProcessMainMenuEvents (p);
				DrawMainMenu ();
			} while (!(_exit || SwinGame.WindowCloseRequested ()));
		}

		/// <summary>
		/// Processes the main menu events.
		/// </summary>
		/// <param name="p">The player</param>
		private void ProcessMainMenuEvents (Player p)
		{
			SwinGame.ProcessEvents ();
			_exit = SwinGame.KeyTyped (KeyCode.EscapeKey);

			if (SwinGame.KeyTyped (KeyCode.UpKey)) {
				_option--;
				if (_option == -1) {
					_option = 3;
				}
			} else if (SwinGame.KeyTyped (KeyCode.DownKey)) {
				_option++;
				if (_option == 4) {
					_option = 0;
				}
			}

			if (SwinGame.KeyTyped (KeyCode.ReturnKey)) {
				HandleGameMenuOption (p);
			}
		}

		/// <summary>
		/// Handles the option for the game menu.
		/// </summary>
		/// <param name="p">The player</param>
		private void HandleGameMenuOption (Player p)
		{
			switch (_option) {
			case 0:
				_exit = true;
				break;
			case 1:
				InventoryMenu (p);
				break;
			case 2:
			case 3:
			default:
				break;
			}
		}

		/// <summary>
		/// The loop to process and draw the inventory menu
		/// </summary>
		/// <param name="p">The Player</param>
		private void InventoryMenu (Player p)
		{
			_option = 0;
			do {
				ProcessInventoryMenuEvents (p);
				DrawInventoryMenu (p);
			} while (!(_exit || SwinGame.WindowCloseRequested()));

			_exit = false;
			_option = 1;
		}

		/// <summary>
		/// Processes the inventory menu events.
		/// </summary>
		/// <param name="p">The player</param>
		private void ProcessInventoryMenuEvents (Player p)
		{
			SwinGame.ProcessEvents ();

			_exit = SwinGame.KeyTyped (KeyCode.EscapeKey);

			if (SwinGame.KeyTyped (KeyCode.ZKey) || SwinGame.KeyTyped (KeyCode.XKey)) {
				ICanBeUsed theItem = (ICanBeUsed)p.Inventory.FetchItem (_option);

				if (theItem != null) {
					if (SwinGame.KeyTyped (KeyCode.ZKey)) {
						p.EquipFirstItem (theItem);
					} else {
						p.EquipSecondItem (theItem);
					}
				}
			} else if (SwinGame.KeyTyped (KeyCode.LeftKey)) {
				_option--;
				if (_option == -1) {
					_option = 11;
				}
			} else if (SwinGame.KeyTyped (KeyCode.RightKey)) {
				_option++;
				if (_option == 12) {
					_option = 0;
				}
			} else if (SwinGame.KeyTyped (KeyCode.UpKey) || SwinGame.KeyTyped (KeyCode.DownKey)) {
				_option += 6;
				if (_option > 11) {
					_option -= 12;
				}
			}
		}

		/// <summary>
		/// Draws the inventory menu.
		/// </summary>
		/// <param name="p">The player</param>
		private void DrawInventoryMenu (Player p)
		{
			SwinGame.ClearScreen (Color.Black);
			SwinGame.DrawFramerate (0, 0);

			SwinGame.DrawText ("INVENTORY", Color.White, "emulogic", 30, 350 - (SwinGame.TextWidth (SwinGame.FontNamed ("emulogic"), "INVENTORY") / 2), 100);

			const int XPOS = 125;
			const int XMODIFIER = 75;

			const int YPOS = 275;
			const int YMODIFIER = 100;

			int xMod = 0;
			int yMod = 0;

			for (int i = 0; i != 12; i++) {
				xMod++;
				if (xMod > 6) {
					xMod -= 6;
					yMod++;
				}

				Color boxColor;

				if (i == _option) boxColor = Color.White;
				else boxColor = Color.Blue;

				SwinGame.DrawRectangle (boxColor, XPOS + XMODIFIER * xMod - 2, YPOS + YMODIFIER * yMod - 2, 36, 36);
			}

			xMod = 0;
			yMod = 0;

			for (int i = 0; i < p.Inventory.Count(); i++) {
				xMod++;
				if (xMod > 5) {
					xMod -= 6;
					yMod++;
				}
				p.Inventory.FetchItem(i).ItemGraphic.Draw (XPOS + XMODIFIER * xMod, YPOS + YMODIFIER * yMod);
			}

			SwinGame.RefreshScreen (60);
		}

		/// <summary>
		/// Draws the main menu.
		/// </summary>
		/// <param name="p">The player</param>
		private void DrawMainMenu ()
		{
			SwinGame.ClearScreen (Color.Black);
			SwinGame.DrawFramerate (0, 0);
			string [] list = new string [] { "RESUME", "INVENTORY", "LOAD", "SAVE" };

			//Draws the text on the screen
			for (int i = 0; i < list.Length; i++) {
				SwinGame.DrawText (list [i], Color.White, SwinGame.FontNamed ("emulogic"), 400 - SwinGame.TextWidth (SwinGame.FontNamed ("emulogic"), list [i]) / 2, 200 + i * 50);
			}

			//Draws the arrow at the right position
			SwinGame.DrawBitmap (SwinGame.BitmapNamed ("arrow"), 375 - SwinGame.TextWidth(SwinGame.FontNamed("emulogic"), list[_option]) / 2, 200 + 50 * _option);

			SwinGame.RefreshScreen (60);
		}
	}
}
