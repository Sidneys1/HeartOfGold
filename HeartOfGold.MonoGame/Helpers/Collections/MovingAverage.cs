namespace HeartOfGold.MonoGame.Helpers.Collections {
	class MovingAverage {
		readonly CircularArray<double> _innerArray;
		double _total;
		readonly uint _samples;
		uint _count;

		public MovingAverage(uint samples) {
			_innerArray = new CircularArray<double>(samples);
			_samples = samples;
		}

		public void AddSample(double sample) {
			if (_innerArray.IsFilled) {
				_total -= _innerArray.Get(_count-1);
			} else
				_count++;
			_innerArray.Push(sample);
			_total += sample;
		}

		public double Average => _count > 0 ? _total / _count : double.NaN;
	}
}
