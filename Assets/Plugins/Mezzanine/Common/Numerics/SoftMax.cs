using System;
using System.Collections.Generic;
using System.Linq;

namespace Mz.Numerics 
{
	using NumericBaseType = System.Single;
	
	public static partial class Numbers 
	{
		/// From Accord.Net
		/// 
		/// <summary>
		///   Computes the Softmax function (also known as normalized Exponential
		///   function) that "squashes" a mzVector or arbitrary real values into a 
		///   mzVector of real values in the range (0, 1) that add up to 1.
		/// </summary>
		/// 
		/// <param name="input">The real values to be converted into the unit interval.</param>
		/// 
		/// <returns>A mzVector with the same number of dimensions as <paramref name="input"/>
		///   but where values lie between 0 and 1.</returns>
		/// 
		public static NumericBaseType[] Softmax(NumericBaseType[] input) {
			return _Softmax_2(input, new NumericBaseType[input.Length]);
		}

		private static NumericBaseType[] _Softmax_2(NumericBaseType[] input, NumericBaseType[] result) {
			var sum = _LogSumExp(input);
			for (var i = 0; i < input.Length; i++) result[i] = Numbers.Exp(input[i] - sum);
			return result;
		}

		private static NumericBaseType _LogSumExp(this IEnumerable<NumericBaseType> values) {
			return values.Aggregate(NumericBaseType.NegativeInfinity, (current, t) => _LogSum_2(t, current));
		}

		private static NumericBaseType _LogSum(IEnumerable<NumericBaseType> values) {
			return values.Aggregate(NumericBaseType.NegativeInfinity, _LogSum_2);
		}

		private static NumericBaseType _LogSum_2(NumericBaseType lnx, NumericBaseType lny) {
			if (NumericBaseType.IsNegativeInfinity(lnx)) return lny;
			if (NumericBaseType.IsNegativeInfinity(lny)) return lnx;

			if (lnx > lny) return lnx + _Log1p(Numbers.Exp(lny - lnx));

			return lny + _Log1p(Numbers.Exp(lnx - lny));
		}

		private static NumericBaseType _Log1p(NumericBaseType x) {
			if (x <= -1.0) return NumericBaseType.NaN;

			if (Math.Abs(x) > 1e-4) return Numbers.Log(1 + x);

			// Use Taylor approx. log(1 + x) = x - x^2/2 with error roughly x^3/3
			// Since |x| < 10^-4, |x|^3 < 10^-12, relative error less than 10^-8
			return (-(NumericBaseType)0.5 * x + 1) * x;
		}
	} 
}
