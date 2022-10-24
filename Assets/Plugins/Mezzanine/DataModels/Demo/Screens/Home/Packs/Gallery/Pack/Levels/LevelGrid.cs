using Mz.TileMaps.Rectangular;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack.Levels
{
    public class LevelGrid
    {
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }

        public MzTileMap<int> MzTiles { get; private set; }

        public LevelGrid(
            int columnCount,
            int rowCount,
            int colorCount,
            int seed = int.MinValue
        )
        {
            ColumnCount = columnCount;
            RowCount = rowCount;

            MzTiles = new MzTileMap<int>(columnCount, rowCount, -1);
        }

        public static LevelGrid Make(int columnCount = 5, int rowCount = 5, int colorCount = 2, int seed = int.MinValue)
        {
            var levelGrid = new LevelGrid(
                columnCount,
                rowCount,
                colorCount,
                seed
            );

            for (var column = 0; column < levelGrid.ColumnCount; column++)
            {
                for (var row = 0; row < levelGrid.RowCount; row++)
                {
                    var tileValue = -2;
                    levelGrid.MzTiles[column, row] = tileValue;
                }

                levelGrid.MzTiles[0, 0] = 1;
            }

            return levelGrid;
        }
    }
}