using System;
using UnityEngine;
using Random = System.Random;

namespace MazeGenerator
{
    public class Maze : IMaze
    {
        const WallType resetWallType = WallType.Right | WallType.Left | WallType.Up | WallType.Down;
        
        private readonly int _width;
        private readonly int _length;
        private readonly WallType[,] _maze;
        private Vector2 _entrance;
        private Vector2 _exit;

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

        public Maze(int width, int length)
        {
            _width = width;
            _length = length;
            _maze = new WallType[_width, _length];
            
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