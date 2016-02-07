using HeartOfGold.NBT;

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
		[NBTProperty("Category", typeof(StringNode))]
		public string Category { get; set; }

		/// <summary>
		/// A quick description of the item.
		/// </summary>
		[NBTProperty("Description", typeof(StringNode))]
		public string Description { get; set; }

		/// <summary>
		/// The displayed name of this item.
		/// </summary>
		[NBTProperty("Name", typeof(StringNode))]
		public string Name { get; set; }

		/// <summary>
		/// The monetary worth of this item.
		/// </summary>
		[NBTProperty("Worth", typeof(DoubleNode))]
		public double Worth { get; set; } 

		#endregion

		public override string ToString()
		{
			return string.Format("{0}: \"{1}\"", Category, Name);
		}
	}
}
