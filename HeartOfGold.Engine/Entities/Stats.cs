using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		[NBT.NBTProperty("Health", typeof(NBT.IntNode))]
		public int Health { get; set; }

		/// <summary>
		/// The maximum amount of health the entity can possibly have.
		/// </summary>
		[NBT.NBTProperty("Max Health", typeof(NBT.IntNode))]
		public int MaxHealth { get; set; }

		/// <summary>
		/// The base damage this entity's attack inflicts.
		/// </summary>
		[NBT.NBTProperty("Strength", typeof(NBT.IntNode))]
		public int Strength { get; set; } 

		#endregion

		public Stats() { }
	}
}
