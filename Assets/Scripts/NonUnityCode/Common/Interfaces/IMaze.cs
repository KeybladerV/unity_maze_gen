namespace MazeGenerator
{
    /// <summary> Represents maze as data object and provides some help methods. </summary>
    public interface IMaze
    {
        /// <summary> Maze width. </summary>
        int Width { get; }
        /// <summary> Maze length. </summary>
        int Length { get; }
        
        /// <summary> X,Y coordinates of maze entrance. </summary>
        Vector2 Entrance { get; }
        /// <summary> X,Y coordinates of maze exit. </summary>
        Vector2 Exit { get; }
        
        /// <summary> Returns cell type by coordinates. </summary>
        CellType this[int i, int j] { get; set; }
        /// <summary> Returns cell type by coordinates. </summary>
        CellType this[Vector2 coord] { get; set; }

        /// <summary>Sets the coordinates for maze entrance. </summary>
        void SetEntrance(Vector2 entrance);
        /// <summary>Sets the coordinates for maze exit. </summary>
        void SetExit(Vector2 exit);
        
        /// <summary>Checks if given coordinates is inside the maze. </summary>
        bool IsInBounds(Vector2 coord);
        /// <summary>Checks if given coordinates is inside the maze and cell is valid and usable. </summary>
        bool IsValid(Vector2 coord);
        
        /// <summary>Tries to get cell by given coordinates. On true returns actual data, on false returns <see cref="CellType.Empty"/>. </summary>
        bool TryGetCell(Vector2 coord, out CellType cell);
        /// <summary>Resets maze to initial state. </summary>
        void Reset();
    }
}