using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartOfGold.Engine.Items
{
	public class Weapon : ItemBase
	{
		[NBT.PropertyAttribute("Damage", typeof(NBT.IntNode))]
		public int Damage { get; private set; }

		//public Weapon(NBT.ObjectNode WeaponNode)
		//{
			
		//}

		public Weapon() { }
	}
}
