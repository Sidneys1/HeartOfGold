using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ProtoBuf;

namespace HeartOfGold.NBT
{
	/// <summary>
	/// The base Node type.
	/// </summary>
	[XmlRoot("Node", Namespace = "HeartOfGold.NBT")]
	[XmlInclude(typeof(ListNode)), XmlInclude(typeof(ByteNode)), XmlInclude(typeof(StringNode)), XmlInclude(typeof(IntNode)),
	XmlInclude(typeof(FloatNode)), XmlInclude(typeof(DoubleNode)), XmlInclude(typeof(LongNode)), XmlInclude(typeof(ObjectNode))]
	[ProtoInclude(2, typeof(ContainerNode)), ProtoInclude(3, typeof(ByteNode)), ProtoInclude(4, typeof(StringNode)), ProtoInclude(5, typeof(IntNode)),
	ProtoInclude(6, typeof(FloatNode)), ProtoInclude(7, typeof(DoubleNode)), ProtoInclude(8, typeof(LongNode))]
	[Serializable, ProtoContract]
    public abstract class Node : INotifyPropertyChanged
    {
		private string _name = string.Empty;

		/// <summary>
		/// The name of this Node. Can represent type, a named value, or a description.
		/// </summary>
		[XmlAttribute("Name"), ProtoMember(1)]
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
		[IgnoreDataMember]
		public virtual bool HasValue => true;

		/// <summary>
		/// True if this is a root-type node.
		/// </summary>
		[IgnoreDataMember]
		public virtual bool CanHaveChildren => false;

		#region INotifyPropertyChanged Stuff

		/// <summary>
		/// Notify any data-bound UIs that this object has updated a property.
		/// </summary>
		/// <param name="propName"></param>
		protected void PropChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged; 

		#endregion
	}
}