namespace Mz.Numerics
{
    using NumericBaseType = System.Single;

    public static partial class Vectors
    {
        public static NumericBaseType GetAngleOfVector(MzVector vector)
        {
            if (vector.X < 0) return 360 - (Numbers.ToDegrees(Numbers.Atan2(vector.X, vector.Y)) * -1);
            return Numbers.ToDegrees(Numbers.Atan2(vector.X, vector.Y));
        }

        /// <summary>
        /// Return component of vector parallel to a unit basis vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="unitBasis">A unit length basis vector</param>
        /// <returns></returns>
        public static MzVector ParallelComponent(MzVector vector, MzVector unitBasis)
        {
            var projection = Dot(vector, unitBasis);
            return unitBasis * projection;
        }

        /// <summary>
        /// Return component of vector perpendicular to a unit basis vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="unitBasis">A unit length basis vector</param>
        /// <returns></returns>
        public static MzVector PerpendicularComponent(MzVector vector, MzVector unitBasis)
        {
            return vector - ParallelComponent(vector, unitBasis);
        }

        /// <summary>
        /// Method to generate a normalized vector perpendicular to another one using the Hughes-Muller method.
        /// The code in this method is adapted from: http://blog.selfshadow.com/2011/10/17/perp-vectors/
        /// Further reading: Hughes, J. F., Muller, T., "Building an Orthonormal Basis from a Unit MzVector", Journal of Graphics Tools 4:4 (1999), 33-35.
        /// </summary>
        /// <param name="vector">The vector with regard to which we will generate a perpendicular unit vector.</param>
        /// <returns>A normalized vector which is perpendicular to the provided vector argument.</returns>
        public static MzVector Perpendicular(MzVector vector)
        {
            // Get the absolute source vector
            var vectorAbs = Abs(vector, false);

            if (vectorAbs.X <= vectorAbs.Y && vectorAbs.X <= vectorAbs.Z)
            {
                return new MzVector(0, -vector.Z, vector.Y).Normalize();
            }

            if (vectorAbs.Y <= vectorAbs.X && vectorAbs.Y <= vectorAbs.Z)
            {
                return new MzVector(-vector.Z, 0, vector.X).Normalize();
            }

            return new MzVector(-vector.Y, vector.X, 0).Normalize();
        }

        /// <summary>
        /// Clamps the length of a given vector to maxLength.  If the vector is
        /// shorter its value is returned unaltered, if the vector is longer
        /// the value returned has length of maxLength and is parallel to the
        /// original input.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        public static MzVector TruncateLength(MzVector vector, NumericBaseType maxLength)
        {
            var maxLengthSquared = maxLength * maxLength;
            var vecLengthSquared = vector.MagnitudeSquared;
            if (vecLengthSquared <= maxLengthSquared) return vector;
            return (vector * (maxLength / Numbers.Sqrt(vecLengthSquared)));
        }

        public static MzVector Rotate(MzVector v, NumericBaseType angleDegrees)
        {
            var len = v.Magnitude;
            if (len < Numbers.Epsilon) return v;

            var refAngle = Angle(v, new MzVector(1, 0), true);

            var dx = len * Numbers.Cos((refAngle + angleDegrees) * Numbers.Pi / 180.0f);
            var dy = -len * Numbers.Sin((refAngle + angleDegrees) * Numbers.Pi / 180.0f);

            return new MzVector(dx, dy);
        }

        /// <summary>
        /// Wrap a position around so it is always within 1 radius of the sphere (keeps repeating wrapping until position is within sphere).
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static MzVector SphericalWrapAround(MzVector vector, MzVector center, NumericBaseType radius)
        {
            NumericBaseType r;
            do
            {
                var offset = vector - center;
                r = offset.Magnitude;
                if (r > radius) vector = vector + ((offset / r) * radius * -2);
            } while (r > radius);

            return vector;
        }

        /// <summary>
        /// Returns a position randomly distributed on a disk of unit radius
        /// on the XZ (Y=0) plane, centered at the origin. Orientation will be
        /// random and length will range between 0 and 1.
        /// </summary>
        public static MzVector RandomVectorOnUnitRadiusXZDisk()
        {
            var vector = new MzVector(0, 0, 0);
            do
            {
                vector.X = (Numbers.Next() * 2) - 1;
                vector.Y = 0;
                vector.Z = (Numbers.Next() * 2) - 1;
            } while (vector.Magnitude >= 1);

            return vector;
        }

        /// <summary>
        /// Returns a position randomly distributed inside a sphere of unit radius
        /// centered at the origin. Orientation will be random and length will range
        /// between 0 and 1.
        /// </summary>
        public static MzVector RandomVectorInUnitRadiusSphere()
        {
            var vector = new MzVector(0, 0, 0);
            do
            {
                vector.X = (Numbers.Next() * 2) - 1;
                vector.Y = (Numbers.Next() * 2) - 1;
                vector.Z = (Numbers.Next() * 2) - 1;
            } while (vector.Magnitude >= 1);

            return vector;
        }

