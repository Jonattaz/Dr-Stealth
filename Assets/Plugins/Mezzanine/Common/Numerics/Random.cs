using System;

namespace Mz.Numerics
{
	using NumericBaseType = System.Single;

	public interface IRandom
	{
		NumericBaseType Next(NumericBaseType min = 0, NumericBaseType max = 1);
		int NextInt(int min, int max);
		long NextLong();
	}

	public class Randomizer : IRandom
	{
		public enum RngType
		{
			System,
			MarsenneTwister
		}
		
		public Randomizer(long seed = -1, RngType type = RngType.System)
		{
			if (seed < 0) seed = DateTime.Now.Ticks;
			Seed = seed;
			Type = type;
			
			if (type == RngType.System) _random = new Random((int)seed);
			else if (type == RngType.MarsenneTwister) _randomMt = new RandomMarsenneTwister(seed);
		}
		
		public long Seed { get; set; }
		public RngType Type { get; set; }
		
		private Random _random;
		private RandomMarsenneTwister _randomMt;
		
		public NumericBaseType Next(NumericBaseType min = 0, NumericBaseType max = 1)
		{
			NumericBaseType random = 0;

			if (Type == RngType.System && _random != null)
			{
				random = (NumericBaseType)_random.NextDouble();
			}
			else if (Type == RngType.MarsenneTwister && _randomMt != null)
			{
				random = _randomMt.Random0To1();
			}
			
			return random * (max - min) + min;

		}

		public int NextInt(int min, int max)
		{
			int random = 0;
			
			if (Type == RngType.System && _random != null)
			{
				random = _random.Next(min, max);
			}
			else if (Type == RngType.MarsenneTwister && _randomMt != null)
			{
				var factor = _randomMt.Random0To1();
				random = (int)(factor * (max - min) + min);
			}

			return random;
		}
		
		public long NextLong() 
		{
			long random = 0;
			
			if (Type == RngType.System && _random != null)
			{
				var buffer = new byte[8];
				_random.NextBytes(buffer);
				random = (long)(BitConverter.ToUInt64(buffer, 0) & long.MaxValue);
			}
			else if (Type == RngType.MarsenneTwister && _randomMt != null)
			{
				random = _randomMt.RandomUnsignedInt();
			}

			return random;
		}
	}
	
	public static partial class Numbers {
		private static Random _random;
		public static Random Random => _random ?? (_random = new Random());

		public static IRandom GetRandom(long seed = -1, Randomizer.RngType type = Randomizer.RngType.System) {
			return new Randomizer(seed, type);
		}

		public static NumericBaseType Next(NumericBaseType min = 0, NumericBaseType max = 1) { 
			var random = (NumericBaseType)Random.NextDouble();
			return random * (max - min) + min;

		}
		
		public static int NextInt(int min, int max) { return Random.Next(min, max);  }
		
		public static long NextLong() {
			var buffer = new byte[8];
			Random.NextBytes(buffer);
			return (long)(BitConverter.ToUInt64(buffer, 0) & long.MaxValue);
		}
	} 
}
