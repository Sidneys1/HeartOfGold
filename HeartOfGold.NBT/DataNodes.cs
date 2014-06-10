using System;
using System.Xml.Serialization;

namespace HeartOfGold.NBT
{
	/// <summary>
	/// A Value-Node representing a byte value
	/// </summary>
	[Serializable()]
	public class ByteNode : Node
	{
		byte _value = 0;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value")]
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
	[Serializable()]
	public class StringNode : Node
	{
		string _value = string.Empty;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value")]
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
	[Serializable()]
	public class IntNode : Node
	{
		int _value = 0;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value")]
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
	[Serializable()]
	public class FloatNode : Node
	{
		float _value = 0f;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value")]
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
	[Serializable()]
	public class DoubleNode : Node
	{
		double _value = 0.0;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value")]
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
	[Serializable()]
	public class LongNode : Node
	{
		long _value = 0L;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[XmlAttribute("Value")]
		public long Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public LongNode() { }

		public LongNode(long value) { Value = value; }
	}
}
