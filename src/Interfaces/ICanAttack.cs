using System;
namespace MyGame
{
	public interface ICanAttack
	{
		/// <summary>
		/// Checks if the enemy can attack at that point
		/// </summary>
		/// <returns><c>true</c>, if the enemy can attack right at that point, <c>false</c> otherwise.</returns>
		bool CheckAttack ();

		/// <summary>
		/// This method will make an enemy do an attack. Returns a projectile that the enemy may or may not shoot.
		/// </summary>
		Projectile Attack ();
	}
}
