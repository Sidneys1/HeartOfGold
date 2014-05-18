using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HeartOfGold.NBT
{
	[Serializable()]
	public class ByteNode : Node
	{
		byte _value = 0;
		[XmlAttribute("Value")]
		public byte Value 
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public ByteNode(){ }

		public ByteNode(byte value) { Value = value; }
	}

	[Serializable()]
	public class StringNode : Node
	{
		string _value = string.Empty;
		[XmlAttribute("Value")]
		public string Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public StringNode() { }

		public StringNode(string value) { Value = value; }
	}

	[Serializable()]
	public class IntNode : Node
	{
		int _value = 0;
		[XmlAttribute("Value")]
		public int Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public IntNode() { }

		public IntNode(int value) { Value = value; }
	}

	[Serializable()]
	public class FloatNode : Node
	{
		float _value = 0f;
		[XmlAttribute("Value")]
		public float Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public FloatNode() { }

		public FloatNode(float value) { Value = value; }
	}

	[Serializable()]
	public class DoubleNode : Node
	{
		double _value = 0.0;
		[XmlAttribute("Value")]
		public double Value
		{
			get { return _value; }
			set { _value = value; PropChanged("Value"); }
		}

		public DoubleNode() { }

		public DoubleNode(double value) { Value = value; }
	}

	[Serializable()]
	public class LongNode : Node
	{
		long _value = 0L;
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
