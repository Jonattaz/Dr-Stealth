using System;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public static partial class Numbers
    {
        public static NumericBaseType Round(NumericBaseType value, int decimalPlaces)
        {
            return (NumericBaseType)decimal.Round((decimal)value, decimalPlaces, MidpointRounding.AwayFromZero);
        }

        // /**
        //  * Decimal adjustment of a number.
        //  * See: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Math/round
        //  *
        //  * ----- Round
        //  * Numbers.round(55.55, -1);   // 55.6
        //  * Numbers.round(55.549, -1);  // 55.5
        //  * Numbers.round(55, 1);       // 60
        //  * Numbers.round(54.9, 1);     // 50
        //  * Numbers.round(-55.55, -1);  // -55.5
        //  * Numbers.round(-55.551, -1); // -55.6
        //  * Numbers.round(-55, 1);      // -50
        //  * Numbers.round(-55.1, 1);    // -60
        //  * Numbers.round(1.005, -2);   // 1.01
        //  *
        //  * ----- Floor
        //  * Numbers.floor(55.59, -1);   // 55.5
        //  * Numbers.floor(59, 1);       // 50
        //  * Numbers.floor(-55.51, -1);  // -55.6
        //  * Numbers.floor(-51, 1);      // -60
        //  *
        //  * ----- Ceil
        //  * Numbers.ceil(55.51, -1);    // 55.6
        //  * Numbers.ceil(51, 1);        // 60
        //  * Numbers.ceil(-55.59, -1);   // -55.5
        //  * Numbers.ceil(-59, 1);       // -50
        //  *
        //  * @param type {string} The type of adjustment.
        //  * @param value {number} The number to operate upon.
        //  * @param exp {number} The exponent (the 10 logarithm of the adjustment base).
        //  * @returns {number} The adjusted value.
        //  */
        // private static NumericBaseType _DecimalAdjust(string type, NumericBaseType value, int exp) 
        // {
        //     // If the exp is undefined or zero...
        //     if (exp == 0) {
        //         switch(type) {
        //             case "round":
        //                 return Numbers.Round(value);
        //             case "floor":
        //                 return Numbers.Floor(value);
        //             case "ceil":
        //                 return Numbers.Ceil(value);
        //         }
        //     }
        //
        //     // If the value is not a number or the exp is not an integer...
        //     if (!TypeTools.Types.IsNumber(value) || exp % 1 != 0) return float.NegativeInfinity;
        //
        //     // Shift
        //     var valueStringArray = value.ToString(CultureInfo.CurrentCulture).Split('e');
        //     if (valueStringArray.Length < 1) return value;
        //
        //     switch(type) {
        //         case "round":
        //             value = Numbers.Round(
        //                 (NumericBaseType)Convert.ToDouble(
        //                     valueStringArray[0] + "e" + (
        //                         valueStringArray.Length > 0 && !string.IsNullOrEmpty(valueStringArray[1]) ? 
        //                         Convert.ToDouble(valueStringArray[1]) - exp : 
        //                         exp
        //                     )
        //                 )
        //             );
        //             
        //             break;
        //         case "floor":
        //             value = Numbers.Floor(
        //                 (NumericBaseType)Convert.ToDouble(
        //                     valueStringArray[0] + "e" + (
        //                         valueStringArray.Length > 0 && !string.IsNullOrEmpty(valueStringArray[1]) ? 
        //                             Convert.ToDouble(valueStringArray[1]) - exp : 
        //                             exp
        //                     )
        //                 )
        //             );
        //             
        //             break;
        //         case "ceil":
        //             value = Numbers.Ceil(
        //                 (NumericBaseType)Convert.ToDouble(
        //                     valueStringArray[0] + "e" + (
        //                         valueStringArray.Length > 0 && !string.IsNullOrEmpty(valueStringArray[1]) ? 
        //                             Convert.ToDouble(valueStringArray[1]) - exp : 
        //                             exp
        //                     )
        //                 )
        //             );
        //             
        //             break;
        //     }
        //
        //     // Shift back
        //     valueStringArray = value.ToString().Split('e');
        //     
        //     return (NumericBaseType)Convert.ToDouble(
        //         valueStringArray[0] + "e" + (
        //             valueStringArray.Length > 0 && !string.IsNullOrEmpty(valueStringArray[1]) ? 
        //                 Convert.ToDouble(valueStringArray[1]) + exp : 
        //                 exp
        //         )
        //     );
        // }
    }
}