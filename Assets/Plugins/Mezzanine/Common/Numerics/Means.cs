using System.Linq;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public static partial class Means
    {
        public static NumericBaseType GeometricMean(this NumericBaseType[] values)
        {
            var sum = values.Aggregate((NumericBaseType)1, (current, t) => current * t);
            return Numbers.Pow(sum, (NumericBaseType)1 / values.Length);
        }

        // See: https://www.geeksforgeeks.org/geometric-mean-two-methods/
        public static NumericBaseType GeometricMeanLog(this NumericBaseType[] values)
        {
            return Numbers.Exp(values.Sum(t => Numbers.Log(t)) / values.Length);
        }
    }
}