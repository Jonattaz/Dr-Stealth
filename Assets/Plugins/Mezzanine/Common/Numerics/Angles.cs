namespace Mz.Numerics
{
    using NumericBaseType = System.Single;

    public static partial class Numbers
    {
        /// <summary>
        /// Hold angle between -180 and 180 degrees.
        /// </summary>
        /// <returns>The constrained angle</returns>
        /// <param name="angleDegrees">Angle degrees.</param>
        public static NumericBaseType ConstrainAngle180(NumericBaseType angleDegrees)
        {
            if (angleDegrees > 180) angleDegrees -= 360;
            else if (angleDegrees < -180) angleDegrees += 360;
            return angleDegrees;
        }

        public static NumericBaseType ConstrainAngle360(NumericBaseType angleDegrees)
        {
            return (angleDegrees + 360) % 360;
        }

        public static NumericBaseType GetTurnAngle(NumericBaseType angleDegreesCurrent,
            NumericBaseType angleDegreesTarget)
        {
            NumericBaseType difference = angleDegreesTarget - angleDegreesCurrent;

            difference = ConstrainAngle360(difference);

            return difference;
        }

        public static NumericBaseType GetShortestAngle(NumericBaseType startAngleDegrees,
            NumericBaseType endAngleDegrees)
        {
            NumericBaseType MAX_ANGLE = 360;

            NumericBaseType distanceForward; // Clockwise
            NumericBaseType distanceBackward; // Counter-Clockwise

            // Calculate both distances, forward and backward:
            distanceForward = endAngleDegrees - startAngleDegrees;
            distanceBackward = startAngleDegrees - endAngleDegrees;

            // Which direction is shortest?
            if (NormalizeAngle(distanceForward) < NormalizeAngle(distanceBackward))
            {
                // Adjust for 360/0 degree wrap
                if (endAngleDegrees < startAngleDegrees) endAngleDegrees += MAX_ANGLE; // Will be above 360
            }

            // Backward?
            else
            {
                // Adjust for 360/0 degree wrap
                if (endAngleDegrees > startAngleDegrees) endAngleDegrees -= MAX_ANGLE; // Will be below 0
            }

            // Now, you can lerp between startAngle and endAngle. 

            // EndAngle can be above 360 if wrapping clockwise past 0, or
            // EndAngle can be below 0 if wrapping counter-clockwise before 0.
            // Normalize each returned Lerp value to bring angle in range of 0 to 360 if required.

            return endAngleDegrees;
        }

        public static NumericBaseType NormalizeAngle(NumericBaseType angleDegrees)
        {
            NumericBaseType MAX_ANGLE = 360;
            while (angleDegrees < 0) angleDegrees += MAX_ANGLE;
            while (angleDegrees >= MAX_ANGLE) angleDegrees -= MAX_ANGLE;
            return angleDegrees;
        }

        public static NumericBaseType ToRadians(this NumericBaseType angleDegrees)
        {
            return Pi / 180 * angleDegrees;
        }

        public static NumericBaseType ToDegrees(NumericBaseType angleRadians)
        {
            return 180 / Pi * angleRadians;
        }

        // Degrees-to-radians conversion constant (RO).
        public const NumericBaseType Deg2Rad = Pi * 2 / 360;

        // Radians-to-degrees conversion constant (RO).
        public const NumericBaseType Rad2Deg = 1 / Deg2Rad;

        // Same as ::ref::Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
        public static NumericBaseType LerpAngle(NumericBaseType a, NumericBaseType b, NumericBaseType t)
        {
            var delta = Repeat((b - a), 360);
            if (delta > 180) delta -= 360;
            return a + delta * Clamp0To1(t);
        }

        // Same as ::ref::MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees.
        public static NumericBaseType MoveTowardsAngle(NumericBaseType current, NumericBaseType target,
            NumericBaseType maxDelta)
        {
            var deltaAngle = DeltaAngle(current, target);
            if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
                return target;
            target = current + deltaAngle;
            return MoveTowards(current, target, maxDelta);
        }

        // Gradually changes an angle given in degrees towards a desired goal angle over time.
        public static NumericBaseType SmoothDampAngle(
            NumericBaseType current,
            NumericBaseType target,
            ref NumericBaseType currentVelocity,
            NumericBaseType smoothTime,
            NumericBaseType deltaTime,
            NumericBaseType maxSpeed = Numbers.Infinity
        )
        {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        // Calculates the shortest difference between two given angles.
        public static NumericBaseType DeltaAngle(NumericBaseType current, NumericBaseType target)
        {
            var delta = Repeat((target - current), 360.0f);
            if (delta > 180.0f) delta -= 360.0f;
            return delta;
        }
    }
}