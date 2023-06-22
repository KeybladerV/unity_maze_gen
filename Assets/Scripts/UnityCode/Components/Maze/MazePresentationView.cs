using System;
using System.Collections;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using MazeGenerator;
using UnityCode.Modules.Metrics;
using UnityEngine;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;
using Vector2 = MazeGenerator.Vector2;

namespace Components.Maze
{
    [Mediator(typeof(MazePresentationMediator))]
    public sealed class MazePresentationView : UnityViewDispatcher
    {
        public readonly Event OnMazeDrawStart = new(typeof(MazePresentationView), nameof(OnMazeDrawStart));
        public readonly Event OnMazeDrawEnd = new(typeof(MazePresentationView), nameof(OnMazeDrawEnd));
        
        [Inject] public IFPSController FPSController { get; set; }

        [SerializeField] private float _cellSize = 1;

        /// <summary>Used to make cells fit each other. For default cell it's - 2f(that matches default wall scale). </summary>
        [SerializeField] private float _cellPositionCoef = 2f;

        [SerializeField] private Transform _cellsContainer;

        private IMaze _maze;

        private MazeCellView[,] _cells;

        private GameObject _entrance;
        private GameObject _exit;

        private int _cellsPerFrame = 10;
        private int _cellsToSpawn;

        private Vector2? _lastMinMaxX;
        private Vector2? _lastMinMaxY;

        public void SetMaze(IMaze maze)
        {
            _maze = maze;
            _cells = new MazeCellView[_maze.Width, _maze.Length];
            _cellsToSpawn = _cellsPerFrame;
        }

        public void SetCellsSize(int size) => _cellSize = size;

        public void DrawMazePart(Vector2 minMaxX, Vector2 minMaxY, Func<MazeCellView> getCell,
            Action<MazeCellView> removeCell)
        {
            StartCoroutine(DrawMazeCoroutine(minMaxX.X, minMaxX.Y, minMaxY.X, minMaxY.Y, getCell, removeCell));
        }

        private IEnumerator DrawMazeCoroutine(int newMinX, int newMaxX, int newMinY, int newMaxY,
            Func<MazeCellView> getCell, Action<MazeCellView> removeCell)
        {
            var minX = newMinX;
            var maxX = newMaxX;
            var minY = newMinY;
            var maxY = newMaxY;

            if (_lastMinMaxX != null && _lastMinMaxY != null)
            {
                minX = Mathf.Min(_lastMinMaxX.Value.X, newMinX);
                maxX = Mathf.Max(_lastMinMaxX.Value.Y, newMaxX);
                minY = Mathf.Min(_lastMinMaxY.Value.X, newMinY);
                maxY = Mathf.Max(_lastMinMaxY.Value.Y, newMaxY);
            }
            
            Dispatch(OnMazeDrawStart);
            for (var i = minX; i < maxX; i++)
            {
                for (var j = minY; j < maxY; j++)
                {
                    

                    // if (_maze.Entrance.X == i && _maze.Entrance.Y == j)
                    // {
                    //     _entrance = Instantiate(_enterPrefab, pos, Quaternion.identity, transform);
                    //     _entrance.name = "Entrance";
                    // }

                    // if (_maze.Exit.X == i && _maze.Exit.Y == j)
                    // {
                    //     _exit = Instantiate(_finishPrefab, pos, Quaternion.identity, transform);
                    //     _exit.name = "Exit";
                    // }

                    if ((newMinX <= i && i <= newMaxX) && (newMinY <= j && j <= newMaxY))
                    {
                        if (_cells[i, j] != null)
                            continue;
                        
                        var cell = getCell();

                        cell.transform.position = GetWorldPosByCell(i, j);
                        cell.transform.SetParent(_cellsContainer);
                        cell.SetSize(_cellSize);
                        cell.SetState(_maze[i, j]);
                        cell.name = $"Cell_X{i}_Y{j}";
                        
                        cell.gameObject.SetActive(true);
                        
                        _cells[i, j] = cell;

                        _cellsToSpawn--;
                        if (_cellsToSpawn <= 0)
                        {
                            if (FPSController.FPS < 60)
                                _cellsPerFrame = Mathf.Max(1, _cellsPerFrame - 1);
                            else if (FPSController.FPS > 80)
                                _cellsPerFrame++;

                            _cellsToSpawn = _cellsPerFrame;
                            yield return null;
                        }
                    }
                    else
                    {
                        removeCell(_cells[i, j]);
                        _cells[i, j] = null;
                    }
                }
            }
            
            _lastMinMaxX = new Vector2(newMinX, newMaxX);
            _lastMinMaxY = new Vector2(newMinY, newMaxY);
            
            Dispatch(OnMazeDrawEnd);
        }

        public Vector3 GetWorldPosByCell(Vector2 xz) => GetWorldPosByCell(xz.X, xz.Y);

        public Vector3 GetWorldPosByCell(int x, int z) =>
            new Vector3(_cellSize + x * _cellSize * _cellPositionCoef, 0,
                _cellSize + z * _cellSize * _cellPositionCoef) + transform.position;

        public Vector2 GetCellByWorldPos(Vector3 worldPos)
        {
            var res = Vector2.zero;
            worldPos -= transform.position;
            res.X = Mathf.FloorToInt(worldPos.x / (_cellSize * _cellPositionCoef));
            res.Y = Mathf.FloorToInt(worldPos.z / (_cellSize * _cellPositionCoef));
            return res;
        }

        public void Clear(Action<MazeCellView> removeAction)
        {
            if (_cells == null)
                return;
            
            for (var i = 0; i < _maze.Width; i++)
                for (var j = 0; j < _maze.Length; j++)
                    if (_cells[i, j] != null)
                        removeAction(_cells[i, j]);
            
            _cells = null;
            _entrance = null;
            _exit = null;
            _lastMinMaxX = null;
            _lastMinMaxY = null;
        }
    }
}