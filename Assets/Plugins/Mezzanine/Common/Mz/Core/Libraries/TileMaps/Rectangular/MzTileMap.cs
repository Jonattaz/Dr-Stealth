using System;
using System.Text;
using Mz.Numerics;

namespace Mz.TileMaps.Rectangular {
    /// <summary>
    /// A rectangular mzTileMap.
    /// </summary>
    /// <typeparam name="TTileValue">The kind of _data contained in the mzTileMap.</typeparam>
    public class MzTileMap<TTileValue> /*: ISerializable*/ {

        public MzTileMap() : this(1, 1, default) {}

        /// <summary>
        /// Creates a new rectangular mzTileMap.
        /// </summary>
        /// <param name="width">The width of the mzTileMap.</param>
        /// <param name="height">The height of the mzTileMap.</param>
        public MzTileMap(int width = 1, int height = 1, TTileValue defaultValue = default) {
            Width = width;
            Height = height;
            Count = Width * Height;
            TileValues = new TTileValue[width, height];
            TileValuesBlocked = new TTileValue[width, height];
        }

        public MzTileMap(TTileValue[,] tileValues) {
            Width = tileValues.GetLength(0);
            Height = tileValues.GetLength(1);
            Count = tileValues.Length;
            TileValues = tileValues;
            TileValuesBlocked = new TTileValue[Width, Height];
        }

        #region properties

        public TTileValue[,] TileValues { get; private set; }
        public TTileValue[,] TileValuesBlocked { get; private set; }

        /// <summary>
        /// The width of the mzTileMap.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the mzTileMap.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the number o tiles in the mzTileMap.
        /// </summary>
        public int Count { get; private set; }

        #endregion

        #region indexers

        /// <summary>
        /// Gets or sets the _data of a mzTile by its coordinates.
        /// </summary>
        /// <param name="x">The X location of the mzTile.</param>
        /// <param name="y">The Y location of the mzTile.</param>
        public TTileValue this[int x, int y] {
            get => TileValues[x, y];
            set => TileValues[x, y] = value;
        }

        /// <summary>
        /// Gets or sets the _data of a mzTile.
        /// </summary>
        /// <param name="mzTile">The mzTile.</param>
        public TTileValue this[MzTile<TTileValue> mzTile] {
            get => this[mzTile.X, mzTile.Y];
            set => this[mzTile.X, mzTile.Y] = value;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Takes the actual pixel dimensions of the map in world space, along
        /// with a position. Returns the corresponding mzTile.
        /// </summary>
        public MzTile<TTileValue> GetTile(int x, int y) {
            MzTile<TTileValue> mzTile;

            if (x < Width && y < Height) {
                mzTile = new MzTile<TTileValue>(this, x, y);
                mzTile.Value = this[x, y];
            } else mzTile = new MzTile<TTileValue>(this);

            return mzTile;
        }

        /// <summary>
        /// Takes the actual pixel dimensions of the map in world space, along
        /// with a position. Returns the corresponding mzTile.
        /// </summary>
        public MzTile<TTileValue> GetTileFromWorldDimensions(float mapWorldWidth, float mapWorldHeight, float worldX, float worldY) {
            var tilePosition = WorldToMapDimensions(mapWorldWidth, mapWorldHeight, worldX, worldY);
            return GetTile((int)tilePosition.X, (int)tilePosition.Y);
        }

        /// <summary>
        /// Takes the actual pixel dimensions of the map in world space, along
        /// with a position. Returns the corresponding mzTile coordinates.
        /// </summary>
        public MzVector WorldToMapDimensions(float mapWorldWidth, float mapWorldHeight, float worldX, float worldY) {
            var tileWorldWidth = mapWorldWidth / Width;
            var tileWorldHeight = mapWorldHeight / Height;
            var tileX = (float)Math.Floor(worldX / tileWorldWidth);
            var tileY = (float)Math.Floor(worldY / tileWorldHeight);
            return new MzVector(tileX, tileY);
        }

        /// <summary>
        /// Takes the actual pixel dimensions of the map in world space, along
        /// with a position. Returns the corresponding mzTile coordinates.
        /// </summary>
        public MzRectangle MapToWorldDimensions(
            float mapWorldWidth,
            float mapWorldHeight,
            float tileX,
            float tileY,
            float gap = 0
        ) {
            var tileWorldWidth = (mapWorldWidth - gap * (Width - 1)) / Width;
            var tileWorldHeight = (mapWorldHeight - gap * (Height - 1)) / Height;
            var worldX = tileWorldWidth * tileX + gap * tileX;
            var worldY = tileWorldHeight * tileY + gap * tileY;
            var tileRectangle = new MzRectangle(worldX, worldY, tileWorldWidth, tileWorldHeight);
            return tileRectangle;
        }

        /// <summary>
        /// Checks whether a specific mzTile location is inside the mzTileMap.
        /// </summary>
        /// <param name="x">The X location of the mzTile.</param>
        /// <param name="y">The Y location of the mzTile.</param>
        public bool IsValidTile(int x, int y) {
            return x >= 0 && x < Width
                && y >= 0 && y < Height;
        }

        /// <summary>
        /// Checks whether a specific mzTile is located inside the mzTileMap.
        /// </summary>
        public bool IsValidTile(MzTile<TTileValue> mzTile) {
            return IsValidTile(mzTile.X, mzTile.Y);
        }

        #endregion

        public override string ToString() {
            var lineSep = new string('-', 4 * Width + 1);
            var output = new StringBuilder();
            output.Append(lineSep);
            output.AppendLine();

            for (var row = 0; row < Width; row++) {
                output.Append("| ");
                for (var column = 0; column < Height; column++) {
                    var value = this[row, column];
                    output.Append(value);
                    output.Append(" | ");
                }

                output.AppendLine();
                output.Append(lineSep);
                output.AppendLine();
            }

            return output.ToString();
        }
    }
}
