using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Items
{
	/// <summary>
	/// Provides a base class for all inventory items.
	/// </summary>
	public class ItemBase
	{
		#region Properties

		/// <summary>
		/// The class-name of this item.
		/// </summary>
		[NBT.NBTProperty("Category", typeof(NBT.StringNode))]
		public string Category { get; set; }

		/// <summary>
		/// A quick description of the item.
		/// </summary>
		[NBT.NBTProperty("Description", typeof(NBT.StringNode))]
		public string Description { get; set; }

		/// <summary>
		/// The displayed name of this item.
		/// </summary>
		[NBT.NBTProperty("Name", typeof(NBT.StringNode))]
		public string Name { get; set; }

		/// <summary>
		/// The monetary worth of this item.
		/// </summary>
		[NBT.NBTProperty("Worth", typeof(NBT.DoubleNode))]
		public double Worth { get; set; } 

		#endregion

		public override string ToString()
		{
			return string.Format("{0}: \"{1}\"", Category, Name);
		}
	}
}
