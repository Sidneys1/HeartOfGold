using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HeartOfGold.NBT
{
	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyAttribute : System.Attribute
	{
		public readonly string Name;
		public readonly Type Type;
		public readonly Type ChildType;
		public readonly string ChildLookup;

		public PropertyAttribute(string name, Type type, Type childType = null, string childLookup = null)
		{
			this.Name = name;
			this.Type = type;
			this.ChildType = childType;
			this.ChildLookup = childLookup;
		}
	}
}
