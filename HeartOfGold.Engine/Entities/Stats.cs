using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Entities
{
	public struct Stats
	{
		public int Health;
		public int MaxHealth;
		public int Strength;

		public Stats(NBT.ObjectNode StatsNode)
		{
			if (StatsNode.Name != "Stats")
				throw new FormatException("ObjectNode was not of format 'Stats'");

			#region Load Int Node "Health"

			NBT.IntNode HealthNode = StatsNode.FindChild<NBT.IntNode>("Health");

			if (HealthNode == null)
				throw new FormatException("ObjectNode of type 'Stats' did not contain expected IntNode 'Health'");
			else
				Health = HealthNode.Value;

			#endregion

			#region Load Int Node "Max Health"

			NBT.IntNode MaxHealthNode = StatsNode.FindChild<NBT.IntNode>("Max Health");

			if (MaxHealthNode == null)
				throw new FormatException("ObjectNode of type 'Stats' did not contain expected IntNode 'MaxHealth'");
			else
				MaxHealth = MaxHealthNode.Value;

			#endregion

			#region Load Int Node "Strength"

			NBT.IntNode StrengthNode = StatsNode.FindChild<NBT.IntNode>("Strength");

			if (StrengthNode == null)
				throw new FormatException("ObjectNode of type 'Stats' did not contain expected IntNode 'Strength'");
			else
				Strength = StrengthNode.Value;

			#endregion
		}
	}
}
