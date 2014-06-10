using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		[NBT.NBTProperty("Damage", typeof(NBT.IntNode))]
		public int Damage { get; private set; } 

		#endregion

		public Weapon() { }
	}
}
