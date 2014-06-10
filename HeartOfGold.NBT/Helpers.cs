using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HeartOfGold.NBT
{
	/// <summary>
	/// An attribute used to identify NBT-backed properties.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class NBTProperty : System.Attribute
	{
		/// <summary>
		/// The name of this property in Tag form.
		/// (E.g., property named "FirstName" may have its tag named "First Name"
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// The type of the backing NBT Node.
		/// (E.g., NBT.IntNode)
		/// </summary>
		public readonly Type Type;

		/// <summary>
		/// The expected Node type of child nodes.
		/// For ListNodes only.
		/// (E.g., NBT.ObjectNode)
		/// </summary>
		public readonly Type ChildType;

		/// <summary>
		/// The format string for the child's type's class path (qualified, with library reference).
		/// For ListNodes with ObjectNode children only.
		/// (E.g., "HeartOfGold.Engine.Items.{0},HeartOfGold")
		/// </summary>
		public readonly string FormatChildClassPath;

		/// <summary>
		/// The expected tag-path of the exact type within the child ObjectNode.
		/// For ListNodes with ObjectNode children only.
		/// (E.g., "Category")
		/// </summary>
		public readonly string ChildClassPathTag;

		/// <summary>
		/// Create a NBTProperty
		/// </summary>
		/// <param name="name">The name of this property.</param>
		/// <param name="type">The type of backing NBT Node.</param>
		/// <param name="childType">The expected Node type of child nodes. (For ListNodes only.)</param>
		/// <param name="formatChildClassPath">The format string for the child's type's class path. (For ListNodes with ObjectNode children only.)</param>
		/// <param name="childClassPathTag">The expected tag-path of the exact type within the child ObjectNode. (For ListNodes with ObjectNode children only. e.g. "Category")</param>
		public NBTProperty(string name, Type type, Type childType = null, string formatChildClassPath = null, string childClassPathTag = null)
		{
			this.Name = name;
			this.Type = type;
			this.ChildType = childType;
			this.FormatChildClassPath = formatChildClassPath;
			this.ChildClassPathTag = childClassPathTag;
		}
	}
}
