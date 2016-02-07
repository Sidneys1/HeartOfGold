using HeartOfGold.NBT;

namespace HeartOfGold.Engine.Items
{
	/// <summary>
	/// Describes a wieldable weapon.
	/// </summary>
	public class Weapon : ItemBase
	{
		#region Properties

		/// <summary>
		/// The base amount of damage this weapon can inflict.
		/// </summary>
		[NBTProperty("Damage", typeof(IntNode))]
// ReSharper disable once UnusedAutoPropertyAccessor.Local
		public int Damage { get; private set; } 

		#endregion
	}
}
