using Sirenix.OdinInspector;
using UnityEngine;

namespace MazeGenerator
{
    public class MazeScriptableObject : SerializedScriptableObject, IMaze
    {
        const WallType resetWallType = WallType.Right | WallType.Left | WallType.Up | WallType.Down;
        
        [SerializeField] private int _width;
        [SerializeField] private int _length;
        [SerializeField] private Vector2 _entrance;
        [SerializeField] private Vector2 _exit;
        
        [SerializeField][TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis")] private WallType[,] _maze;

        public int Width => _width;
        public int Length => _length;
        public Vector2 Entrance => _entrance;
        public Vector2 Exit => _exit;
        
        public WallType this[int i, int j]
        {
            get => _maze[i, j];
            set => _maze[i, j] = value;
        }

        public WallType this[Vector2 coord]
        {
            get => _maze[coord.X, coord.Y];
            set => _maze[coord.X, coord.Y] = value;
        }

        public void Init(int width, int length)
        {
            _width = width;
            _length = length;
            _maze = new WallType[width, length];
            Reset();
        }
        
        public void SetEntrance(Vector2 entrance) => _entrance = entrance;
        public void SetExit(Vector2 exit) => _exit = exit;

        public void Reset()
        {
            for (var i = 0; i < _width; i++)
                for (var j = 0; j < _length; j++)
                    _maze[i, j] = resetWallType;
        }
    }
}