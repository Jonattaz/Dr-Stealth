using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mz.Numerics
{
    /// <summary>
    /// Extension methods for rectangular tile maps.
    /// </summary>
    public static class DirectionTools
    {
        //===== Lookup Tables

        public static readonly DirectionStep[] DirectionDeltas =
        {
            new DirectionStep(0, 0, 0),
            new DirectionStep(1, 0, 0),
            new DirectionStep(1, 1, 0),
            new DirectionStep(0, 1, 0),
            new DirectionStep(-1, 1, 0),
            new DirectionStep(-1, 0, 0),
            new DirectionStep(-1, -1, 0),
            new DirectionStep(0, -1, 0),
            new DirectionStep(1, -1, 0),

            new DirectionStep(0, 0, 1),
            new DirectionStep(1, 0, 1),
            new DirectionStep(1, 1, 1),
            new DirectionStep(0, 1, 1),
            new DirectionStep(-1, 1, 1),
            new DirectionStep(-1, 0, 1),
            new DirectionStep(-1, -1, 1),
            new DirectionStep(0, -1, 1),
            new DirectionStep(1, -1, 1),

            new DirectionStep(0, 0, -1),
            new DirectionStep(1, 0, -1),
            new DirectionStep(1, 1, -1),
            new DirectionStep(0, 1, -1),
            new DirectionStep(-1, 1, -1),
            new DirectionStep(-1, 0, -1),
            new DirectionStep(-1, -1, -1),
            new DirectionStep(0, -1, -1),
            new DirectionStep(1, -1, -1),
        };

        private static readonly Direction[] _directionOpposite =
        {
            Direction.None,
            Direction.Left,
            Direction.DownLeft,
            Direction.Down,
            Direction.DownRight,
            Direction.Right,
            Direction.UpRight,
            Direction.Up,
            Direction.UpLeft,

            Direction.Backward,
            Direction.BackwardLeft,
            Direction.BackwardDownLeft,
            Direction.BackwardDown,
            Direction.BackwardDownRight,
            Direction.BackwardRight,
            Direction.BackwardUpRight,
            Direction.BackwardUp,
            Direction.BackwardUpLeft,

            Direction.Forward,
            Direction.ForwardLeft,
            Direction.ForwardDownLeft,
            Direction.ForwardDown,
            Direction.ForwardDownRight,
            Direction.ForwardRight,
            Direction.ForwardUpRight,
            Direction.ForwardUp,
            Direction.ForwardUpLeft,
        };

        private static readonly Direction[] _directions2D =
        {
            Direction.Right,
            Direction.UpRight,
            Direction.Up,
            Direction.UpLeft,
            Direction.Left,
            Direction.DownLeft,
            Direction.Down,
            Direction.DownRight,
        };

        private static readonly Direction[] _directions3D =
        {
            Direction.Right,
            Direction.UpRight,
            Direction.Up,
            Direction.UpLeft,
            Direction.Left,
            Direction.DownLeft,
            Direction.Down,
            Direction.DownRight,

            Direction.Forward,
            Direction.ForwardRight,
            Direction.ForwardUpRight,
            Direction.ForwardUp,
            Direction.ForwardUpLeft,
            Direction.ForwardLeft,
            Direction.ForwardDownLeft,
            Direction.ForwardDown,
            Direction.ForwardDownRight,

            Direction.Backward,
            Direction.BackwardRight,
            Direction.BackwardUpRight,
            Direction.BackwardUp,
            Direction.BackwardUpLeft,
            Direction.BackwardLeft,
            Direction.BackwardDownLeft,
            Direction.BackwardDown,
            Direction.BackwardDownRight,
        };

        private static readonly ReadOnlyCollection<Direction> _directions2DAsReadOnly
            = Array.AsReadOnly(_directions2D);

        private static readonly ReadOnlyCollection<Direction> _directions3DAsReadOnly
            = Array.AsReadOnly(_directions3D);

        /// <summary>
        /// A list of the eight directions in 2D rectangular tile maps.
        /// </summary>
        public static ReadOnlyCollection<Direction> Directions2DAll => _directions2DAsReadOnly;

        /// <summary>
        /// A list of the eight directions in 3D rectangular tile maps.
        /// </summary>
        public static ReadOnlyCollection<Direction> Directions3DAll => _directions3DAsReadOnly;

        //===== Direction

        public static DirectionStep Step(this Direction direction)
        {
            return DirectionDeltas[(int) direction];
        }

        public static Direction Step2D4(int xDelta, int yDelta, int zDelta = 0)
        {
            var direction2D = DirectionAngle2D.Of(new MzVector(xDelta, yDelta, zDelta));
            var direction = GetDirection2D4(direction2D);
            return direction;
        }

        /// <summary>
        /// Returns the direction opposite to this one.
        /// </summary>
        public static Direction Opposite(this Direction direction)
        {
            return _directionOpposite[(int) direction];
        }

        /// <summary>
        /// Returns the closest of the eight directions inside a rectangular tile map, by angle.
        /// </summary>
        public static Direction GetDirection2D8(this DirectionAngle2D directionAngle)
        {
            return (Direction) ((int) Math.Floor(directionAngle.Degrees * (1 / 45f) + 0.5f) % 8 + 1);
        }

        /// <summary>
        /// Returns the closest of the four non-diagonal directions inside a rectangular tile map, by angle.
        /// </summary>
        public static Direction GetDirection2D4(this DirectionAngle2D directionAngle)
        {
            return (Direction) ((int) Math.Floor(directionAngle.Degrees * (1 / 90f) + 0.5f) % 4 * 2 + 1);
        }

        //===== Directions

        /// <summary>
        /// Checks if any directions are set.
        /// </summary>
        /// <returns>True if any direction is set. False otherwise.</returns>
        public static bool Any(this Directions directions)
        {
            return directions != Directions.None;
        }

        /// <summary>
        /// Checks if any of the given directions are set, i.e. if the directions intersect.
        /// </summary>
        /// <param name="directions">The directions to check.</param>
        /// <param name="match">The directions to search for.</param>
        /// <returns>True if the directions intersect. False otherwise</returns>
        public static bool Any(this Directions directions, Directions match)
        {
            return directions.Intersect(match) != Directions.None;
        }

        /// <summary>
        /// Checks if all directions are set.
        /// </summary>
        /// <returns>True if all direction are set. False otherwise.</returns>
        public static bool All(Directions directions)
        {
            return directions == Mz.Numerics.Directions.All2D;
        }

        /// <summary>
        /// Checks if all of the given directions are set, i.e. if their intersection equals the parameter.
        /// </summary>
        /// <param name="directions">The directions to check.</param>
        /// <param name="match">The directions to search for.</param>
        /// <returns>True if all directions are included. False otherwise</returns>
        public static bool All(this Directions directions, Directions match)
        {
            return directions.HasFlag(match);
        }

        /// <summary>
        /// Returns the union of two sets of directions.
        /// </summary>
        public static Directions Union(this Directions directions, Directions directions2)
        {
            return directions | directions2;
        }

        /// <summary>
        /// Excludes one set of directions from another.
        /// </summary>
        /// <returns>The original value, without the given directions.</returns>
        public static Directions Except(this Directions directions, Directions directions2)
        {
            return directions & ~directions2;
        }

        /// <summary>
        /// Returns the intersection of two sets of directions.
        /// </summary>
        public static Directions Intersect(this Directions directions, Directions directions2)
        {
            return directions & directions2;
        }

        //===== Direction and Directions

        public static Directions ToDirections(this Direction direction)
        {
            return (Directions) (1 << ((int) direction - 1));
        }

        /// <summary>
        /// Enumerates all the set directions.
        /// </summary>
        public static IEnumerable<Direction> Enumerate2D(this Directions directions)
        {
            return _directions2D.Where(direction => directions.Includes(direction));
        }

        /// <summary>
        /// Enumerates all the set directions.
        /// </summary>
        public static IEnumerable<Direction> Enumerate3D(this Directions directions)
        {
            return _directions3D.Where(direction => directions.Includes(direction));
        }

        /// <summary>
        /// Checks if a specific direction is set.
        /// </summary>
        /// <returns>True if the given direction is set. False otherwise.</returns>
        public static bool Includes(this Directions directions, Direction direction)
        {
            return directions.HasFlag(direction.ToDirections());
        }

        /// <summary>
        /// Sets a given direction.
        /// </summary>
        /// <returns>A union of the original value and the given direction.</returns>
        public static Directions And(this Directions directions, Direction direction)
        {
            return directions | direction.ToDirections();
        }

        /// <summary>
        /// Un-sets a given direction.
        /// </summary>
        /// <returns>The original value, without the given direction.</returns>
        public static Directions Except(this Directions directions, Direction direction)
        {
            return directions & ~direction.ToDirections();
        }
    }
}
