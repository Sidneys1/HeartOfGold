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
		[NBT.PropertyAttribute("X", typeof(NBT.IntNode))]
		public int X { get; set; }

		[NBT.PropertyAttribute("Y", typeof(NBT.IntNode))]
		public int Y { get; set; }

		///// <summary>
		///// Creates a Location from NBT
		///// </summary>
		///// <param name="LocationNode">The NBT ObjectNode to parse</param>
		//public Location(NBT.ObjectNode LocationNode)
		//{
		//	if (LocationNode.Name != "Location")
		//		throw new FormatException("ObjectNode was not of format 'Location'");

		//	#region Load Int Node "X"

		//	NBT.IntNode XNode = (NBT.IntNode)LocationNode.Children.FirstOrDefault(o => o is NBT.IntNode && o.Name == "X");

		//	if (XNode == null)
		//		throw new FormatException("ObjectNode of type 'Location' did not contain expected IntNode 'X'");
		//	else
		//		X = XNode.Value;

		//	#endregion

		//	#region Load Int Node "Y"

		//	NBT.IntNode YNode = (NBT.IntNode)LocationNode.Children.FirstOrDefault(o => o is NBT.IntNode && o.Name == "Y");

		//	if (YNode == null)
		//		throw new FormatException("ObjectNode of type 'Location' did not contain expected IntNode 'Y'");
		//	else
		//		Y = YNode.Value;

		//	#endregion
		//}

		public Location() { }

		public override string ToString()
		{
			return string.Format("[{0}, {1}]", X, Y);
		}
	}
}
