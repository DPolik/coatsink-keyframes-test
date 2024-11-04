public class Program
{
    public static void Main()
    {
        System.Console.WriteLine("Program Start");
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        CodeTest();

        stopwatch.Stop();
        System.Console.WriteLine($"It took {stopwatch.ElapsedMilliseconds}ms to sample the array 100,000 times");
    }

    private static void CodeTest()
    {
        var keyframeManager = new KeyframeManager();

        // Add a million keyframes in order, then remove 100 at random
        var testKeyframeNumber = 1000000;
		
		// Add all keyframes in bulk since we know the amount we want
		keyframeManager.AddRandomKeyFramesInBulk(testKeyframeNumber);
        
		var rand = new System.Random();
		
        for (int i = 0; i < 100; i++)
        {
            keyframeManager.RemoveKeyframe(rand.Next(0, testKeyframeNumber));
            testKeyframeNumber--;
        }

        // Sample the keyframes 100,000 times
        for (int i = 0; i < 100000; i++)
        {
            var sampleTimeA = rand.NextSingle() * testKeyframeNumber;
            var sampledFrameA = keyframeManager.FindNearestKeyframe(sampleTimeA);

            var sampleTimeB = rand.NextSingle() * testKeyframeNumber;
            var sampledFrameB = keyframeManager.FindNearestKeyframe(sampleTimeB);

            var interpolatedFrame = keyframeManager.Interpolate(sampledFrameA, sampledFrameB);
        }
    }

    public class Keyframe
    {
        public float x, y, z;
        public float time;
        public override string ToString() { return $"(X:{x}, Y:{y}, Z:{z} Time:{time})"; }
    }

    public class KeyframeManager
    {
		private const int DefaultArraySize = 4;
		private const float ReduceArraySizeFactor = 2f;
		
        private Keyframe[] _keyframes;
        private int _keyframeCount;
		private bool CanReduceKeyframesArraySize => _keyframeCount < _keyframes.Length/4;

        public KeyframeManager()
        {
            _keyframes = new Keyframe[DefaultArraySize];
            _keyframeCount = 0;
        }
		
		public void AddKeyframe(float x, float y, float z, float time)
        {
            if (_keyframeCount == _keyframes.Length)
            {
                DoubleArraySize();
            }

            _keyframes[_keyframeCount] = new Keyframe { x = x, y = y, z = z, time = time };
            _keyframeCount++;
        }
		
		private void DoubleArraySize()
        {
            Keyframe[] newKeyframes = new Keyframe[_keyframes.Length * 2];
            _keyframes.CopyTo(newKeyframes, 0);
            _keyframes = newKeyframes;
        }
		
		public void AddRandomKeyFramesInBulk(int bulkNumber)
		{
			// Check if array is able to store all keyframes
			var extraArraySize = bulkNumber - (_keyframes.Length - _keyframeCount);
			if(extraArraySize > 0)
			{
				var finalArray = new Keyframe[(_keyframes.Length + extraArraySize) * 2]; // double to reduce the number of times resizing is needed
				_keyframes.CopyTo(finalArray, 0);
				_keyframes = finalArray;
			}
			
			var rand = new System.Random();			
			for (int i = 0; i < bulkNumber; i++)
			{
				AddKeyframe (rand.Next(0, 100), rand.Next(0, 100), rand.Next(0, 100), i + rand.NextSingle() );
			}
		}
        
        public void RemoveKeyframe(int index)
        {
            if (index < 0 || index >= _keyframeCount)
			{
				return;
			}
			
            for (int i = index; i < _keyframeCount - 1; i++)
            {
                _keyframes[i] = _keyframes[i + 1];
            }

            _keyframeCount--;
            _keyframes[_keyframeCount] = null;
			
			ReduceKeyframesArraySize();
        }
		
		private void ReduceKeyframesArraySize()
		{
			if(!CanReduceKeyframesArraySize)
			{
				return;
			}
			
			var newArray = new Keyframe[(int)(_keyframes.Length / ReduceArraySizeFactor)];
			for(int i = 0; i < _keyframeCount; i++)
			{
				newArray[i] = _keyframes[i];
			}
			_keyframes = newArray;
		}

        public Keyframe FindNearestKeyframe(float nearestToTime)
        {
			if(_keyframeCount == 0)
			{
				return null;
			}
			
			// If the requested time is before the first keyframe, return the first keyframe.
			if (nearestToTime <= _keyframes[0].time)
			{
				return _keyframes[0];
			}
			
            // Binary search to find the nearest keyframe before the given time
            var low = 0;
            var high = _keyframeCount - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
				if (_keyframes[mid].time == nearestToTime)
				{
					return _keyframes[mid];
				}
				
                if (_keyframes[mid].time < nearestToTime)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return high >= 0 ? _keyframes[high] : _keyframes[0];
        }

        public Keyframe Interpolate(Keyframe a, Keyframe b, float t = 0.5f)
        {
            if (a == null || b == null)
			{
				return null;
			}
			
			// Linear interpolation between a and b
            float x = a.x + t * (b.x - a.x);
			float y = a.y + t * (b.y - a.y);
			float z = a.z + t * (b.z - a.z);
			float time = a.time + t * (b.time - a.time);

            return new Keyframe { x = x, y = y, z = z, time = time };
        }
    }
}