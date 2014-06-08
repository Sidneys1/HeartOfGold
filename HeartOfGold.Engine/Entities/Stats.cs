using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Entities
{
	public class Stats
	{
		[NBT.PropertyAttribute("Health", typeof(NBT.IntNode))]
		public int Health { get; set; }

		[NBT.PropertyAttribute("Max Health", typeof(NBT.IntNode))]
		public int MaxHealth { get; set; }


		[NBT.PropertyAttribute("Strength", typeof(NBT.IntNode))]
		public int Strength { get; set; }

		//public Stats(NBT.ObjectNode StatsNode)
		//{
		//	if (StatsNode.Name != "Stats")
		//		throw new FormatException("ObjectNode was not of format 'Stats'");

		//	#region Load Int Node "Health"

		//	NBT.IntNode HealthNode = StatsNode.FindChild<NBT.IntNode>("Health");

		//	if (HealthNode == null)
		//		throw new FormatException("ObjectNode of type 'Stats' did not contain expected IntNode 'Health'");
		//	else
		//		Health = HealthNode.Value;

		//	#endregion

		//	#region Load Int Node "Max Health"

		//	NBT.IntNode MaxHealthNode = StatsNode.FindChild<NBT.IntNode>("Max Health");

		//	if (MaxHealthNode == null)
		//		throw new FormatException("ObjectNode of type 'Stats' did not contain expected IntNode 'MaxHealth'");
		//	else
		//		MaxHealth = MaxHealthNode.Value;

		//	#endregion

		//	#region Load Int Node "Strength"

		//	NBT.IntNode StrengthNode = StatsNode.FindChild<NBT.IntNode>("Strength");

		//	if (StrengthNode == null)
		//		throw new FormatException("ObjectNode of type 'Stats' did not contain expected IntNode 'Strength'");
		//	else
		//		Strength = StrengthNode.Value;

		//	#endregion
		//}

		public Stats() { }
	}
}
