using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Items
{
	public class ItemBase
	{
		public string Category { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public double Worth { get; set; }
	}
}
