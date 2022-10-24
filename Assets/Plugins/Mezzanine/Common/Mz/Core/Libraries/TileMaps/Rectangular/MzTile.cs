using System;
using System.Collections.Generic;
using Mz.Numerics;

namespace Mz.TileMaps.Rectangular {
    /// <summary>
    /// Represents a reference to a specific tile of a rectangular mzTileMap.
    /// </summary>
    /// <typeparam name="TTileValue">The kind of _data contained in the mzTileMap.</typeparam>
    public struct MzTile<TTileValue>
        : IEquatable<MzTile<TTileValue>> {
        private readonly MzTileMap<TTileValue> _mzTileMap;

        /// <summary>
        /// Creates a new tile reference.
        /// </summary>
        /// <param name="mzTileMap">The mzTileMap.</param>
        /// <param name="x">X location of tile.</param>
        /// <param name="y">Y location of tile.</param>
        /// <exception cref="ArgumentNullException">Throws if mzTileMap is null.</exception>
        public MzTile(MzTileMap<TTileValue> mzTileMap, int x = int.MinValue, int y = int.MinValue) {
            _mzTileMap = mzTileMap ?? throw new ArgumentNullException(nameof(mzTileMap));
            X = x;
            Y = y;

            if (x > int.MinValue && y > int.MinValue) IsEmpty = false;
            else IsEmpty = true;
        }

        #region properties

        /// <summary>
        /// X location of the tile.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y location of the tile.
        /// </summary>
        public int Y { get; private set; }

        public bool IsEmpty { get; private set; }

        /// <summary>
        /// The data contained in the tile. Will throw if <see cref="IsValid"/> is false.
        /// </summary>
        public TTileValue Value {
            get { return _mzTileMap[X, Y]; }
            set { _mzTileMap[X, Y] = value; }
        }

        /// <summary>
        /// Data that can't be contained in the tile.
        /// </summary>
        public TTileValue ValueBlocked {
            get { return _mzTileMap.TileValuesBlocked[X, Y]; }
            set { _mzTileMap.TileValuesBlocked[X, Y] = value; }
        }

        /// <summary>
        /// True if the tile is located within its mzTileMap. False otherwise. 
        /// </summary>
        public bool IsValid {
            get { return _mzTileMap != null && _mzTileMap.IsValidTile(this); }
        }

        /// <summary>
        /// Returns all eight tiles neighbouring this one, except those outside the mzTileMap.
        /// </summary>
        public IEnumerable<MzTile<TTileValue>> ValidNeighbours {
            get {
                for (int i = 1; i < 9; i++) {
                    var tile = Neighbour(Extensions.DirectionDeltas[i]);
                    if (tile.IsValid)
                        yield return tile;
                }
            }
        }

        /// <summary>
        /// Returns all eight tiles neighbouring this one, whether or not they are outside the mzTileMap.
        /// </summary>
        public IEnumerable<MzTile<TTileValue>> PossibleNeighbours {
            get {
                for (int i = 1; i < 9; i++) {
                    yield return Neighbour(Extensions.DirectionDeltas[i]);
                }
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns the tile neighbouring this one in a given direction.
        /// </summary>
        public MzTile<TTileValue> Neighbour(Direction direction) {
            return Neighbour(direction.Step());
        }

        internal MzTile<TTileValue> Neighbour(Step step) {
            return new MzTile<TTileValue>(
                _mzTileMap,
                X + step.X,
                X + step.Y
                );
        }

        #endregion

        #region Equals, GetHashCode, euqality operators

        /// <summary>
        /// Indicates whether this instance references the same tile as another one.
        /// </summary>
        public bool Equals(MzTile<TTileValue> other) {
            return X == other.X && Y == other.Y && _mzTileMap == other._mzTileMap;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is MzTile<TTileValue> && Equals((MzTile<TTileValue>)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() {
            unchecked {
                var hashCode = (_mzTileMap != null ? _mzTileMap.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }

        /// <summary>
        /// Indicates whether this instance references the same tile as another one.
        /// </summary>
        public static bool operator ==(MzTile<TTileValue> t1, MzTile<TTileValue> t2) {
            return t1.Equals(t2);
        }

        /// <summary>
        /// Indicates whether this instance references a different tile than another one.
        /// </summary>
        public static bool operator !=(MzTile<TTileValue> t1, MzTile<TTileValue> t2) {
            return !(t1 == t2);
        }

        #endregion
    }
}
