using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace HeartOfGold.NBT
{
	/// <summary>
	/// Describes a branch node.
	/// </summary>
	[Serializable()]
	public abstract class ContainerNode : Node
	{
		/// <summary>
		/// A list containing child nodes.
		/// </summary>
		[XmlElement(Type = typeof(NBT.ByteNode)),
		XmlElement(Type = typeof(NBT.ListNode)),
		XmlElement(Type = typeof(NBT.StringNode)),
		XmlElement(Type = typeof(NBT.IntNode)),
		XmlElement(Type = typeof(NBT.FloatNode)),
		XmlElement(Type = typeof(NBT.DoubleNode)),
		XmlElement(Type = typeof(NBT.LongNode)),
		XmlElement(Type = typeof(NBT.ObjectNode))]
		public ObservableCollection<Node> Children { get; protected set; }

		public override bool CanHaveChildren
		{
			get
			{
				return true;
			}
		}

		public override bool HasValue
		{
			get
			{
				return false;
			}
		}
	}

	/// <summary>
	/// Describes a branch node with arbitrary in-order children
	/// </summary>
	[Serializable()]
	public class ListNode : ContainerNode
	{
		public ListNode()
		{
			Children = new ObservableCollection<Node>();
		}

		/// <summary>
		/// Creates a list node from a saved XML file.
		/// </summary>
		/// <param name="FromFile">The path to the file to load.</param>
		public static ListNode Deserialize(string FromFile)
		{
			XmlSerializer b = new XmlSerializer(typeof(NBT.ListNode), "HeartOfGold.NBT");
			XmlReader r = XmlReader.Create(FromFile);
			ListNode n = (NBT.ListNode)b.Deserialize(r);
			r.Close();
			return n;
		}

		/// <summary>
		/// Access a child node by index.
		/// </summary>
		/// <param name="key">The index of the requested node.</param>
		/// <returns>Node at Index Key</returns>
		public Node this[int key]
		{
			get { return Children[key]; }
			set { Children[key] = value; }
		}
	}

	/// <summary>
	/// Represents a saved memory object or class.
	/// </summary>
	[Serializable()]
	public class ObjectNode : ContainerNode
	{
		public ObjectNode()
		{
			Children = new ObservableCollection<Node>();
		}

		/// <summary>
		/// Creates an object node from a saved XML file.
		/// </summary>
		/// <param name="FromFile">The path to the file to load.</param>
		public static ObjectNode Deserialize(string FromFile)
		{
			XmlSerializer b = new XmlSerializer(typeof(NBT.ObjectNode), "HeartOfGold.NBT");
			XmlReader r = XmlReader.Create(FromFile);
			ObjectNode n = (NBT.ObjectNode)b.Deserialize(r);
			r.Close();
			return n;
		}

		/// <summary>
		/// Access a child node by name.
		/// </summary>
		/// <param name="key">The name of the child node to search for.</param>
		/// <returns>The first result matching the search key, or null if no result was found.</returns>
		public Node this[string key]
		{
			get { return Children.FirstOrDefault(o => o.Name == key); }
			set
			{
				Node n = Children.FirstOrDefault(o => o.Name == key);
				if (n != null)
				{
					int i = Children.IndexOf(n);
					Children.Remove(n);
					Children.Insert(i, value);
				}
			}
		}

		/// <summary>
		/// Search for a Node of generic type by name.
		/// </summary>
		/// <typeparam name="T">The specific node type to search for.</typeparam>
		/// <param name="Name">The name of the node to search for.</param>
		/// <returns>The requested node, or null</returns>
		public T FindChild<T>(string Name) where T : Node
		{
			return (T)Children.FirstOrDefault(o => o is T && o.Name == Name);
		}

		// WARNING:
		// HERE BE DRAGONS
		//
		//			   \                  /
		//	  _________))                ((__________
		//   /.-------./\\    \    /    //\.--------.\
		//  //#######//##\\   ))  ((   //##\\########\\
		// //#######//###((  ((    ))  ))###\\########\\
		//((#######((#####\\  \\  //  //#####))########))
		// \##' `###\######\\  \)(/  //######/####' `##/
		//  )'    ``#)'  `##\`->oo<-'/##'  `(#''     `(
		//		    (       ``\`..'/''       )
		//					   \""(
		//					    `- )
		//					    / /
		//					   ( /\
		//					   /\| \
		//					  (  \
		//						  )
		//					     /
		//					    (
		//				 	     `

		/// <summary>
		/// Create an object from this ObjectNode
		/// </summary>
		/// <typeparam name="T">The object type to return.</typeparam>
		/// <returns>A fully instantiated object of type T</returns>
		public T Instantiate<T>()
		{
			// Retrieve properties from T
			PropertyInfo[] props = typeof(T).GetProperties();

			// Create instance of T to return
			T returnval = Activator.CreateInstance<T>();

			foreach (PropertyInfo info in props)
			{
				// If this property doesn't have an NBTProperty attribute, we don't need to instantiate it.
				if (!Attribute.IsDefined(info, typeof(NBTProperty)))
					continue;

				NBTProperty prop = (NBTProperty)info.GetCustomAttribute(typeof(NBTProperty));

				if (prop.Type == typeof(ListNode))
				{
					// This is going to be more complex than the other types, as we can don't have any knowledge of what types of children to expect within this list.
					#region List Node

					// Find the appropriate ListNode within the current ObjectNode
					ListNode correctNode = (ListNode)this.FindChild<ListNode>(prop.Name);

					if (correctNode == null)
						throw new FormatException(string.Format("{0} named '{1}' did not contain expected {2} '{3}'", typeof(T).Name, this.Name, prop.Type.Name, prop.Name));

					// Now we need to loop through each item in this list and add it to the return value's list.
					foreach (Node n in correctNode.Children)
					{
						// If it's an Object, we need to determine its type and instantiate it.
						if (prop.ChildType == typeof(ObjectNode))
						{
							ObjectNode o = (ObjectNode)n;

							// Create a generic method to find our Category node.
							Type genericClassType = typeof(ObjectNode);
							MethodInfo mInfo = genericClassType.GetMethod("FindChild");
							MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(typeof(StringNode));

							// Assume ChildClassPathTag is one level deep for now...
							// In the future possibly allow hierarchically class path tags, e.g. "Details.Category"
							StringNode path = (StringNode)genericMethodInfo.Invoke(o, new object[] { prop.ChildClassPathTag });

							if (path == null)
								throw new FormatException(string.Format("{0} named '{1}' did not contain expected StringNode '{3}'", typeof(T).Name, this.Name, prop.ChildClassPathTag));

							// Retrieve the type from the qualified type path we get from combining FormatChildClassPath
							// and the value we got from the generic method above.
							Type t = Type.GetType(string.Format(prop.FormatChildClassPath, path.Value), true, true);

							// Create another generic method call to this method of the type we deduced above.
							// This will instantiate the object using the appropriate tag within the list
							mInfo = genericClassType.GetMethod("Instantiate");
							genericMethodInfo = mInfo.MakeGenericMethod(t);
							object item = genericMethodInfo.Invoke(o, null);

							// Get the actual instance of the List<...> from the return value, and invoke the Add method with the instantiated item as a parameter
							object list = info.GetValue(returnval);
							info.PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, list, new object[] { item });
						}
						// Ban nested ListNodes? I'm really just too lazy to add it right now.
						// Make it a recursive function in the future?
						//else if (prop.ChildType == ListNode)
						// { }
						else
						{
							// We have a value type! This is easy, just retreieve the Value property and invoke the Add method of the List<...> object in the return value.
							object value = n.GetType().GetProperty("Value").GetValue(correctNode);
							object list = info.GetValue(returnval);
							info.PropertyType.InvokeMember("Add", BindingFlags.Default, null, list, new object[] { value });
						}
					}

					#endregion
				}
				else if (prop.Type == typeof(ObjectNode))
				{
					#region Object Node

					// Search this ObjectNode for the appropriate child node.
					ObjectNode correctNode = this.FindChild<ObjectNode>(prop.Name);

					if (correctNode == null)
						throw new FormatException(string.Format("{0} named '{1}' did not contain expected {2} '{3}'", typeof(T).Name, this.Name, prop.Type.Name, prop.Name));

					// Create and run a generic method call to Instantiate<> on object.
					MethodInfo mInfo = this.GetType().GetMethod("Instantiate");
					MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(info.PropertyType);
					object value = genericMethodInfo.Invoke(correctNode, null);

					// And finally set the value in the return object.
					info.SetValue(returnval, value);

					#endregion
				}
				else
				{
					#region Value Node

					// Create a generic method call to find the appropriate child tag within this ObjectNode.
					Type genericClassType = typeof(ObjectNode);
					MethodInfo mInfo = genericClassType.GetMethod("FindChild");
					MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(prop.Type);
					Node correctNode = (Node)genericMethodInfo.Invoke(this, new object[] { prop.Name });

					if (correctNode == null)
						throw new FormatException(string.Format("{0} named '{1}' did not contain expected {2} '{3}'", typeof(T).Name, this.Name, prop.Type.Name, prop.Name));

					// Retrieve the value from the child node, and apply it to the return object.
					object value = prop.Type.GetProperty("Value").GetValue(correctNode);
					info.SetValue(returnval, value);

					#endregion
				}
			}
			return returnval;
		}
	}
}
