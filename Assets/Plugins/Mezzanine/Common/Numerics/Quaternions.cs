// See: https://gist.github.com/aeroson/043001ca12fe29ee911e
// Euler reference: https://stackoverflow.com/questions/12088610/conversion-between-euler-mzQuaternion-like-in-unity3d-engine
// Unity Mathematics: https://github.com/Unity-Technologies/Unity.Mathematics/blob/master/src/Unity.Mathematics/mzQuaternion.cs
// XNA Geometry Library: http://www.technologicalutopia.com/sourcecode/xnageometry/mzQuaternion.cs.htm
// System.Numerics: https://github.com/dotnet/corefx/tree/master/src/System.Numerics.Vectors/src/System/Numerics
// SharpDX Math: https://github.com/sharpdx/SharpDX/tree/master/Source/SharpDX.Mathematics

namespace Mz.Numerics 
{
	using NumericBaseType = System.Single;
	
    public static class Quaternions {
        public static MzQuaternion Zero => new MzQuaternion(0, 0, 0, 1);
        public static MzQuaternion Identity => Zero;

        /// <summary>
	    ///   <para>Creates a rotation which rotates from /fromDirection/ to /toDirection/.</para>
	    /// </summary>
	    /// <param name="fromDirection"></param>
	    /// <param name="toDirection"></param>
	    public static MzQuaternion FromToRotation(MzVector fromDirection, MzVector toDirection) {
		    return new MzQuaternion(FromToRotation(fromDirection.Values, toDirection.Values));
	    }
	    public static NumericBaseType[] FromToRotation(NumericBaseType[] fromDirection, NumericBaseType[] toDirection) {
		    return RotateTowards(LookRotation(fromDirection, Vectors.Up.Values), LookRotation(toDirection, Vectors.Up.Values), NumericBaseType.MaxValue);
	    }

	    /// <summary>
	    ///   <para>Creates a rotation with the specified /forward/ and /up/ directions.</para>
	    /// </summary>
	    /// <param name="forward">The direction to look in.</param>
	    /// <param name="up">The mzVector that defines the up direction.</param>
	    public static MzQuaternion LookRotation(MzVector forward, MzVector? up = null) {
		    var upNew = up ?? Vectors.Up;
		    return new MzQuaternion(LookRotation(forward.Values, upNew.Values));
	    }
		public static NumericBaseType[] LookRotation(NumericBaseType[] forward, NumericBaseType[] up) {
		    var upNew = up ?? Vectors.Up.Values;
			return _LookRotation(ref forward, ref upNew);
		}

		// from http://answers.unity3d.com/questions/467614/what-is-the-source-code-of-quaternionlookrotation.html
		private static NumericBaseType[] _LookRotation(ref NumericBaseType[] forward, ref NumericBaseType[] up) {
			Metrics.Normalize(forward, true);
			var right = Metrics.Normalize(Vectors.Cross(up, forward), true);
			up = Vectors.Cross(forward, right);
			var m00 = right[0];
			var m01 = right[1];
			var m02 = right[2];
			var m10 = up[0];
			var m11 = up[1];
			var m12 = up[2];
			var m20 = forward[0];
			var m21 = forward[1];
			var m22 = forward[2];


			var num8 = (m00 + m11) + m22;
			var quaternion = new NumericBaseType[] { 0, 0, 0, 0};
			if (num8 > 0f) {
				var num = Numbers.Sqrt(num8 + 1f);
				quaternion[3] = num * 0.5f;
				num = 0.5f / num;
				quaternion[0] = (m12 - m21) * num;
				quaternion[1] = (m20 - m02) * num;
				quaternion[2] = (m01 - m10) * num;
				return quaternion;
			}
			if ((m00 >= m11) && (m00 >= m22)) {
				var num7 = Numbers.Sqrt(((1f + m00) - m11) - m22);
				var num4 = 0.5f / num7;
				quaternion[0] = 0.5f * num7;
				quaternion[1] = (m01 + m10) * num4;
				quaternion[2] = (m02 + m20) * num4;
				quaternion[3] = (m12 - m21) * num4;
				return quaternion;
			}
			if (m11 > m22) {
				var num6 = Numbers.Sqrt(((1f + m11) - m00) - m22);
				var num3 = 0.5f / num6;
				quaternion[0] = (m10 + m01) * num3;
				quaternion[1] = 0.5f * num6;
				quaternion[2] = (m21 + m12) * num3;
				quaternion[3] = (m20 - m02) * num3;
				return quaternion;
			}
			var num5 = Numbers.Sqrt(((1f + m22) - m00) - m11);
			var num2 = 0.5f / num5;
			quaternion[0] = (m20 + m02) * num2;
			quaternion[1] = (m21 + m12) * num2;
			quaternion[2] = 0.5f * num5;
			quaternion[3] = (m01 - m10) * num2;
			return quaternion;
		}