        /// <summary>
        /// Returns a position randomly distributed on the surface of a sphere
        /// of unit radius centered at the origin. Orientation will be random
        /// and length will be 1.
        /// </summary>
        public static MzVector RandomUnitVector()
        {
            return RandomVectorInUnitRadiusSphere().Normalize(true);
        }

        /// <summary>
        /// Returns a position randomly distributed on a circle of unit radius
        /// on the XZ (Y=0) plane, centered at the origin. Orientation will be
        /// random and length will be 1.
        /// </summary>
        public static MzVector RandomUnitVectorOnXZPlane()
        {
            var vector = RandomVectorInUnitRadiusSphere();
            vector.Y = 0;
            vector.Normalize(true);
            return vector;
        }

        /// <summary>
        /// Clip a vector to be within the given cone.
        /// </summary>
        /// <param name="source">A vector to clip</param>
        /// <param name="cosineOfConeAngle">The cosine of the cone angle</param>
        /// <param name="basis">The vector along the middle of the cone</param>
        public static MzVector LimitMaxDeviationAngle(
            MzVector source,
            NumericBaseType cosineOfConeAngle,
            MzVector basis
        )
        {
            return _LimitDeviationAngleUtility(true, /* force source INSIDE cone */ source, cosineOfConeAngle, basis);
        }

        /// <summary>
        /// Clip a vector to be outside the given cone.
        /// </summary>
        /// <param name="source">A vector to clip</param>
        /// <param name="cosineOfConeAngle">The cosine of the cone angle</param>
        /// <param name="basis">The vector along the middle of the cone</param>
        public static MzVector LimitMinDeviationAngle(
            MzVector source, 
            NumericBaseType cosineOfConeAngle,
            MzVector basis
        )
        {
            return _LimitDeviationAngleUtility(false, /* force source OUTSIDE cone */ source, cosineOfConeAngle, basis);
        }

        /// <summary>
        /// Used by LimitMaxDeviationAngle / LimitMinDeviationAngle.
        /// </summary>
        private static MzVector _LimitDeviationAngleUtility(
            bool insideOrOutside, 
            MzVector source,
            NumericBaseType cosineOfConeAngle, 
            MzVector basis
        )
        {
            // immediately return zero length input vectors
            var sourceLength = source.Magnitude;
            if (sourceLength < NumericBaseType.Epsilon) return source;

            // measure the angular diviation of "source" from "basis"
            var direction = source / sourceLength;

            var cosineOfSourceAngle = Dot(direction, basis);

            // Simply return "source" if it already meets the angle criteria.
            // (note: we hope this top "if" gets compiled out since the flag
            // is a constant when the function is inlined into its caller)
            if (insideOrOutside)
            {
                // source vector is already inside the cone, just return it
                if (cosineOfSourceAngle >= cosineOfConeAngle) return source;
            }
            else if (cosineOfSourceAngle <= cosineOfConeAngle) return source;

            // find the portion of "source" that is perpendicular to "basis"
            var perpendicular = PerpendicularComponent(source, basis);
            if (perpendicular == Zero) return Zero;

            // normalize that perpendicular
            var unitPerp = perpendicular.Normalize(false);

            // construct a new vector whose length equals the source vector,
            // and lies on the intersection of a plane (formed the source and
            // basis vectors) and a cone (whose axis is "basis" and whose
            // angle corresponds to cosineOfConeAngle)
            var perpDist = Numbers.Sqrt(1 - (cosineOfConeAngle * cosineOfConeAngle));
            var c0 = basis * cosineOfConeAngle;
            var c1 = unitPerp * perpDist;
            return (c0 + c1) * sourceLength;
        }

        /// <summary>
        /// Returns the distance between a point and a line.
        /// </summary>
        /// <param name="point">The point to measure distance to</param>
        /// <param name="lineOrigin">A point on the line</param>
        /// <param name="lineUnitTangent">A UNIT vector parallel to the line</param>
        public static NumericBaseType DistanceFromLine(MzVector point, MzVector lineOrigin, MzVector lineUnitTangent)
        {
            var offset = point - lineOrigin;
            var perpendicular = PerpendicularComponent(offset, lineUnitTangent);
            return perpendicular.Magnitude;
        }

        /// <summary>
        /// Find any arbitrary vector which is perpendicular to the given vector.
        /// </summary>
        public static MzVector FindPerpendicularIn3d(MzVector direction)
        {
            // to be filled in:
            MzVector quasiPerp; // a direction which is "almost perpendicular"
            MzVector result; // the computed perpendicular to be returned

            // three mutually perpendicular basis vectors
            var i = UnitX;
            var j = UnitY;
            var k = UnitZ;

            // measure the projection of "direction" onto each of the axes
            var id = Dot(i, direction);
            var jd = Dot(j, direction);
            var kd = Dot(k, direction);

            // set quasiPerp to the basis which is least parallel to "direction"
            if ((id <= jd) && (id <= kd)) quasiPerp = i; // projection onto i was the smallest
            else if ((jd <= id) && (jd <= kd)) quasiPerp = j; // projection onto j was the smallest
            else quasiPerp = k; // projection onto k was the smallest

            // return the cross product (direction x quasiPerp)
            // which is guaranteed to be perpendicular to both of them
            result = Cross(direction, quasiPerp);

            return result;
        }
    }
}