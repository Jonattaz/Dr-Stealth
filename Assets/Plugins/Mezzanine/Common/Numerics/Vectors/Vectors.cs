namespace Mz.Numerics
{
    using NumericBaseType = System.Single;

    public static partial class Vectors
    {
        public static MzVector Zero => new MzVector(0, 0, 0);
        public static MzVector Identity => Zero;
        public static MzVector One => new MzVector(1, 1, 1);

        public static MzVector UnitX => new MzVector(1, 0, 0);
        public static MzVector UnitY => new MzVector(0, 1, 0);
        public static MzVector UnitZ => new MzVector(0, 0, 1);

        public static NumericBaseType[] Xy(NumericBaseType[] values)
        {
            var valuesNew = new NumericBaseType[] {0, 0};
            var length = values.Length > 2 ? 2 : values.Length;
            for (var i = 0; i < length; i++)
            {
                valuesNew[i] = values[i];
            }

            return valuesNew;
        }

        public static NumericBaseType[] Xyz(NumericBaseType[] values)
        {
            var valuesNew = new NumericBaseType[] {0, 0, 0};
            var length = values.Length > 3 ? 3 : values.Length;
            for (var i = 0; i < length; i++)
            {
                valuesNew[i] = values[i];
            }

            return valuesNew;
        }

        public static NumericBaseType Distance(MzVector a, MzVector b)
        {
            return Metrics.Distance(a.Values, b.Values);
        }

        public static MzVector Abs(MzVector mzVector, bool isModifyOriginal = true)
        {
            var valuesAbs = Metrics.Abs(mzVector.Values, isModifyOriginal);
            return isModifyOriginal ? mzVector : new MzVector(valuesAbs);
        }

        // Cardinal axes
        public static MzVector AxisX { get; } = new MzVector(1, 0, 0);
        public static MzVector AxisY { get; } = new MzVector(0, 1, 0);
        public static MzVector AxisZ { get; } = new MzVector(0, 0, 1);
        
        public static NumericBaseType Dot(MzVector lhs, MzVector rhs)
        {
            return Metrics.Dot(lhs.Values, rhs.Values);
        }

        public static MzVector Cross(MzVector lhs, MzVector rhs)
        {
            var values = Cross(lhs.Values, rhs.Values);
            return new MzVector(values);
        }

        public static NumericBaseType[] Cross(NumericBaseType[] lhs, NumericBaseType[] rhs)
        {
            lhs = new[] {lhs[0], lhs[1], lhs.Length > 2 ? lhs[2] : 0};
            rhs = new[] {rhs[0], rhs[1], rhs.Length > 2 ? rhs[2] : 0};

            return new[]
            {
                lhs[1] * rhs[2] - lhs[2] * rhs[1],
                lhs[2] * rhs[0] - lhs[0] * rhs[2],
                lhs[0] * rhs[1] - lhs[1] * rhs[0]
            };
        }
        
        /// <summary>
        /// Returns: a * (1 - bias) + b * bias
        /// A bias of 0 returns a, while 1 returns b.
        /// 0.5 is an interpolation between the two.
        /// </summary>        
        public static MzVector Interpolate(MzVector a, MzVector b, float bias = 0.5f) {
            return new MzVector(a.X * (1.0f - bias) + b.X * bias, a.Y * (1.0f - bias) + b.Y * bias);
        }

        /// <summary>
        /// Return the global pitch of this mzVector about the global X-Axis. The returned value is within the range -180..180
        /// </summary>
        public static NumericBaseType Pitch(MzVector v, bool isDegrees = true)
        {
            MzVector xProjected = ProjectOnPlane(v, AxisX, false);
            var pitch = Angle(AxisZ.Negated, xProjected, isDegrees);
            return xProjected.Y < 0.0 ? -pitch : pitch;
        }

        /// <summary>
        /// Return the global yaw of this mzVector about the global Y-Axis. The returned value is within the range -180..180
        /// </summary>
        public static NumericBaseType Yaw(MzVector v, bool isDegrees = true)
        {
            var yProjected = ProjectOnPlane(v, AxisY, false);
            var yaw = Angle(AxisZ.Negated, yProjected, isDegrees);
            return yProjected.X < 0.0 ? -yaw : yaw;
        }

        /// <summary>
        /// Projects a mzVector onto another mzVector.
        /// </summary>
        public static MzVector Project(MzVector mzVector, MzVector onNormal, bool isModifyOriginal = true)
        {
            var values = Project(mzVector.Values, onNormal.Values, isModifyOriginal);
            return isModifyOriginal ? mzVector : new MzVector(values);
        }

        public static NumericBaseType[] Project(NumericBaseType[] vector, NumericBaseType[] onNormal,
            bool isModifyOriginal = true)
        {
            var sqrMag = Metrics.Dot(onNormal, onNormal);
            if (sqrMag < Numbers.Epsilon) return Zero.Values;

            var dot = Metrics.Dot(vector, onNormal);
            var vectorNew = isModifyOriginal ? vector : new NumericBaseType[3];

            vectorNew[0] = onNormal[0] * dot / sqrMag;
            vectorNew[1] = onNormal[1] * dot / sqrMag;
            vectorNew[2] = onNormal.Length > 2 ? onNormal[2] * dot / sqrMag : 0;

            return vectorNew;
        }

        /// <summary>
        /// Projects a mzVector onto a plane defined by a normal orthogonal to the plane.
        /// </summary>
        public static MzVector ProjectOnPlane(MzVector mzVector, MzVector onNormal, bool isModifyOriginal = true)
        {
            var values = ProjectOnPlane(mzVector.Values, onNormal.Values, isModifyOriginal);
            return isModifyOriginal ? mzVector : new MzVector(values);
        }

        public static NumericBaseType[] ProjectOnPlane(NumericBaseType[] vector, NumericBaseType[] planeNormal,
            bool isModifyOriginal = true)
        {
            var sqrMag = Metrics.Dot(planeNormal, planeNormal);
            if (sqrMag < Numbers.Epsilon) return vector;
            else
            {
                var dot = Metrics.Dot(vector, planeNormal);
                var vectorNew = isModifyOriginal ? vector : new NumericBaseType[3];

                vectorNew[0] = vector[0] - planeNormal[0] * dot / sqrMag;
                vectorNew[1] = vector[1] - planeNormal[1] * dot / sqrMag;
                vectorNew[2] = vector.Length > 2 && planeNormal.Length > 2
                    ? vector[2] - planeNormal[2] * dot / sqrMag
                    : 0;

                return vectorNew;
            }
        }
        
        // Reflects a vector off the plane defined by a normal.
        public static MzVector Reflect(MzVector inDirection, MzVector inNormal) 
        {
            return -2F * Dot(inNormal, inDirection) * inNormal + inDirection;
        }

        /// <summary>
        /// Returns the angle in degrees between /from/ and /to/. This will be between 0 and 180 degrees.
        /// </summary>
        public static NumericBaseType Angle(MzVector from, MzVector to, bool isDegrees = true)
        {
            return Angle(from.Values, to.Values, isDegrees);
        }

        public static NumericBaseType Angle(NumericBaseType[] from, NumericBaseType[] to, bool isDegrees = true)
        {
            var kEpsilonNormalSqrt = 1e-15F;

            // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
            var denominator = Numbers.Sqrt(Metrics.MagnitudeSquared(from) * Metrics.MagnitudeSquared(to));
            if (denominator < kEpsilonNormalSqrt) return 0F;

            var dot = Numbers.Clamp(Metrics.Dot(from, to) / denominator, -1F, 1F);
            return isDegrees ? Numbers.Acos(dot) * Numbers.Rad2Deg : (NumericBaseType) Numbers.Acos(dot);
        }

        /// <summary>
        /// The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        /// If you imagine the /from/ and /to/ vectors as lines on a piece of paper, both originating from the same point, then the /axis/ mzVector would point up out of the paper.
        /// The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        /// </summary>
        public static NumericBaseType AngleSigned(MzVector from, MzVector to, MzVector axis)
        {
            return AngleSigned(from.Values, to.Values, axis.Values);
        }

        public static NumericBaseType AngleSigned(NumericBaseType[] from, NumericBaseType[] to, NumericBaseType[] axis)
        {
            from = new NumericBaseType[] {from[0], from[1], from.Length > 2 ? from[2] : 0};
            to = new NumericBaseType[] {to[0], to[1], to.Length > 2 ? to[2] : 0};
            axis = new NumericBaseType[] {axis[0], axis[1], axis.Length > 2 ? axis[2] : 0};

            var unsignedAngle = Angle(from, to);
            var crossX = from[1] * to[2] - from[2] * to[1];
            var crossY = from[2] * to[0] - from[0] * to[2];
            var crossZ = from[0] * to[1] - from[1] * to[0];
            var sign = Numbers.Sign(axis[0] * crossX + axis[1] * crossY + axis[2] * crossZ);
            return unsignedAngle * sign;
        }

        public static bool IsMovingInto(MzVector direction, MzVector normal)
        {
            return IsMovingInto(direction.Values, normal.Values);
        }

        public static bool IsMovingInto(NumericBaseType[] direction, NumericBaseType[] normal)
        {
            return Angle(direction, normal) > 90f;
        }

        // Linearly interpolates between two vectors.
        public static MzVector Lerp(MzVector a, MzVector b, float t) 
        {
            t = Numbers.Clamp0To1(t);
            return new MzVector(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        }

        // Linearly interpolates between two vectors without clamping the interpolant
        public static MzVector LerpUnclamped(MzVector a, MzVector b, float t) 
        {
            return new MzVector(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t
            );
        }

        // Moves a point /current/ in a straight line towards a /target/ point.
        public static MzVector MoveTowards(MzVector current, MzVector target, float maxDistanceDelta) 
        {
            var toVector = target - current;
            var dist = toVector.Magnitude;
            if (dist <= maxDistanceDelta || dist < float.Epsilon)
                return target;
            return current + toVector / dist * maxDistanceDelta;
        }

        // Gradually changes a vector towards a desired goal over time.
        public static MzVector SmoothDamp(MzVector current, MzVector target, ref MzVector currentVelocity, float smoothTime, float deltaTime, float maxSpeed = Numbers.Infinity) 
        {
            smoothTime = Numbers.Max(0.0001f, smoothTime);
            var omega = 2F / smoothTime;

            var x = omega * deltaTime;
            var exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
            var change = current - target;
            var originalTo = target;

            var maxChange = maxSpeed * smoothTime;
            change = ClampMagnitude(change, maxChange);
            target = current - change;

            var temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            var output = target + (change + temp) * exp;

            if (Dot(originalTo - current, output - originalTo) > 0) {
                output = originalTo;
                currentVelocity = (output - originalTo) / deltaTime;
            }

            return output;
        }
        
        // Returns a copy of /vector/ with its magnitude clamped to /maxLength/.
        public static MzVector ClampMagnitude(MzVector vector, float maxLength) {
            if (vector.MagnitudeSquared > maxLength * maxLength) return vector.Normalized * maxLength;
            return vector;
        }
    } 
}