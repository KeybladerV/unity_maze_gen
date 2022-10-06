using System;
using System.Collections.Generic;

namespace MazeGenerator
{
    [Flags]
    public enum WallType
    {
        Up = 4,
        Down = 8,
        Left = 1,
        Right = 2,
        Placed = 128
    }

    public struct NearWall
    {
        public Vector2 Position;
        public WallType SharedWall;
    }

    public class MazeMaker
    {
        private static readonly List<NearWall> _unplacedWallsCache = new List<NearWall>();
        private static readonly Random _random = new Random();
        
        public static void MakeMaze(IMaze maze)
        {
            var opened = new Stack<Vector2>();
            var pos = new Vector2(_random.Next(0, maze.Width), _random.Next(0, maze.Length));

            maze[pos.X, pos.Y] |= WallType.Placed;
            opened.Push(pos);

            while (opened.Count > 0)
            {
                var curr = opened.Pop();
                CollectUnplacedWalls(curr, maze.Width, maze.Length, maze);

                if (_unplacedWallsCache.Count <= 0) continue;
                
                opened.Push(curr);

                var randomWall = _unplacedWallsCache[_random.Next(0, _unplacedWallsCache.Count)];

                var newPos = randomWall.Position;

                maze[curr.X, curr.Y] &= ~randomWall.SharedWall;
                maze[newPos.X, newPos.Y] &= ~randomWall.SharedWall.GetOpposite();
                maze[newPos.X, newPos.Y] |= WallType.Placed;

                opened.Push(newPos);
            }

            if (maze.Width > maze.Length)
            {
                maze.SetEntrance(new Vector2(0, _random.Next(0, maze.Length)));
                maze.SetExit(new Vector2(maze.Width - 1, _random.Next(0, maze.Length)));
            }
            else
            {
                maze.SetEntrance(new Vector2(_random.Next(0, maze.Width),0));
                maze.SetExit(new Vector2(_random.Next(0, maze.Width), maze.Length - 1));
            }

        }

        private static void CollectUnplacedWalls(Vector2 pos, int width, int length, IMaze maze)
        {
            _unplacedWallsCache.Clear();
            
            if (pos.X > 0)
                TryAddNearWall(new Vector2(pos.X - 1, pos.Y), WallType.Left, maze);

            if (pos.Y > 0)
                TryAddNearWall(new Vector2(pos.X, pos.Y - 1), WallType.Down, maze);

            if (pos.X < width - 1)
                TryAddNearWall(new Vector2(pos.X + 1, pos.Y), WallType.Right, maze);

            if (pos.Y < length - 1)
                TryAddNearWall(new Vector2(pos.X, pos.Y + 1), WallType.Up, maze);
        }

        private static void TryAddNearWall(Vector2 pos, WallType sharedWall, IMaze maze)
        {
            if (!maze[pos.X, pos.Y].HasFlag(WallType.Placed))
            {
                _unplacedWallsCache.Add(new NearWall()
                {
                    Position = pos,
                    SharedWall = sharedWall
                });
            }
        }
    }
}