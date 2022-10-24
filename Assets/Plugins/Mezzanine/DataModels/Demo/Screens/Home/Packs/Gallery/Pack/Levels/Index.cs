using System;
using UnityEngine;
using UnityEngine.UI;
using Mz.App;
using Mz.DemoDataModels.Models;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack.Levels
{
    public class Index : MzBehaviour
    {
        private LevelGrid _levelGrid;
        private LevelGridCell[,] _tileImages;
        private float _tileMapHeight;
        private float _tileGap;
        private float _tileMapWidth;
        
        public float Height { get; private set; }
        
        public event EventHandler<int> LevelClicked;

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.pivot = new Vector2(0.5f, 0.5f);

            _tileGap = 24;
            _tileMapWidth = Core.ScreenWidth - Core.Specs.GutterHorizontal * 2;
            
            var canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1;

            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minWidth = _tileMapWidth;
            layoutElement.preferredWidth = _tileMapWidth;
        }

        public void LoadData(PackData packData)
        {
            _levelGrid = LevelGrid.Make(6, packData.Levels.Length / 6);
            _LayoutLevelGrid(_levelGrid);
            _SetLevelStates(packData);
        }

        private void _SetLevelStates(PackData packData)
        {
            var count = 0;
            
            // Go through the puzzle row-by-row and column-by-column.
            for (var row = 0; row < _levelGrid.MzTiles.Height; row++)
            {
                for (var column = 0; column < _levelGrid.MzTiles.Width; column++)
                {
                    var levelState = packData.Levels[count];
                    var tileImage = _tileImages[column, row];
                    
                    if (tileImage != null) tileImage.SetState(levelState);
                    
                    count++;
                    if (!packData.IsPurchased && count % 6 == 0)
                    {
                        if (levelState > -1) tileImage.SetState(-2); // Ad was watched.
                        tileImage.Label.Text.text = "AD";
                    }
                    else if (levelState == 1) tileImage.Label.Text.text = "╳";
                    else if (levelState == 2) tileImage.Label.Text.text = "*";
                    else if (levelState > 2) tileImage.Label.Text.text = "+";
                    else tileImage.Label.Text.text = $"{count}";
                }
            }
        }
        
        private void _LayoutLevelGrid(LevelGrid levelGrid)
        {
            var tileMap = levelGrid.MzTiles;

            _tileMapHeight = levelGrid.RowCount * _tileMapWidth / levelGrid.ColumnCount;
            Height = _tileMapHeight;
            
            transform.Clear();
            var rectTransform = Rect;
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(_tileMapWidth, _tileMapHeight);

            // Create and position the tiles
            var count = 0;
            _tileImages = new LevelGridCell[tileMap.Width, tileMap.Height];
            for (var row = 0; row < tileMap.Height; row++)
            {
                for (var column = 0; column < tileMap.Width; column++)
                {
                    count++;
                    
                    var rectangleTile = tileMap.MapToWorldDimensions(
                        _tileMapWidth,
                        _tileMapHeight,
                        column,
                        row,
                        _tileGap
                    );
                    rectangleTile.Y *= -1;

                    var tile = Add<LevelGridCell>($"tile_{column}_{row}");
                    tile.transform.SetParent(transform);

                    rectTransform = tile.GetComponent<RectTransform>();
                    rectTransform.localScale = new Vector3(1, 1, 1);
                    rectTransform.sizeDelta = new Vector2((float) rectangleTile.Width, (float) rectangleTile.Width);
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.pivot = new Vector2(0, 1);
                    rectTransform.localPosition = new Vector3((float) rectangleTile.X, (float) rectangleTile.Y, 0);
                    rectTransform.anchoredPosition = new Vector2((float) rectangleTile.X, (float) rectangleTile.Y);
                    
                    _tileImages[column, row] = tile;
                }
            }
        }
        
        public void Update()
        {
            _ApplyUserInput();
        }
        
        private bool _isProcessingUserInput;
        private void _ApplyUserInput()
        {
            if (_isProcessingUserInput) return;
            if (!Input.GetMouseButtonDown(0)) return;
            _isProcessingUserInput = true;
            var rectTransform = Rect;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                Input.mousePosition,
                Core.CameraMain.Camera,
                out var localPosition
            );
            
            OnUserInputPanelMain(localPosition);
            _isProcessingUserInput = false;
        }
        
        protected void OnUserInputPanelMain(Vector2 localPosition)
        {
            if (_levelGrid == null) return;

            localPosition.x += _tileMapWidth / 2;
            localPosition.y -= _tileMapWidth / _levelGrid.ColumnCount * _levelGrid.RowCount / 2;
            localPosition.y *= -1;

            if (localPosition.x < 0 || localPosition.y < 0) return;

            if (_levelGrid?.MzTiles == null) return;
            var grid = _levelGrid;
            var tile = grid.MzTiles.GetTileFromWorldDimensions(
                _tileMapWidth,
                _tileMapHeight,
                localPosition.x,
                localPosition.y
            );
            
            if (tile.IsEmpty) return;

            var index = tile.Y * _levelGrid.ColumnCount + tile.X;
            LevelClicked?.Invoke(this, index);

            var isBlocked = false;
            var tileX = 0;

            var valueOld = tile.Value;
            var valueNew = tile.Value;

            switch (tile.Value)
            {
                case -1:
                    tileX = tile.X;
                    if (grid.MzTiles.TileValuesBlocked[tileX, tile.Y] == 0) isBlocked = true;
                    tileX = grid.ColumnCount - tile.X - 1;
                    if (!isBlocked && grid.MzTiles.TileValuesBlocked[tileX, tile.Y] == 0) isBlocked = true;

                    valueNew = isBlocked ? 1 : 0;
                    break;
                case 0:
                    tileX = tile.X;
                    if (grid.MzTiles.TileValuesBlocked[tileX, tile.Y] == 1) isBlocked = true;
                    tileX = grid.ColumnCount - tile.X - 1;
                    if (!isBlocked && grid.MzTiles.TileValuesBlocked[tileX, tile.Y] == 1) isBlocked = true;

                    valueNew = isBlocked ? -1 : 1;
                    break;
                case 1:
                    valueNew = -1;
                    break;
            }

            if (valueOld == valueNew) return;

            grid.MzTiles[tile.X, tile.Y] = valueNew;
            grid.MzTiles[grid.ColumnCount - tile.X - 1, tile.Y] = valueNew;
        }
    } 
}