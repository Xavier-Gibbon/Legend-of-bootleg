using System;
namespace MyGame
{
	/// <summary>
	/// Defines the state of the sprite for both players and enemies
	/// </summary>
	public enum SpriteState
	{
		Stationary,
		Moving,
		Attacking,
		Hit,
		Dead
	}
}
