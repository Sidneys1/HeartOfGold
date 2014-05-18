using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HeartOfGold.NBT
{
	[XmlRoot("Node", Namespace = "HeartOfGold.NBT")]
	[XmlInclude(typeof(ListNode))]
	[XmlInclude(typeof(ByteNode))]
	[XmlInclude(typeof(StringNode))]
	[XmlInclude(typeof(IntNode))]
	[XmlInclude(typeof(FloatNode))]
	[XmlInclude(typeof(DoubleNode))]
	[XmlInclude(typeof(LongNode))]
	[XmlInclude(typeof(ObjectNode))]
	[Serializable()]
    public abstract class Node : INotifyPropertyChanged
    {
		string _name = string.Empty;

		[XmlAttribute("Name")]
		public string Name 
		{
			get
			{
				return _name;
			} 
			set
			{
				_name = value;
				PropChanged("Name");
			}
		}
		public virtual bool HasValue { get { return true; } }
		public virtual bool CanHaveChildren { get { return false; } }

		protected void PropChanged(string PropName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(PropName));
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
