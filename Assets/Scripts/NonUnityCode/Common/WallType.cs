using System;

namespace MazeGenerator
{
    [Flags]
    public enum WallType
    {
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3,
        
        Placed = 1 << 4,
        
        AllWalls = Left | Right | Up | Down
    }
}