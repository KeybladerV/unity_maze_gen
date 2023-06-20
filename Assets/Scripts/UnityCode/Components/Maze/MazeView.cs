using System.Collections;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using Components.Maze;
using MazeGenerator;
using UnityCode.Modules.Metrics;
using UnityEngine;
using Vector2 = MazeGenerator.Vector2;

namespace Components.Maze
{
    [Mediator(typeof(MazeMediator))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class MazeView : UnityViewDispatcher
    {
        [Inject] public IFPSController FPSController { get; set; }
        
        [SerializeField] private float _cellSize = 1;

        /// <summary>Used to make cells fit each other. For default cell it's - 2f(that matches default wall scale). </summary>
        [SerializeField] private float _cellPositionCoef = 2f;

        [SerializeField] private MazeCellView _cellPrefab;
        [SerializeField] private GameObject _enterPrefab;
        [SerializeField] private GameObject _finishPrefab;

        [SerializeField] private Transform _cellsContainer;

        private IMaze _maze;

        private MazeCellView[,] _cells;

        private GameObject _entrance;
        private GameObject _exit;

        private int _cellsPerFrame = 10;
        private int _cellsToSpawn;

        public void SetMaze(IMaze maze) => _maze = maze;
        public void SetCellSize(int size) => _cellSize = size;

        public void DrawMaze()
        {
            _cells = new MazeCellView[_maze.Width, _maze.Length];
            _cellsToSpawn = _cellsPerFrame;
            
            StartCoroutine(DrawMazeCoroutine());
        }

        private IEnumerator DrawMazeCoroutine()
        {
            for (var i = 0; i < _maze.Width; i++)
            {
                for (var j = 0; j < _maze.Length; j++)
                {
                    var wallType = _maze[i, j];
                    var pos = GetWorldPosByCell(i, j);

                    if (_maze.Entrance.X == i && _maze.Entrance.Y == j)
                    {
                        _entrance = Instantiate(_enterPrefab, pos, Quaternion.identity, transform);
                        _entrance.name = "Entrance";
                    }

                    if (_maze.Exit.X == i && _maze.Exit.Y == j)
                    {
                        _exit = Instantiate(_finishPrefab, pos, Quaternion.identity, transform);
                        _exit.name = "Exit";
                    }

                    _cells[i, j] = Instantiate(_cellPrefab, pos, Quaternion.identity, _cellsContainer);
                    _cells[i, j].SetSize(_cellSize);
                    _cells[i, j].SetState(wallType);
                    _cells[i, j].name = $"Cell_X{i}_Y{j}";

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
            }
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

        public void Clear(bool destroy)
        {
            if (_cells == null)
                return;

            if(destroy)
                DestroyAll();
            else
                ReturnAllToPool();
        }

        private void DestroyAll()
        {
            for (var i = 0; i < _maze.Width; i++)
                for (var j = 0; j < _maze.Length; j++)
                    if (_cells[i, j] != null)
                        DestroyImmediate(_cells[i, j].gameObject);
            
            DestroyImmediate(_entrance);
            DestroyImmediate(_exit);
            
            _cells = null;
            _entrance = null;
            _exit = null;
        }

        private void ReturnAllToPool()
        {
            
        }
    }
}