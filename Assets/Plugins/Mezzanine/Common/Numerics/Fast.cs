namespace Mz.Numerics
{
    using NumericBaseType = System.Single;

    public partial class Numbers
    {
        // See: https://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int
        public static NumericBaseType PowFast(NumericBaseType value, int exponent)
        {
            NumericBaseType result = 1;
            
            while (exponent > 0)
            {
                if (exponent % 2 == 1) result *= value;
                exponent >>= 1;
                value *= value;
            }

            return result;
        }
    }
}