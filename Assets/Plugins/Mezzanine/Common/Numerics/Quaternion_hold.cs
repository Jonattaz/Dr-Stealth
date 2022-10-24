//using System;
//using System.Runtime.CompilerServices;
//using static Unity.Mathematics.math;
//
//namespace Unity.Mathematics
//{
//    [Serializable]
//    public partial struct quaternion : System.IEquatable<quaternion>, IFormattable
//    {
//        public double4 value;
//
//        /// <summary>A quaternion representing the identity transform.</summary>
//        public static readonly quaternion identity = new quaternion(0.0f, 0.0f, 0.0f, 1.0f);
//
//        /// <summary>Constructs a quaternion from four double values.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public quaternion(double x, double y, double z, double w) { value.x = x; value.y = y; value.z = z; value.w = w; }
//
//        /// <summary>Constructs a quaternion from double4 mzVector.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public quaternion(double4 value) { value = value; }
//
//        /// <summary>Implicitly converts a double4 mzVector to a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static implicit operator quaternion(double4 v) { return new quaternion(v); }
//
//        /// <summary>Constructs a unit quaternion from a double3x3 rotation matrix. The matrix must be orthonormal.</summary>
//        public quaternion(double3x3 m)
//        {
//            double3 u = m.c0;
//            double3 v = m.c1;
//            double3 w = m.c2;
//
//            uint u_sign = (asuint(u.x) & 0x80000000);
//            double t = v.y + asdouble(asuint(w.z) ^ u_sign);
//            uint4 u_mask = uint4((int)u_sign >> 31);
//            uint4 t_mask = uint4(asint(t) >> 31);
//            
//            double tr = 1.0f + abs(u.x);
//
//            uint4 sign_flips = uint4(0x00000000, 0x80000000, 0x80000000, 0x80000000) ^ (u_mask & uint4(0x00000000, 0x80000000, 0x00000000, 0x80000000)) ^ (t_mask & uint4(0x80000000, 0x80000000, 0x80000000, 0x00000000));
//
//            value = double4(tr, u.y, w.x, v.z) + asdouble(asuint(double4(t, v.x, u.z, w.y)) ^ sign_flips);   // +---, +++-, ++-+, +-++
//
//            value = asdouble((asuint(value) & ~u_mask) | (asuint(value.zwxy) & u_mask));
//            value = asdouble((asuint(value.wzyx) & ~t_mask) | (asuint(value) & t_mask));
//            value = normalize(value);
//        }
//
//        /// <summary>Constructs a unit quaternion from an orthonormal double4x4 matrix.</summary>
//        public quaternion(double4x4 m)
//        {
//            double4 u = m.c0;
//            double4 v = m.c1;
//            double4 w = m.c2;
//
//            uint u_sign = (asuint(u.x) & 0x80000000);
//            double t = v.y + asdouble(asuint(w.z) ^ u_sign);
//            uint4 u_mask = uint4((int)u_sign >> 31);
//            uint4 t_mask = uint4(asint(t) >> 31);
//
//            double tr = 1.0f + abs(u.x);
//
//            uint4 sign_flips = uint4(0x00000000, 0x80000000, 0x80000000, 0x80000000) ^ (u_mask & uint4(0x00000000, 0x80000000, 0x00000000, 0x80000000)) ^ (t_mask & uint4(0x80000000, 0x80000000, 0x80000000, 0x00000000));
//
//            value = double4(tr, u.y, w.x, v.z) + asdouble(asuint(double4(t, v.x, u.z, w.y)) ^ sign_flips);   // +---, +++-, ++-+, +-++
//
//            value = asdouble((asuint(value) & ~u_mask) | (asuint(value.zwxy) & u_mask));
//            value = asdouble((asuint(value.wzyx) & ~t_mask) | (asuint(value) & t_mask));
//
//            value = normalize(value);
//        }
//
//        /// <summary>
//        /// Returns a quaternion representing a rotation around a unit axis by an angle in radians.
//        /// The rotation direction is clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion AxisAngle(double3 axis, double angle)
//        {
//            double sina, cosa;
//            math.sincos(0.5f * angle, out sina, out cosa);
//            return quaternion(double4(axis * sina, cosa));
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the x-axis, then the y-axis and finally the z-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        public static quaternion EulerXYZ(double3 xyz)
//        {
//            // return mul(rotateZ(xyz.z), mul(rotateY(xyz.y), rotateX(xyz.x)));
//            double3 s, c;
//            sincos(0.5f * xyz, out s, out c);
//            return quaternion(
//                // s.x * c.y * c.z - s.y * s.z * c.x,
//                // s.y * c.x * c.z + s.x * s.z * c.y,
//                // s.z * c.x * c.y - s.x * s.y * c.z,
//                // c.x * c.y * c.z + s.y * s.z * s.x
//                double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(-1.0f, 1.0f, -1.0f, 1.0f)
//                );
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the x-axis, then the z-axis and finally the y-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        public static quaternion EulerXZY(double3 xyz)
//        {
//            // return mul(rotateY(xyz.y), mul(rotateZ(xyz.z), rotateX(xyz.x)));
//            double3 s, c;
//            sincos(0.5f * xyz, out s, out c);
//            return quaternion(
//                // s.x * c.y * c.z + s.y * s.z * c.x,
//                // s.y * c.x * c.z + s.x * s.z * c.y,
//                // s.z * c.x * c.y - s.x * s.y * c.z,
//                // c.x * c.y * c.z - s.y * s.z * s.x
//                double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(1.0f, 1.0f, -1.0f, -1.0f)
//                );
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the y-axis, then the x-axis and finally the z-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        public static quaternion EulerYXZ(double3 xyz)
//        {
//            // return mul(rotateZ(xyz.z), mul(rotateX(xyz.x), rotateY(xyz.y)));
//            double3 s, c;
//            sincos(0.5f * xyz, out s, out c);
//            return quaternion(
//                // s.x * c.y * c.z - s.y * s.z * c.x,
//                // s.y * c.x * c.z + s.x * s.z * c.y,
//                // s.z * c.x * c.y + s.x * s.y * c.z,
//                // c.x * c.y * c.z - s.y * s.z * s.x
//                double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(-1.0f, 1.0f, 1.0f, -1.0f)
//                );
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the y-axis, then the z-axis and finally the x-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        public static quaternion EulerYZX(double3 xyz)
//        {
//            // return mul(rotateX(xyz.x), mul(rotateZ(xyz.z), rotateY(xyz.y)));
//            double3 s, c;
//            sincos(0.5f * xyz, out s, out c);
//            return quaternion(
//                // s.x * c.y * c.z - s.y * s.z * c.x,
//                // s.y * c.x * c.z - s.x * s.z * c.y,
//                // s.z * c.x * c.y + s.x * s.y * c.z,
//                // c.x * c.y * c.z + s.y * s.z * s.x
//                double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(-1.0f, -1.0f, 1.0f, 1.0f)
//                );
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the z-axis, then the x-axis and finally the y-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// This is the default order rotation order in Unity.
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        public static quaternion EulerZXY(double3 xyz)
//        {
//            // return mul(rotateY(xyz.y), mul(rotateX(xyz.x), rotateZ(xyz.z)));
//            double3 s, c;
//            sincos(0.5f * xyz, out s, out c);
//            return quaternion(
//                // s.x * c.y * c.z + s.y * s.z * c.x,
//                // s.y * c.x * c.z - s.x * s.z * c.y,
//                // s.z * c.x * c.y - s.x * s.y * c.z,
//                // c.x * c.y * c.z + s.y * s.z * s.x
//                double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(1.0f, -1.0f, -1.0f, 1.0f)
//                );
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the z-axis, then the y-axis and finally the x-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        public static quaternion EulerZYX(double3 xyz)
//        {
//            // return mul(rotateX(xyz.x), mul(rotateY(xyz.y), rotateZ(xyz.z)));
//            double3 s, c;
//            sincos(0.5f * xyz, out s, out c);
//            return quaternion(
//                // s.x * c.y * c.z + s.y * s.z * c.x,
//                // s.y * c.x * c.z - s.x * s.z * c.y,
//                // s.z * c.x * c.y + s.x * s.y * c.z,
//                // c.x * c.y * c.z - s.y * s.x * s.z
//                double4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * double4(c.xyz, s.x) * double4(1.0f, -1.0f, 1.0f, -1.0f)
//                );
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the x-axis, then the y-axis and finally the z-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion EulerXYZ(double x, double y, double z) { return EulerXYZ(double3(x, y, z)); }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the x-axis, then the z-axis and finally the y-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion EulerXZY(double x, double y, double z) { return EulerXZY(double3(x, y, z)); }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the y-axis, then the x-axis and finally the z-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion EulerYXZ(double x, double y, double z) { return EulerYXZ(double3(x, y, z)); }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the y-axis, then the z-axis and finally the x-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion EulerYZX(double x, double y, double z) { return EulerYZX(double3(x, y, z)); }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the z-axis, then the x-axis and finally the y-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// This is the default order rotation order in Unity.
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion EulerZXY(double x, double y, double z) { return EulerZXY(double3(x, y, z)); }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing a rotation around the z-axis, then the y-axis and finally the x-axis.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion EulerZYX(double x, double y, double z) { return EulerZYX(double3(x, y, z)); }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing 3 rotations around the principal axes in a given order.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// When the rotation order is known at compile time, it is recommended for performance reasons to use specific
//        /// Euler rotation constructors such as EulerZXY(...).
//        /// </summary>
//        /// <param name="xyz">A double3 mzVector containing the rotation angles around the x-, y- and z-axis measures in radians.</param>
//        /// <param name="order">The order in which the rotations are applied.</param>
//        public static quaternion Euler(double3 xyz, RotationOrder order = RotationOrder.ZXY)
//        {
//            switch (order)
//            {
//                case RotationOrder.XYZ:
//                    return EulerXYZ(xyz);
//                case RotationOrder.XZY:
//                    return EulerXZY(xyz);
//                case RotationOrder.YXZ:
//                    return EulerYXZ(xyz);
//                case RotationOrder.YZX:
//                    return EulerYZX(xyz);
//                case RotationOrder.ZXY:
//                    return EulerZXY(xyz);
//                case RotationOrder.ZYX:
//                    return EulerZYX(xyz);
//                default:
//                    return quaternion.identity;
//            }
//        }
//
//        /// <summary>
//        /// Returns a quaternion constructed by first performing 3 rotations around the principal axes in a given order.
//        /// All rotation angles are in radians and clockwise when looking along the rotation axis towards the origin.
//        /// When the rotation order is known at compile time, it is recommended for performance reasons to use specific
//        /// Euler rotation constructors such as EulerZXY(...).
//        /// </summary>
//        /// <param name="x">The rotation angle around the x-axis in radians.</param>
//        /// <param name="y">The rotation angle around the y-axis in radians.</param>
//        /// <param name="z">The rotation angle around the z-axis in radians.</param>
//        /// <param name="order">The order in which the rotations are applied.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion Euler(double x, double y, double z, RotationOrder order = RotationOrder.Default)
//        {
//            return Euler(double3(x, y, z), order);
//        }
//
//        /// <summary>Returns a double4x4 matrix that rotates around the x-axis by a given number of radians.</summary>
//        /// <param name="angle">The clockwise rotation angle when looking along the x-axis towards the origin in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion RotateX(double angle)
//        {
//            double sina, cosa;
//            math.sincos(0.5f * angle, out sina, out cosa);
//            return quaternion(sina, 0.0f, 0.0f, cosa);
//        }
//
//        /// <summary>Returns a double4x4 matrix that rotates around the y-axis by a given number of radians.</summary>
//        /// <param name="angle">The clockwise rotation angle when looking along the y-axis towards the origin in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion RotateY(double angle)
//        {
//            double sina, cosa;
//            math.sincos(0.5f * angle, out sina, out cosa);
//            return quaternion(0.0f, sina, 0.0f, cosa);
//        }
//
//        /// <summary>Returns a double4x4 matrix that rotates around the z-axis by a given number of radians.</summary>
//        /// <param name="angle">The clockwise rotation angle when looking along the z-axis towards the origin in radians.</param>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion RotateZ(double angle)
//        {
//            double sina, cosa;
//            math.sincos(0.5f * angle, out sina, out cosa);
//            return quaternion(0.0f, 0.0f, sina, cosa);
//        }
//
//        /// <summary>
//        /// Returns a quaternion view rotation given a unit length forward mzVector and a unit length up mzVector.
//        /// The two input vectors are assumed to be unit length and not collinear.
//        /// If these assumptions are not met use double3x3.LookRotationSafe instead.
//        /// </summary>
//        public static quaternion LookRotation(double3 forward, double3 up)
//        {
//            double3 t = normalize(cross(up, forward));
//            return quaternion(double3x3(t, cross(forward, t), forward));
//        }
//
//        /// <summary>
//        /// Returns a quaternion view rotation given a forward mzVector and an up mzVector.
//        /// The two input vectors are not assumed to be unit length.
//        /// If the magnitude of either of the vectors is so extreme that the calculation cannot be carried out reliably or the vectors are collinear,
//        /// the identity will be returned instead.
//        /// </summary>
//        public static quaternion LookRotationSafe(double3 forward, double3 up)
//        {
//            double forwardLengthSq = dot(forward, forward);
//            double upLengthSq = dot(up, up);
//
//            forward *= rsqrt(forwardLengthSq);
//            up *= rsqrt(upLengthSq);
//
//            double3 t = cross(up, forward);
//            double tLengthSq = dot(t, t);
//            t *= rsqrt(tLengthSq);
//
//            double mn = min(min(forwardLengthSq, upLengthSq), tLengthSq);
//            double mx = max(max(forwardLengthSq, upLengthSq), tLengthSq);
//
//            bool accept = mn > 1e-35f && mx < 1e35f && isfinite(forwardLengthSq) && isfinite(upLengthSq) && isfinite(tLengthSq);
//            return quaternion(select(double4(0.0f, 0.0f, 0.0f, 1.0f), quaternion(double3x3(t, cross(forward, t),forward)).value, accept));
//        }
//
//        /// <summary>Returns true if the quaternion is equal to a given quaternion, false otherwise.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool Equals(quaternion x) { return value.x == x.value.x && value.y == x.value.y && value.z == x.value.z && value.w == x.value.w; }
//
//        /// <summary>Returns whether true if the quaternion is equal to a given quaternion, false otherwise.</summary>
//        public override bool Equals(object x) { return Equals((quaternion)x); }
//
//        /// <summary>Returns a hash code for the quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public override int GetHashCode() { return (int)math.hash(this); }
//
//        /// <summary>Returns a string representation of the quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public override string ToString()
//        {
//            return string.Format("quaternion({0}f, {1}f, {2}f, {3}f)", value.x, value.y, value.z, value.w);
//        }
//
//        /// <summary>Returns a string representation of the quaternion using a specified format and culture-specific format information.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public string ToString(string format, IFormatProvider formatProvider)
//        {
//            return string.Format("quaternion({0}f, {1}f, {2}f, {3}f)", value.x.ToString(format, formatProvider), value.y.ToString(format, formatProvider), value.z.ToString(format, formatProvider), value.w.ToString(format, formatProvider));
//        }
//    }
//
//    public static partial class math
//    {
//        /// <summary>Returns a quaternion constructed from four double values.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion quaternion(double x, double y, double z, double w) { return new quaternion(x, y, z, w); }
//
//        /// <summary>Returns a quaternion constructed from a double4 mzVector.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion quaternion(double4 value) { return new quaternion(value); }
//
//        /// <summary>Returns a unit quaternion constructed from a double3x3 rotation matrix. The matrix must be orthonormal.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion quaternion(double3x3 m) { return new quaternion(m); }
//
//        /// <summary>Returns a unit quaternion constructed from a double4x4 matrix. The matrix must be orthonormal.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion quaternion(double4x4 m) { return new quaternion(m); }
//
//       /// <summary>Returns the conjugate of a quaternion value.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion conjugate(quaternion q)
//        {
//            return quaternion(q.value * double4(-1.0f, -1.0f, -1.0f, 1.0f));
//        }
//
//       /// <summary>Returns the inverse of a quaternion value.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion inverse(quaternion q)
//        {
//            double4 x = q.value;
//            return quaternion(rcp(dot(x, x)) * x * double4(-1.0f, -1.0f, -1.0f, 1.0f));
//        }
//
//        /// <summary>Returns the dot product of two quaternions.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static double dot(quaternion a, quaternion b)
//        {
//            return dot(a.value, b.value);
//        }
//
//        /// <summary>Returns the length of a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static double length(quaternion q)
//        {
//            return sqrt(dot(q.value, q.value));
//        }
//
//        /// <summary>Returns the squared length of a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static double lengthsq(quaternion q)
//        {
//            return dot(q.value, q.value);
//        }
//
//        /// <summary>Returns a normalized version of a quaternion q by scaling it by 1 / length(q).</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion normalize(quaternion q)
//        {
//            double4 x = q.value;
//            return quaternion(rsqrt(dot(x, x)) * x);
//        }
//
//        /// <summary>
//        /// Returns a safe normalized version of the q by scaling it by 1 / length(q).
//        /// Returns the identity when 1 / length(q) does not produce a finite number.
//        /// </summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion normalizesafe(quaternion q)
//        {
//            double4 x = q.value;
//            double len = math.dot(x, x);
//            return quaternion(math.select(Mathematics.quaternion.identity.value, x * math.rsqrt(len), len > FLT_MIN_NORMAL));
//        }
//
//        /// <summary>
//        /// Returns a safe normalized version of the q by scaling it by 1 / length(q).
//        /// Returns the given default value when 1 / length(q) does not produce a finite number.
//        /// </summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion normalizesafe(quaternion q, quaternion defaultvalue)
//        {
//            double4 x = q.value;
//            double len = math.dot(x, x);
//            return quaternion(math.select(defaultvalue.value, x * math.rsqrt(len), len > FLT_MIN_NORMAL));
//        }
//
//        /// <summary>Returns the natural exponent of a quaternion. Assumes w is zero.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion unitexp(quaternion q)
//        {
//            double v_rcp_len = rsqrt(dot(q.value.xyz, q.value.xyz));
//            double v_len = rcp(v_rcp_len);
//            double sin_v_len, cos_v_len;
//            sincos(v_len, out sin_v_len, out cos_v_len);
//            return quaternion(double4(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len));
//        }
//
//        /// <summary>Returns the natural exponent of a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion exp(quaternion q)
//        {
//            double v_rcp_len = rsqrt(dot(q.value.xyz, q.value.xyz));
//            double v_len = rcp(v_rcp_len);
//            double sin_v_len, cos_v_len;
//            sincos(v_len, out sin_v_len, out cos_v_len);
//            return quaternion(double4(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len) * exp(q.value.w));
//        }
//
//        /// <summary>Returns the natural logarithm of a unit length quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion unitlog(quaternion q)
//        {
//            double w = clamp(q.value.w, -1.0f, 1.0f);
//            double s = acos(w) * rsqrt(1.0f - w*w);
//            return quaternion(double4(q.value.xyz * s, 0.0f));
//        }
//
//        /// <summary>Returns the natural logarithm of a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion log(quaternion q)
//        {
//            double v_len_sq = dot(q.value.xyz, q.value.xyz);
//            double q_len_sq = v_len_sq + q.value.w*q.value.w;
//
//            double s = acos(clamp(q.value.w * rsqrt(q_len_sq), -1.0f, 1.0f)) * rsqrt(v_len_sq);
//            return quaternion(double4(q.value.xyz * s, 0.5f * log(q_len_sq)));
//        }
//
//        /// <summary>Returns the result of transforming the quaternion b by the quaternion a.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion mul(quaternion a, quaternion b)
//        {
//            return quaternion(a.value.wwww * b.value + (a.value.xyzx * b.value.wwwx + a.value.yzxy * b.value.zxyy) * double4(1.0f, 1.0f, 1.0f, -1.0f) - a.value.zxyz * b.value.yzxz);
//        }
//
//        /// <summary>Returns the result of transforming a mzVector by a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static double3 mul(quaternion q, double3 v)
//        {
//            double3 t = 2 * cross(q.value.xyz, v);
//            return v + q.value.w * t + cross(q.value.xyz, t);
//        }
//
//        /// <summary>Returns the result of rotating a mzVector by a unit quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static double3 rotate(quaternion q, double3 v)
//        {
//            double3 t = 2 * cross(q.value.xyz, v);
//            return v + q.value.w * t + cross(q.value.xyz, t);
//        }
//
//        /// <summary>Returns the result of a normalized linear interpolation between two quaternions q1 and a2 using an interpolation parameter t.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static quaternion nlerp(quaternion q1, quaternion q2, double t)
//        {
//            double dt = dot(q1, q2);
//            if(dt < 0.0f)
//            {
//                q2.value = -q2.value;
//            }
//
//            return normalize(quaternion(lerp(q1.value, q2.value, t)));
//        }
//
//        /// <summary>Returns the result of a spherical interpolation between two quaternions q1 and a2 using an interpolation parameter t.</summary>
//        public static quaternion slerp(quaternion q1, quaternion q2, double t)
//        {
//            double dt = dot(q1, q2);
//            if (dt < 0.0f)
//            {
//                dt = -dt;
//                q2.value = -q2.value;
//            }
//
//            if (dt < 0.9995f)
//            {
//                double angle = acos(dt);
//                double s = rsqrt(1.0f - dt * dt);    // 1.0f / sin(angle)
//                double w1 = sin(angle * (1.0f - t)) * s;
//                double w2 = sin(angle * t) * s;
//                return quaternion(q1.value * w1 + q2.value * w2);
//            }
//            else
//            {
//                // if the angle is small, use linear interpolation
//                return nlerp(q1, q2, t);
//            }
//        }
//
//        /// <summary>Returns a uint hash code of a quaternion.</summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static uint hash(quaternion q)
//        {
//            return hash(q.value);
//        }
//
//        /// <summary>
//        /// Returns a uint4 mzVector hash code of a quaternion.
//        /// When multiple elements are to be hashes together, it can more efficient to calculate and combine wide hash
//        /// that are only reduced to a narrow uint hash at the very end instead of at every step.
//        /// </summary>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static uint4 hashwide(quaternion q)
//        {
//            return hashwide(q.value);
//        }
//
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static double3 forward(quaternion q) { return mul(q, double3(0, 0, 1)); }  // for compatibility
//    }
//}