using System;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public class RangeScaler
    {
        public RangeScaler(
            NumericBaseType inputMin = 0,
            NumericBaseType inputMax = 1,
            NumericBaseType outputMin = 0,
            NumericBaseType outputMax = 1,
            EaseFunction ease = null
        ) 
        {
            var isInvert = outputMin > outputMax;
            var outputMinFinal = isInvert ? outputMax : outputMin;
            var outputMaxFinal = isInvert ? outputMin : outputMax;

            InputMin = inputMin;
            InputMax = inputMax;
            
            OutputMin = outputMinFinal;
            OutputMax = outputMaxFinal;

            Ease = ease;
            IsInvert = isInvert;
        }

        /*--------------------------------------------------------------------*
         START: Properties
         *--------------------------------------------------------------------*/

        public NumericBaseType InputMin { get; set; }
        public NumericBaseType InputMax { get; set; }

        protected NumericBaseType _inputPrevious;
        protected NumericBaseType _lastOutputNormalized;

        public NumericBaseType OutputMin { get; set; } = 0f;
        public NumericBaseType OutputMax { get; set; } = 0f;
        
        public EaseFunction Ease { get; set; }
        public bool IsInvert { get; set; } = false;
        
        public NumericBaseType Value { get; private set; }
        public NumericBaseType Progress => _lastOutputNormalized;
        public NumericBaseType P => Progress;
        
        public bool IsRangeEnteredFromBelow { get; private set; }
        public bool IsRangeEnteredFromAbove { get; private set; }
        
        /*--------------------------------------------------------------------*
         START: Static Methods
         *--------------------------------------------------------------------*/
        
        /// <summary>
        /// Convert a value in one range into a value in another range.
        /// If the original range is approximately zero then the returned value is the
        /// average value of the new range, that is: (newMin + newMax) / 2.0
        /// </summary>
        /// <param name="originalValue">The original value in the original range.</param>
        /// <param name="originalMin">The minimum value in the original range.</param>
        /// <param name="originalMax">The maximum value in the original range.</param>
        /// <param name="newMin">The new range's minimum value.</param>
        /// <param name="newMax">The new range's maximum value.</param>
        /// <returns>The original value converted into the new range.</returns>
        public static NumericBaseType ScaleValue(
            NumericBaseType originalValue, 
            NumericBaseType originalMin, 
            NumericBaseType originalMax, 
            NumericBaseType newMin, 
            NumericBaseType newMax
        )
        {	
            var origRange = originalMax - originalMin;
            var newRange  = newMax  - newMin;
		
            NumericBaseType newValue;
            if (origRange > -Numbers.Epsilon && origRange < Numbers.Epsilon)
            {
                newValue = (newMin + newMax) / 2;
            }
            else
            {
                newValue = (((originalValue - originalMin) * newRange) / origRange) + newMin;				
            }
		
            return newValue;
        }
        
        /*--------------------------------------------------------------------*
         START: Public Methods
         *--------------------------------------------------------------------*/
        public float Scale(
            NumericBaseType input,
            bool isValueNormalized = false,
            bool isConstrain = true,
            bool isNormalizeReturnValue = false,
            bool isInvertReturnValue = false
        )
        {
            //----- Make sure we have a raw (not normalized or constrained) input value to work with.
            if (isValueNormalized) input = Numbers.Unnormalize(input, InputMin, InputMax, false);
            if (Math.Abs(input - _inputPrevious) < Numbers.Epsilon) return Value;
            _inputPrevious = input;
            
            _CheckRangeEntered(input);

            //----- Now get the raw, unconstrained output value.
            var output = ScaleValue(input, InputMin, InputMax, OutputMin, OutputMax);

            //----- Find the return value.
            var outputNormalized = Numbers.Normalize(output, OutputMin, OutputMax, isConstrain, isInvertReturnValue, Ease);
            if (IsInvert) outputNormalized = 1 - outputNormalized;
            _lastOutputNormalized = outputNormalized;

            var outputFinal = !isNormalizeReturnValue ? Numbers.Unnormalize(output, OutputMin, OutputMax) : output;

            //----- Store the value for possible later retrieval.
            Value = outputFinal;

            return outputFinal;
        }

        /*--------------------------------------------------------------------*
         START: Private Methods
         *--------------------------------------------------------------------*/

        protected void _CheckRangeEntered(float input)
        {
            IsRangeEnteredFromBelow = false;
            IsRangeEnteredFromAbove = false;
            
            if (
                (_inputPrevious < InputMin || _inputPrevious > InputMax)
                && (input >= InputMin && input <= InputMax)
            )
            {
                //----- The input value just entered the allowed range, while it was previously outside the range.
                if (_inputPrevious < InputMin)
                {
                    //----- We just entered the range from below the minimum.
                    IsRangeEnteredFromBelow = true;
                }
                else if (_inputPrevious > InputMax)
                {
                    //----- We just entered the range from above the maximum.
                    IsRangeEnteredFromAbove = true;
                }
            }
        }
    }
}