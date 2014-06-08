using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HeartOfGold.NBT
{
	[Serializable()]
	public abstract class ContainerNode : Node
	{
		[XmlElement(Type = typeof(NBT.ByteNode)),
		XmlElement(Type = typeof(NBT.ListNode)),
		XmlElement(Type = typeof(NBT.StringNode)),
		XmlElement(Type = typeof(NBT.IntNode)),
		XmlElement(Type = typeof(NBT.FloatNode)),
		XmlElement(Type = typeof(NBT.DoubleNode)),
		XmlElement(Type = typeof(NBT.LongNode)),
		XmlElement(Type = typeof(NBT.ObjectNode))]
		public ObservableCollection<Node> Children { get; protected set; }
	}

	[Serializable()]
	public class ListNode : ContainerNode
	{
		public ListNode()
		{
			Children = new ObservableCollection<Node>();
		}

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


		public Node this[int key]
		{
			get { return Children[key]; }
			set { Children[key] = value; }
		}

		public override bool HasValue
		{
			get { return false; }
		}

		public override bool CanHaveChildren
		{
			get
			{
				return true;
			}
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

		public override bool HasValue
		{
			get { return false; }
		}

		public override bool CanHaveChildren
		{
			get
			{
				return true;
			}
		}

		public T FindChild<T>(string Name) where T : Node
		{
			return (T)Children.FirstOrDefault(o => o is T && o.Name == Name);
		}

		public T Instantiate<T>()
		{
			PropertyInfo[] props = typeof(T).GetProperties();
			T returnval = Activator.CreateInstance<T>();
			foreach (PropertyInfo info in props)
			{
				if (Attribute.IsDefined(info, typeof(PropertyAttribute)))
				{
					PropertyAttribute prop = (PropertyAttribute)info.GetCustomAttribute(typeof(PropertyAttribute));

					if (prop.Type == typeof(ListNode))
					{ // When a node is a parent-type list node. This might be a bit harder, as we can only know the superclass to expect, not the subclasses. (An inventory contains items, but they might be weapons or armor, we don't know!)
						//Type t = Type.GetType(string.Format(prop.ChildLookup, ), true, true);
						//Activator.CreateInstance(t);
					}
					else if (prop.Type == typeof(ObjectNode))
					{ // When a node is a parent-type object node. Fairly easy as we know the exact subclass to expect.
						Type genericClassType = typeof(ObjectNode);
						MethodInfo mInfo = genericClassType.GetMethod("FindChild");
						MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(prop.Type);

						ObjectNode correctNode = (ObjectNode)genericMethodInfo.Invoke(this, new object[] { prop.Name });

						mInfo = genericClassType.GetMethod("Instantiate");
						genericMethodInfo = mInfo.MakeGenericMethod(info.PropertyType);

						object value = genericMethodInfo.Invoke(correctNode, null);
						info.SetValue(returnval, value);
					}
					else
					{ // Otherwise it's a value-type node.
						try
						{
							Type genericClassType = typeof(ObjectNode);
							MethodInfo mInfo = genericClassType.GetMethod("FindChild");
							MethodInfo genericMethodInfo = mInfo.MakeGenericMethod(prop.Type);

							Node correctNode = (Node)genericMethodInfo.Invoke(this, new object[] { prop.Name });

							if (correctNode == null)
								throw new Exception();

							object value = prop.Type.GetProperty("Value").GetValue(correctNode);
							info.SetValue(returnval, value);
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
