using System;

namespace HeartOfGold.MonoGame.Helpers.Collections {
	class MovingAverage<T> {
		public readonly CircularArray<T> InnerArray;
		double _total;
		readonly uint _samples;
		uint _count;
		Func<T, double> _add; 

		public MovingAverage(uint samples, Func<T, double> add) {
			InnerArray = new CircularArray<T>(samples);
			_samples = samples;
			_add = add;
		}

		public void AddSample(T sample) {
			if (InnerArray.IsFilled) {
				_total -= _add(InnerArray.Get(_count-1));
			} else
				_count++;
			InnerArray.Push(sample);
			_total += _add(sample);
		}

		public double Average => _count > 0 ? _total / _count : double.NaN;
	}
}
