using HeartOfGold.NBT;

namespace HeartOfGold.Engine.Entities
{
	/// <summary>
	/// Describes an entity's base statistics
	/// </summary>
	public class Stats
	{
		#region Properties

		/// <summary>
		/// The amount of health an entity currently has.
		/// </summary>
		[NBTProperty("Health", typeof(IntNode))]
		public int Health { get; set; }

		/// <summary>
		/// The maximum amount of health the entity can possibly have.
		/// </summary>
		[NBTProperty("Max Health", typeof(IntNode))]
		public int MaxHealth { get; set; }

		/// <summary>
		/// The base damage this entity's attack inflicts.
		/// </summary>
		[NBTProperty("Strength", typeof(IntNode))]
		public int Strength { get; set; } 

		#endregion
	}
}
