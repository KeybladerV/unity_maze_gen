using Sirenix.OdinInspector;
using UnityEngine;

namespace MazeGenerator
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MazeView : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 1;

        /// <summary>Used to make cells fit each other. For default cell it's - 2f(that matches default wall scale). </summary>
        [SerializeField] private float _cellPositionCoef = 2f;

        [SerializeField] private MazeCellView _cellPrefab;
        [SerializeField] private GameObject _enterPrefab;
        [SerializeField] private GameObject _finishPrefab;

        [SerializeField] private Transform _cellsContainer;

        [SerializeField] private MazeScriptableObject _maze;
        
        private MazeCellView[,] _cells;
        private Vector2Int _cellsArraySize = new Vector2Int();

        private GameObject _entrance;
        private GameObject _exit;

        public MazeScriptableObject Maze => _maze;

        public void SetMaze(MazeScriptableObject maze) => _maze = maze;

        [Button]
        public void DoDrawing()
        {
            if (_maze == null)
            {
                Debug.LogError($"{this} has no maze set! Cannot draw {null}!");
                return;
            }

            Clear();

            DrawMaze(_maze);
        }

        private void DrawMaze(IMaze maze)
        {
            for (var i = 0; i < maze.Width; i++)
            {
                for (var j = 0; j < maze.Length; j++)
                {
                    var wallType = maze[i, j];
                    var pos = GetCellWorldPos(i, j);

                    if (maze.Entrance.X == i && maze.Entrance.Y == j)
                    {
                        _entrance = Instantiate(_enterPrefab, pos, Quaternion.identity, transform);
                        _entrance.name = "Entrance";
                    }

                    if (maze.Exit.X == i && maze.Exit.Y == j)
                    {
                        _exit = Instantiate(_finishPrefab, pos, Quaternion.identity, transform);
                        _exit.name = "Exit";
                    }

                    _cells[i, j] = Instantiate(_cellPrefab, pos, Quaternion.identity, _cellsContainer);
                    _cells[i, j].SetSize(_cellSize);
                    _cells[i, j].SetState(wallType);
                    _cells[i, j].name = $"Cell_X{i}_Y{j}";
                }
            }
        }

        public Vector3 GetCellWorldPos(int x, int z) =>
            new Vector3(_cellSize + x * _cellSize * _cellPositionCoef, 0,
                _cellSize + z * _cellSize * _cellPositionCoef) + transform.position;

        public Vector3 GetCellWorldPos(Vector2 xz) => GetCellWorldPos(xz.X, xz.Y);

        public Vector2 GetCellByWorldPos(Vector3 worldPos)
        {
            var res = Vector2.zero;
            worldPos -= transform.position;
            res.X = Mathf.FloorToInt(worldPos.x / (_cellSize * _cellPositionCoef));
            res.Y = Mathf.FloorToInt(worldPos.z / (_cellSize * _cellPositionCoef));
            return res;
        }

        [Button]
        public void Clear()
        {
            if (_cells != null)
            {
                for (var i = 0; i < _cellsArraySize.x; i++)
                    for (var j = 0; j < _cellsArraySize.y; j++)
                        if (_cells[i, j] != null)
                            DestroyImmediate(_cells[i, j].gameObject);
            }

            _cells = new MazeCellView[_maze.Width, _maze.Length];
            _cellsArraySize.x = _maze.Width;
            _cellsArraySize.y = _maze.Length;

            DestroyImmediate(_entrance);
            DestroyImmediate(_exit);
            _entrance = null;
            _exit = null;
        }
    }
}