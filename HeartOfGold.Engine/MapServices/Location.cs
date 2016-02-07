using HeartOfGold.NBT;

namespace HeartOfGold.Engine.MapServices
{
	/// <summary>
	/// Describes a two-dimensional point.
	/// </summary>
	public class Location
	{
		#region Properties
		
		/// <summary>
		/// The X-coordinate
		/// </summary>
		[NBTProperty("X", typeof(IntNode))]
		public int X { get; set; }

		/// <summary>
		/// The Y-coordinate
		/// </summary>
		[NBTProperty("Y", typeof(IntNode))]
		public int Y { get; set; } 

		#endregion

		public override string ToString()
		{
			return string.Format("[{0}, {1}]", X, Y);
		}
	}
}
