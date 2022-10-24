using System.Collections.Generic;
using System.Linq;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public partial class Numbers
    {
        public static NumericBaseType[] ToNumericBaseTypeArray(float[] values) {
            if (values == null) return null;
            var valuesConverted = new NumericBaseType[values.Length];
            
            for (var i = 0; i < values.Length; i++) {
                valuesConverted[i] = (NumericBaseType)values[i];
            }
            return valuesConverted;
        }

        public static NumericBaseType[] ToNumericBaseTypeArray(double[] values) {
            if (values == null) return null;
            var valuesConverted = new NumericBaseType[values.Length];
            
            for (var i = 0; i < values.Length; i++) {
                valuesConverted[i] = (NumericBaseType)values[i];
            }
            return valuesConverted;
        }
        
        /// <summary>
        /// Convert array of doubles to array of floats
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static float[] ToFloats(this IEnumerable<double> values)
        {
            return values.Select(v => (float)v).ToArray();
        }
        
        public static float[] ToFloats(this IEnumerable<Numeric> values)
        {
            return values.Select(v => (float)v).ToArray();
        }

        /// <summary>
        /// Convert array of floats to array of doubles
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double[] ToDoubles(this IEnumerable<float> values)
        {
            return values.Select(v => (double)v).ToArray();
        }

        public static double[] ToDoubles(this IEnumerable<Numeric> values)
        {
            return values.Select(v => (double)v).ToArray();
        }
    }
}