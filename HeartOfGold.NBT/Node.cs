using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace HeartOfGold.NBT
{
	/// <summary>
	/// The base Node type.
	/// </summary>
	[XmlRoot("Node", Namespace = "HeartOfGold.NBT")]
	[XmlInclude(typeof(ListNode)), XmlInclude(typeof(ByteNode)), XmlInclude(typeof(StringNode)), XmlInclude(typeof(IntNode)),
	XmlInclude(typeof(FloatNode)), XmlInclude(typeof(DoubleNode)), XmlInclude(typeof(LongNode)), XmlInclude(typeof(ObjectNode))]
	[Serializable()]
    public abstract class Node : INotifyPropertyChanged
    {
		string _name = string.Empty;

		/// <summary>
		/// The name of this Node. Can represent type, a named value, or a description.
		/// </summary>
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

		/// <summary>
		/// True if this is a value-type node
		/// </summary>
		public virtual bool HasValue { get { return true; } }

		/// <summary>
		/// True if this is a root-type node.
		/// </summary>
		public virtual bool CanHaveChildren { get { return false; } }

		#region INotifyPropertyChanged Stuff

		/// <summary>
		/// Notify any data-bound UIs that this object has updated a property.
		/// </summary>
		/// <param name="PropName"></param>
		protected void PropChanged(string PropName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(PropName));
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged; 

		#endregion
	}
}