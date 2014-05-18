using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
			r.Dispose();
			this.Children = n.Children;
			this.Name = n.Name;
			r.Close();
			r.Dispose();
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
	}
}
