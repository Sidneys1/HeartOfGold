using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		[NBT.NBTProperty("X", typeof(NBT.IntNode))]
		public int X { get; set; }

		/// <summary>
		/// The Y-coordinate
		/// </summary>
		[NBT.NBTProperty("Y", typeof(NBT.IntNode))]
		public int Y { get; set; } 

		#endregion

		public Location() { }

		public override string ToString()
		{
			return string.Format("[{0}, {1}]", X, Y);
		}
	}
}
