using System;
using SwinGameSDK;
namespace MyGame
{
	public static class Utilities
	{
		/// <summary>
		/// Reverses the given direction.
		/// </summary>
		/// <returns>The reversed direction.</returns>
		/// <param name="theDirection">The direction to be reversed.</param>
		public static Direction ReverseDirection (Direction theDirection)
		{
			Direction newDirection;

			if (theDirection == Direction.Up) {
				newDirection = Direction.Down;
			} else if (theDirection == Direction.Down) {
				newDirection = Direction.Up;
			} else if (theDirection == Direction.Left) {
				newDirection = Direction.Right;
			} else if (theDirection == Direction.Right) {
				newDirection = Direction.Left;
			} else {
				newDirection = Direction.None;
			}

			return newDirection;
		}

		/// <summary>
		/// Gives a random direction
		/// </summary>
		/// <returns>The random direction.</returns>
		public static Direction RandomDirection ()
		{
			Direction newDirection = (Direction)SwinGame.Rnd (5);
			return newDirection;
		}
	}
}
