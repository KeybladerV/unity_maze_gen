namespace MazeGenerator
{
    public sealed class Maze : IMaze
    {
        private readonly int _width;
        private readonly int _length;
        private readonly CellType[,] _maze;
        private Vector2 _entrance;
        private Vector2 _exit;

        public int Width => _width;
        public int Length => _length;
        public Vector2 Entrance => _entrance;
        public Vector2 Exit => _exit;
        
        public CellType this[int i, int j]
        {
            get => _maze[i, j];
            set => _maze[i, j] = value;
        }

        public CellType this[Vector2 coord]
        {
            get => _maze[coord.X, coord.Y];
            set => _maze[coord.X, coord.Y] = value;
        }

        public Maze(int width, int length)
        {
            _width = width;
            _length = length;
            _maze = new CellType[_width, _length];
            
            Reset();
        }

        public void SetEntrance(Vector2 entrance) => _entrance = entrance;
        public void SetExit(Vector2 exit) => _exit = exit;
        
        public bool IsInBounds(Vector2 coord) => coord.X >= 0 && coord.X < _width && coord.Y >= 0 && coord.Y < _length;
        public bool IsValid(Vector2 coord) => IsInBounds(coord) && _maze[coord.X, coord.Y] != CellType.Empty;
        
        /// <summary> Returns true if the cell is in bounds and returns the cell type, otherwise returns false and CellType.Empty </summary>
        public bool TryGetCell(Vector2 coord, out CellType cell)
        {
            if (IsInBounds(coord))
            {
                cell = _maze[coord.X, coord.Y];
                return true;
            }

            cell = CellType.Empty;
            return false;
        }

        public void Reset()
        {
            for (var i = 0; i < _width; i++)
                for (var j = 0; j < _length; j++)
                    _maze[i, j] = CellType.AllWalls;
        }
    }
}