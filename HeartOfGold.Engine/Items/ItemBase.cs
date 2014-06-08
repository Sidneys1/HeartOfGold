using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Items
{
	public class ItemBase
	{
		[NBT.PropertyAttribute("Category", typeof(NBT.StringNode))]
		public string Category { get; set; }

		[NBT.PropertyAttribute("Description", typeof(NBT.StringNode))]
		public string Description { get; set; }

		[NBT.PropertyAttribute("Name", typeof(NBT.StringNode))]
		public string Name { get; set; }

		[NBT.PropertyAttribute("Worth", typeof(NBT.DoubleNode))]
		public double Worth { get; set; }

		public override string ToString()
		{
			return string.Format("{0}: \"{1}\"", Category, Name);
		}
	}
}
