using System;

namespace Mz.Numerics
{
    /// <summary>
    /// A representation of a direction in two dimensional space.
    /// </summary>
    public struct DirectionAngle2D : IEquatable<DirectionAngle2D>, IFormattable
    {
        private DirectionAngle2D(uint data)
        {
            _data = data;
        }

        /// <summary>
        /// Initializes a direction from an absolute angle value in radians.
        /// </summary>
        public static DirectionAngle2D FromAngleRadians(float radians)
        {
            return new DirectionAngle2D((uint) (radians * _fromRadians));
        }

        /// <summary>
        /// Initializes a direction from an absolute angle value in degrees.
        /// </summary>
        public static DirectionAngle2D FromAngleDegrees(float degrees)
        {
            return new DirectionAngle2D((uint) (degrees * _fromDegrees));
        }

        /// <summary>
        /// Initializes a direction along a mzVector.
        /// </summary>
        public static DirectionAngle2D Of(MzVector mzVector)
        {
            return FromAngleRadians((float) Numbers.Atan2(mzVector.Y, mzVector.X));
        }

        /// <summary>
        /// Initializes the direction between two points.
        /// </summary>
        /// <param name="from">The base point.</param>
        /// <param name="to">The point the directions "points" towards.</param>
        public static DirectionAngle2D Between(MzVector from, MzVector to)
        {
            return Of(to - from);
        }

        //===== Static Fields

        private const float _fromRadians = uint.MaxValue / (float) (Numbers.Pi * 2.0);
        private const float _toRadians = (float) (Numbers.Pi * 2.0) / uint.MaxValue;

        private const float _fromDegrees = uint.MaxValue / 360f;
        private const float _toDegrees = 360f / uint.MaxValue;

        private readonly uint _data;

        /// <summary>
        /// Default base direction (along positive X axis).
        /// </summary>
        public static readonly DirectionAngle2D Zero = new DirectionAngle2D(0);

        //===== Properties

        /// <summary>
        /// Gets the absolute angle of the direction in radians between 0 and 2pi.
        /// </summary>
        public float Radians => _data * _toRadians;

        /// <summary>
        /// Gets the absolute angle of the direction in degrees between 0 and 360.
        /// </summary>
        public float Degrees => _data * _toDegrees;

        /// <summary>
        /// Gets the absolute angle of the direction in radians between -pi and pi.
        /// </summary>
        public float RadiansSigned => (int) _data * _toRadians;

        /// <summary>
        /// Gets the absolute angle of the direction in degrees between -180 and 180.
        /// </summary>
        public float DegreesSigned => (int) _data * _toDegrees;

        /// <summary>
        /// Gets the unit vector pointing in this direction.
        /// </summary>
        public MzVector Vector
        {
            get
            {
                var radians = Radians;
                return new MzVector((float) System.Math.Cos(radians), (float) System.Math.Sin(radians));
            }
        }

        //===== Methods

        /// <summary>
        /// Returns this direction turned towards a target direction with a given maximum step length in radians.
        /// This will never overshoot the goal.
        /// </summary>
        /// <param name="target">The goal direction.</param>
        /// <param name="maxStepInRadians">The maximum step length in radians. Negative values will return the original direction.</param>
        public DirectionAngle2D TurnedTowards(DirectionAngle2D target, float maxStepInRadians)
        {
            if (maxStepInRadians <= 0) return this;
            var step = maxStepInRadians;
            var thisToGoal = target - this;
            if (step > Math.Abs(thisToGoal)) return target;
            step *= Math.Sign(thisToGoal);
            return this + step;
        }

        //===== Statics

        /// <summary>
        /// Linearly interpolates between two directions.
        /// This always interpolates along the shorter arc.
        /// </summary>
        /// <param name="d0">The first direction (at p == 0).</param>
        /// <param name="d1">The second direction (at p == 1).</param>
        /// <param name="p">The parameter.</param>
        public static DirectionAngle2D Lerp(DirectionAngle2D d0, DirectionAngle2D d1, float p)
        {
            return d0 + p * (d1 - d0);
        }

        //===== Operators

        //----- Arithmetic

        /// <summary>
        /// Adds an angle to a direction.
        /// </summary>
        public static DirectionAngle2D operator +(DirectionAngle2D directionAngle, float angleDegrees)
        {
            return new DirectionAngle2D((uint) (directionAngle._data + (angleDegrees * (float) (Math.PI / 180)) * _fromRadians));
        }

        /// <summary>
        /// Substracts an angle from a direction.
        /// </summary>
        public static DirectionAngle2D operator -(DirectionAngle2D directionAngle, float angleDegrees)
        {
            return new DirectionAngle2D((uint) (directionAngle._data - (angleDegrees * (float) (Math.PI / 180)) * _fromRadians));
        }

        /// <summary>
        /// Gets the signed difference between two directions.
        /// Always returns the angle of the shorter arc in degrees.
        /// </summary>
        public static float operator -(DirectionAngle2D direction1, DirectionAngle2D direction2)
        {
            var angleRadians = ((int) direction1._data - (int) direction2._data) * _toRadians;
            return angleRadians * (float) (180 / Math.PI);
        }

        /// <summary>
        /// Gets the inverse direction to a direction.
        /// </summary>
        public static DirectionAngle2D operator -(DirectionAngle2D directionAngle)
        {
            return new DirectionAngle2D(directionAngle._data + (uint.MaxValue / 2 + 1));
        }

        //----- Boolean

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(DirectionAngle2D other)
        {
            return _data == other._data;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is DirectionAngle2D && Equals((DirectionAngle2D) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and _data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        /// <summary>
        /// Checks two directions for equality.
        /// </summary>
        public static bool operator ==(DirectionAngle2D x, DirectionAngle2D y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Checks two directions for inequality.
        /// </summary>
        public static bool operator !=(DirectionAngle2D x, DirectionAngle2D y)
        {
            return !(x == y);
        }

        public string ToString(string format, IFormatProvider formatProvider)
            => $"{Radians.ToString(format, formatProvider)} rad";
    }
}