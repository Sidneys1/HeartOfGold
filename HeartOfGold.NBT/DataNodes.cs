using System;
using System.Xml.Serialization;
using ProtoBuf;

namespace HeartOfGold.NBT
{
	/// <summary>
	/// A Value-Node representing a byte value
	/// </summary>
	[Serializable, ProtoContract]
	public class ByteNode : Node
	{
		private byte _value;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value"), ProtoMember(1)]
		public byte Value 
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public ByteNode(){ }

		public ByteNode(byte value) { Value = value; }
	}

	/// <summary>
	/// A Value-Node representing a text value
	/// </summary>
	[Serializable, ProtoContract]
	public class StringNode : Node
	{
		private string _value = string.Empty;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value"), ProtoMember(1)]
		public string Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public StringNode() { }

		public StringNode(string value) { Value = value; }
	}

	/// <summary>
	/// A Value-Node representing an integer value
	/// </summary>
	[Serializable, ProtoContract]
	public class IntNode : Node
	{
		private int _value;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value"), ProtoMember(1)]
		public int Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public IntNode() { }

		public IntNode(int value) { Value = value; }
	}

	/// <summary>
	/// A Value-Node representing a floating-point value
	/// </summary>
	[Serializable, ProtoContract]
	public class FloatNode : Node
	{
		private float _value;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value"), ProtoMember(1)]
		public float Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public FloatNode() { }

		public FloatNode(float value) { Value = value; }
	}

	/// <summary>
	/// A Value-Node representing a double-precision value
	/// </summary>
	[Serializable, ProtoContract]
	public class DoubleNode : Node
	{
		private double _value;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value"), ProtoMember(1)]
		public double Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public DoubleNode() { }

		public DoubleNode(double value) { Value = value; }
	}

	/// <summary>
	/// A Value-Node representing a long integer value
	/// </summary>
	[Serializable, ProtoContract]
	public class LongNode : Node
	{
		private long _value;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value"), ProtoMember(1)]
		public long Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public LongNode() { }

		public LongNode(long value) { Value = value; }
	}
}
