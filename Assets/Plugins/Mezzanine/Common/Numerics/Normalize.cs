namespace Mz.Numerics
{
    public partial class Numbers
    {
        /// <summary>
        /// Returns the relative position of the input value
        /// within the min/max range as a value between 0.0 and 1.0.
        /// </summary>
        public static float Normalize(
            float value, 
            float min, 
            float max, 
            bool isConstrain = true,
            bool isInvert = false,
            EaseFunction ease = null
        )
        {
            var p = (value - min) / (max - min);

            if (isConstrain)
            {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }

            if (isInvert) p = 1 - p;

            if (ease != null) p = ease(p);
            
            return p;
        }

        /// <summary>
        /// Accepts a value between 0.0 and 1.0 and returns the input value at the corresponding
        /// progress point within the min/max input range.
        /// </summary>
        public static float Unnormalize(
            float valueNormalized, 
            float min, 
            float max, 
            bool isConstrain = true,
            bool isInvert = false
        )
        {
            var p = valueNormalized;

            if (isConstrain)
            {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }

            if (isInvert) p = 1 - p;
            return (min + (p * (max - min)));
        }
    }
}