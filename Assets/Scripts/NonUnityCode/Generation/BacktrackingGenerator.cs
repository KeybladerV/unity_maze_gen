using System;
using System.Collections.Generic;

namespace MazeGenerator
{
    public class BacktrackingGenerator : IMazeGenerator
    {
        private readonly List<NeighbourCell> _neighboursCache = new(4);
        private readonly Random _random;

        public BacktrackingGenerator() => _random = new();
        public BacktrackingGenerator(int seed) => _random = new(seed);

        public void Generate(IMaze maze)
        {
            var opened = new Stack<Vector2>();
            var startPos = new Vector2(_random.Next(0, maze.Width), _random.Next(0, maze.Length));

            maze[startPos.X, startPos.Y] |= WallType.Placed;
            opened.Push(startPos);

            while (opened.Count > 0)
            {
                var currentCell = opened.Pop();
                CacheUnplacedNeighbours(currentCell, maze.Width, maze.Length, maze);

                if (_neighboursCache.Count <= 0) continue;
                
                opened.Push(currentCell);

                var randomWall = _neighboursCache[_random.Next(0, _neighboursCache.Count)];

                var newPos = randomWall.Position;

                maze[currentCell.X, currentCell.Y] &= ~randomWall.SharedWall;
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

        private void CacheUnplacedNeighbours(Vector2 pos, int width, int length, IMaze maze)
        {
            _neighboursCache.Clear();
            
            if (pos.X > 0)
                TryAddNeighbour(new Vector2(pos.X - 1, pos.Y), WallType.Left, maze);

            if (pos.Y > 0)
                TryAddNeighbour(new Vector2(pos.X, pos.Y - 1), WallType.Down, maze);

            if (pos.X < width - 1)
                TryAddNeighbour(new Vector2(pos.X + 1, pos.Y), WallType.Right, maze);

            if (pos.Y < length - 1)
                TryAddNeighbour(new Vector2(pos.X, pos.Y + 1), WallType.Up, maze);
        }

        private void TryAddNeighbour(Vector2 pos, WallType sharedWall, IMaze maze)
        {
            if (!maze[pos.X, pos.Y].HasFlag(WallType.Placed))
            {
                _neighboursCache.Add(new NeighbourCell()
                {
                    Position = pos,
                    SharedWall = sharedWall
                });
            }
        }
    }
}