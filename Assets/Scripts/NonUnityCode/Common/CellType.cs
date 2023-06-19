using System;

namespace MazeGenerator
{
    [Flags]
    public enum CellType
    {
        Empty = 0, /// <summary> Marks cell is completely empty and must not be processes or used. </summary>
        
        Left = 1 << 0, /// <summary> Marks cell has left wall. </summary>
        Right = 1 << 1, /// <summary> Marks cell has right wall. </summary>
        Up = 1 << 2, /// <summary> Marks cell has up wall. </summary>
        Down = 1 << 3, /// <summary> Marks cell has down wall. </summary>
        
        Placed = 1 << 4, /// <summary> Marks cell is placed in maze. </summary>
        
        AllWalls = Left | Right | Up | Down, /// <summary> Marks cell has all walls. </summary>
    }
}