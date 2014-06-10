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
		public ListNode(string FromFile)
		{
			XmlSerializer b = new XmlSerializer(typeof(NBT.ListNode), "HeartOfGold.NBT");
			XmlReader r = XmlReader.Create(FromFile);
			ListNode n = (NBT.ListNode)b.Deserialize(r);
			r.Close();
			//r.Dispose();
			this.Children = n.Children;
			this.Name = n.Name;
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
			PropertyInfo[] props = typeof(T).GetProperties();
			T returnval = Activator.CreateInstance<T>();
			foreach (PropertyInfo info in props)
			{
				if (Attribute.IsDefined(info, typeof(NBTProperty)))
				{
					NBTProperty prop = (NBTProperty)info.GetCustomAttribute(typeof(NBTProperty));

					if (prop.Type == typeof(ListNode))
					{
						// When a node is a parent-type list node.
						// This might be a bit harder, as we can only know the superclass to expect, not the subclasses.
						// (An inventory contains items, but they might be weapons or armor, we don't know!)
						#region List Node

						ListNode correctNode = (ListNode)this.FindChild<ListNode>(prop.Name);

						foreach (Node n in correctNode.Children)
						{
							if (prop.ChildType == typeof(ObjectNode))
							{
								ObjectNode o = (ObjectNode)n;

								Type genericClassType = typeof(ObjectNode);
								MethodInfo mInfo = genericClassType.GetMethod("FindChild");
								MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(typeof(StringNode));

								// Assume ChildClassPathTag is one level deep for now...
								StringNode path = (StringNode)genericMethodInfo.Invoke(o, new object[] { prop.ChildClassPathTag });

								Type t = Type.GetType(string.Format(prop.FormatChildClassPath, path.Value), true, true);
								//object item = Activator.CreateInstance(t);

								mInfo = genericClassType.GetMethod("Instantiate");
								genericMethodInfo = mInfo.MakeGenericMethod(t);

								object item = genericMethodInfo.Invoke(o, null);

								object list = info.GetValue(returnval);

								info.PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, list, new object[] { item });
							}
							// Ban nested ListNodes?
							//else if (prop.ChildType == ListNode)
							// { }
							else
							{
								// We have a value type!
								object value = n.GetType().GetProperty("Value").GetValue(correctNode);
								info.PropertyType.InvokeMember("Add", BindingFlags.Default, null, returnval, new object[] { value });
							}
						}

						//Type t = Type.GetType(string.Format(prop.ChildLookup, categoryName), true, true);
						//Activator.CreateInstance(t); 

						#endregion
					}
					else if (prop.Type == typeof(ObjectNode))
					{
						// When a node is a parent-type object node. Fairly easy as we know the exact subclass to expect.
						#region ObjectNode

						//Type genericClassType = typeof(ObjectNode);
						//MethodInfo mInfo = genericClassType.GetMethod("FindChild");
						//MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(prop.Type);

						ObjectNode correctNode = this.FindChild<ObjectNode>(prop.Name); //(ObjectNode)genericMethodInfo.Invoke(this, new object[] { prop.Name });

						MethodInfo mInfo = this.GetType().GetMethod("Instantiate");
						MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(info.PropertyType);

						object value = genericMethodInfo.Invoke(correctNode, null);
						info.SetValue(returnval, value); 

						#endregion
					}
					else
					{
						// Otherwise it's a value-type node.
						try
						{
							#region Value Node

							Type genericClassType = typeof(ObjectNode);
							MethodInfo mInfo = genericClassType.GetMethod("FindChild");
							MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(prop.Type);

							Node correctNode = (Node)genericMethodInfo.Invoke(this, new object[] { prop.Name });

							if (correctNode == null)
								throw new Exception();

							object value = prop.Type.GetProperty("Value").GetValue(correctNode);
							info.SetValue(returnval, value); 

							#endregion
						}
						catch (Exception ex)
						{
							throw new FormatException(string.Format("{0} named '{1}' did not contain expected {2} '{3}'", typeof(T).Name, this.Name, prop.Type.Name, prop.Name), ex);
						}
					}
				}
			}
			return returnval;
		}
	}
}
