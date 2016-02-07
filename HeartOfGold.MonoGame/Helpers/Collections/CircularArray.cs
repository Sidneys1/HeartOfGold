using System;

namespace HeartOfGold.MonoGame.Helpers.Collections {
	public class CircularArray<T> {
		private readonly T[] _facadeArray;
		private int _head;

		public CircularArray(uint length) {
			BaseArray = new T[length];
			_facadeArray = new T[length];
		}

		public T[] Array {
			get {
				var pos = _head;
				for (var i = 0; i < BaseArray.Length; i++) {
					Math.DivRem(pos, BaseArray.Length, out pos);
					_facadeArray[i] = BaseArray[pos];
					pos++;
				}
				return _facadeArray;
			}
		}

		public T[] BaseArray { get; }

		public bool IsFilled { get; private set; }

		public void Push(T value) {
			if (!IsFilled && _head == BaseArray.Length - 1)
				IsFilled = true;

			Math.DivRem(_head, BaseArray.Length, out _head);
			BaseArray[_head] = value;
			_head++;
		}

		public T Get(uint indexBackFromHead) {
			var pos = _head - indexBackFromHead - 1;
			pos = pos < 0 ? pos + BaseArray.Length : pos;
			Math.DivRem(pos, BaseArray.Length, out pos);
			return BaseArray[pos];
		}
	}
}
