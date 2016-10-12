using System;
using SwinGameSDK;
namespace MyGame
{
	public static class Utilities
	{
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
	}
}