		/// <summary>
		///   <para>Spherically interpolates between /a/ and /b/ by t. The parameter /t/ is clamped to the range [0, 1].</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static NumericBaseType[] Slerp(NumericBaseType[] a, NumericBaseType[] b, NumericBaseType t) {
			return _Slerp(ref a, ref b, t);
		}
		private static NumericBaseType[] _Slerp(ref NumericBaseType[] a, ref NumericBaseType[] b, NumericBaseType t) {
			if (t > 1) t = 1;
			if (t < 0) t = 0;
			return _SlerpUnclamped(ref a, ref b, t);
		}
		/// <summary>
		///   <para>Spherically interpolates between /a/ and /b/ by t. The parameter /t/ is not clamped.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static NumericBaseType[] SlerpUnclamped(NumericBaseType[] a, NumericBaseType[] b, NumericBaseType t) {
			return _SlerpUnclamped(ref a, ref b, t);
		}
		private static NumericBaseType[] _SlerpUnclamped(ref NumericBaseType[] a, ref NumericBaseType[] b, NumericBaseType t) {
			// if either input is zero, return the other.
			if (Numbers.Abs(Metrics.MagnitudeSquared(a)) < Numbers.Epsilon) {
				return Numbers.Abs(Metrics.MagnitudeSquared(b)) < Numbers.Epsilon ? Identity.Values : b;
			}
			
			if (Numbers.Abs(Metrics.MagnitudeSquared(b)) < Numbers.Epsilon) return a;
			
			var cosHalfAngle = a[3] * b[3] + Metrics.Dot(Vectors.Xyz(a), Vectors.Xyz(b));

			if (cosHalfAngle >= 1f || cosHalfAngle <= -1f) {
				// angle = 0.0f, so just return one input.
				return a;
			}
			
			if (cosHalfAngle < 0f) {
				Metrics.Set(b, Metrics.MultiplyNumber(Vectors.Xyz(b), -1));
				b[3] *= -1;
				cosHalfAngle = -cosHalfAngle;
			}

			NumericBaseType blendA;
			NumericBaseType blendB;
			if (cosHalfAngle < 0.99f) {
				// do proper slerp for big angles
				var halfAngle = Numbers.Acos(cosHalfAngle);
				var sinHalfAngle = Numbers.Sin(halfAngle);
				var oneOverSinHalfAngle = 1f / sinHalfAngle;
				blendA = Numbers.Sin(halfAngle * (1f - t)) * oneOverSinHalfAngle;
				blendB = Numbers.Sin(halfAngle * t) * oneOverSinHalfAngle;
			} else {
				// do lerp if angle is really small.
				blendA = 1f - t;
				blendB = t;
			}

			var xyz = Metrics.Add(Metrics.MultiplyNumber(Vectors.Xyz(a), blendA),Metrics.MultiplyNumber(Vectors.Xyz(b), blendB));
			var result = new NumericBaseType[] { xyz[0], xyz[1], xyz[2], blendA * a[3] + blendB * b[3] };
			return Metrics.MagnitudeSquared(result) > 0f ? Metrics.Normalize(result, true) : Identity.Values;
		}
		/// <summary>
		///   <para>Interpolates between /a/ and /b/ by /t/ and normalizes the result afterwards. The parameter /t/ is clamped to the range [0, 1].</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static NumericBaseType[] Lerp(NumericBaseType[] a, NumericBaseType[] b, NumericBaseType t) {
			if (t > 1) t = 1;
			if (t < 0) t = 0;
			return _Slerp(ref a, ref b, t); // TODO: use lerp not slerp, "Because mzQuaternion works in 4D. Rotation in 4D are linear" ???
		}
		/// <summary>
		///   <para>Interpolates between /a/ and /b/ by /t/ and normalizes the result afterwards. The parameter /t/ is not clamped.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="t"></param>
		public static NumericBaseType[] LerpUnclamped(NumericBaseType[] a, NumericBaseType[] b, NumericBaseType t) {
			return _Slerp(ref a, ref b, t);
		}

		/// <summary>
		///   <para>Rotates a rotation /from/ towards /to/.</para>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="maxDegreesDelta"></param>
		public static NumericBaseType[] RotateTowards(NumericBaseType[] from, NumericBaseType[] to, NumericBaseType maxDegreesDelta) {
			var num = Angle(from, to);
			if (Numbers.Abs(num) < Numbers.Epsilon) return to;
	
			var t = Numbers.Min(1f, maxDegreesDelta / num);
			return SlerpUnclamped(from, to, t);
		}

		/// <summary>
		///   <para>Returns the Inverse of /rotation/.</para>
		/// </summary>
		/// <param name="rotation"></param>
		public static MzQuaternion Inverse(MzQuaternion rotation, bool isModifyOriginal = true) {
			var inversedValues = Inverse(rotation.Values, isModifyOriginal);
			return isModifyOriginal ? rotation : new MzQuaternion(inversedValues);
		}
		public static NumericBaseType[] Inverse(NumericBaseType[] rotation, bool isModifyOriginal = true) {
			var lengthSq = Metrics.MagnitudeSquared(rotation);
			if (Numbers.Abs(lengthSq) < Numbers.Epsilon) return rotation;
			var i = 1.0f / lengthSq;

			var rotationNew = isModifyOriginal ? rotation : (NumericBaseType[])rotation.Clone();
			rotationNew[0] *= -i;
			rotationNew[1] *= -i;
			rotationNew[2] *= -i;
			rotationNew[3] *= i;

			return rotationNew;
		}

		/// <summary>
		///   <para>Returns the angle in degrees between two rotations /a/ and /b/.</para>
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		public static NumericBaseType Angle(NumericBaseType[] a, NumericBaseType[] b) {
	        var f = Metrics.Dot(a, b);
	        return Numbers.Acos(Numbers.Min(Numbers.Abs(f), 1f)) * 2f * Numbers.Rad2Deg;
        }

        /// <summary>
        ///   <para>Returns a mzVector representing the direction of rotation.</para>
        /// </smmary>
		public static MzVector ToEulerDegrees(MzQuaternion mzQuaternion) {
			return new MzVector(ToEulerDegrees(mzQuaternion.Values));
		}
		public static NumericBaseType[] ToEulerDegrees(NumericBaseType[] quaternion) {
			return Metrics.MultiplyNumber(ToEulerRadians(quaternion),Numbers.Rad2Deg);
		}

		public static NumericBaseType[] FromEulerDegrees(NumericBaseType[] vector) {
			return FromEulerRadians(Metrics.MultiplyNumber(vector, Numbers.Deg2Rad));
		}
        
	    /// <summary>
	    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).</para>
	    /// </smmary>
	    /// <param name="x"></param>
	    /// <param name="y"></param>
	    /// <param name="z"></param>
	    public static NumericBaseType[] FromEulerDegrees(NumericBaseType x, NumericBaseType y, NumericBaseType z) {
		    return FromEulerDegrees(new NumericBaseType[] {x, y, z});
	    }
	    
	    /// <summary>
	    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).</para>
	    /// </summary>
	    /// <param name="euler"></param>
	    public static NumericBaseType[] Euler(NumericBaseType[] euler) {
		    return FromEulerRadians(Metrics.MultiplyNumber(euler,Numbers.Deg2Rad));
	    }
	
	    /// <summary>
	    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).</para>
	    /// </summary>
	    /// <param name="x"></param>
	    /// <param name="y"></param>
	    /// <param name="z"></param>
	    public static MzQuaternion Euler(NumericBaseType x, NumericBaseType y, NumericBaseType z) { return Euler(new MzVector(x, y, z)); }
	    
	    /// <summary>
	    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).</para>
	    /// </summary>
	    /// <param name="euler"></param>
	    public static MzQuaternion Euler(MzVector value) {
		    return new MzQuaternion(FromEulerRadians((value * Numbers.Deg2Rad).Values));
	    }
	
	    public static NumericBaseType[] ToQuaternion(NumericBaseType yaw, NumericBaseType pitch, NumericBaseType roll) {
		    yaw *= Numbers.Deg2Rad;
		    pitch *= Numbers.Deg2Rad;
		    roll *= Numbers.Deg2Rad;

		    NumericBaseType yawOver2 = yaw * 0.5f;
		    NumericBaseType cosYawOver2 = Numbers.Cos(yawOver2);
		    NumericBaseType sinYawOver2 = Numbers.Sin(yawOver2);
		    NumericBaseType pitchOver2 = pitch * 0.5f;
		    NumericBaseType cosPitchOver2 = Numbers.Cos(pitchOver2);
		    NumericBaseType sinPitchOver2 = Numbers.Sin(pitchOver2);
		    NumericBaseType rollOver2 = roll * 0.5f;
		    NumericBaseType cosRollOver2 = Numbers.Cos(rollOver2);
		    NumericBaseType sinRollOver2 = Numbers.Sin(rollOver2);            
		    NumericBaseType[] result = new NumericBaseType[4];
		    result[3] = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
		    result[0] = sinYawOver2 * cosPitchOver2 * cosRollOver2 + cosYawOver2 * sinPitchOver2 * sinRollOver2;
		    result[1] = cosYawOver2 * sinPitchOver2 * cosRollOver2 - sinYawOver2 * cosPitchOver2 * sinRollOver2;
		    result[2] = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

		    return result;
	    }

		// from http://stackoverflow.com/questions/12088610/conversion-between-euler-mzQuaternion-like-in-unity3d-engine
		public static NumericBaseType[] ToEulerRadians(NumericBaseType[] rotation) {
			var sqw = rotation[3] * rotation[3];
			var sqx = rotation[0] * rotation[0];
			var sqy = rotation[1] * rotation[1];
			var sqz = rotation[2] * rotation[2];
			var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
			var test = rotation[0] * rotation[3] - rotation[1] * rotation[2];
			var vector = new NumericBaseType[3];

			if (test > 0.4995f * unit) { // singularity at north pole
				vector[1] = 2f * Numbers.Atan2(rotation[1], rotation[0]);
				vector[0] = Numbers.Pi / 2;
				vector[2] = 0;
				return _NormalizeAngles(Metrics.MultiplyNumber(vector, Numbers.Rad2Deg));
			}
			if (test < -0.4995f * unit) { // singularity at south pole
				vector[1] = -2f * Numbers.Atan2(rotation[1], rotation[0]);
				vector[0] = -Numbers.Pi / 2;
				vector[2] = 0;
				return _NormalizeAngles(Metrics.MultiplyNumber(vector, Numbers.Rad2Deg));
			}
			var quaternion = new NumericBaseType[] {rotation[3], rotation[2], rotation[0], rotation[1]};
			vector[1] = Numbers.Atan2(2f * quaternion[0] * quaternion[3] + 2f * quaternion[1] * quaternion[2], 1 - 2f * (quaternion[2] * quaternion[2] + quaternion[3] * quaternion[3])); // Yaw
			vector[0] = Numbers.Asin(2f * (quaternion[0] * quaternion[2] - quaternion[3] * quaternion[1])); // Pitch
			vector[2] = Numbers.Atan2(2f * quaternion[0] * quaternion[1] + 2f * quaternion[2] * quaternion[3], 1 - 2f * (quaternion[1] * quaternion[1] + quaternion[2] * quaternion[2])); // Roll
			return _NormalizeAngles(Metrics.MultiplyNumber(vector,Numbers.Rad2Deg));
		}
		private static NumericBaseType[] _NormalizeAngles(NumericBaseType[] angles) {
			angles[0] = _NormalizeAngle(angles[0]);
			angles[1] = _NormalizeAngle(angles[1]);
			angles[2] = _NormalizeAngle(angles[2]);
			return angles;
		}
		private static NumericBaseType _NormalizeAngle(NumericBaseType angle) {
			while (angle > 360) angle -= 360;
			while (angle < 0) angle += 360;
			return angle;
		}
		// from http://stackoverflow.com/questions/11492299/mzQuaternion-to-euler-angles-algorithm-how-to-convert-to-y-up-and-between-ha
		public static NumericBaseType[] FromEulerRadians(NumericBaseType[] euler) {
			var yaw = euler[0];
			var pitch = euler[1];
			var roll = euler[2];
			var rollOver2 = roll * 0.5f;
			var sinRollOver2 = Numbers.Sin((NumericBaseType)rollOver2);
			var cosRollOver2 = Numbers.Cos((NumericBaseType)rollOver2);
			var pitchOver2 = pitch * 0.5f;
			var sinPitchOver2 = Numbers.Sin((NumericBaseType)pitchOver2);
			var cosPitchOver2 = Numbers.Cos((NumericBaseType)pitchOver2);
			var yawOver2 = yaw * 0.5f;
			var sinYawOver2 = Numbers.Sin((NumericBaseType)yawOver2);
			var cosYawOver2 = Numbers.Cos((NumericBaseType)yawOver2);
			var result = new NumericBaseType[4];
			result[0] = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
			result[1] = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
			result[2] = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
			result[3] = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
			return result;

		}
		private static void _ToAxisAngleRadians(NumericBaseType[] quaternion, out NumericBaseType[] axis, out NumericBaseType angle) {
			if (System.Math.Abs(quaternion[3]) > 1.0f) Metrics.Normalize(quaternion, true);
			angle = 2.0f * Numbers.Acos(quaternion[3]); // angle
			var den = Numbers.Sqrt(1.0f - quaternion[3] * quaternion[3]);
			if (den > Numbers.Epsilon) {
				axis = Metrics.DivideNumber(new NumericBaseType[3] { quaternion[0], quaternion[1], quaternion[2] }, den, true);
			} else {
				// This occurs when the angle is zero. 
				// Not a problem: just set an arbitrary normalized axis.
				axis = new NumericBaseType[3] { 1, 0, 0 };
			}
		}
    }
}