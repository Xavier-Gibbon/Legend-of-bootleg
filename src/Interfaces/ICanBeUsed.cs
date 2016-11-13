using System;
namespace MyGame
{
	public interface ICanBeUsed
	{
		/// <summary>
		/// This will use the object, using the player as a reference to apply the effects
		/// </summary>
		/// <param name="p">The player.</param>
		void Use (Player p);
	}
}
