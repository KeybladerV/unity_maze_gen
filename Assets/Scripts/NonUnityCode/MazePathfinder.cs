using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public enum MoveDir
    {
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8
    }

    public class MazePathfinder
    {
        private IMaze _maze;

        private Vector2 _target;
        private Vector2 _currentPos;

        private PathNode[,] _pathNodes;

        private List<PathNode> _openList = new List<PathNode>();
        private List<PathNode> _closeList = new List<PathNode>();

        public MazePathfinder(IMaze maze) => SetMaze(maze);

        public void SetMaze(IMaze maze) => _maze = maze;

        private bool TryMove(Vector2 currentPos, MoveDir moveDir, out Vector2 newPos)
        {
            newPos = moveDir switch
            {
                MoveDir.Left => currentPos + Vector2.left,
                MoveDir.Right => currentPos + Vector2.right,
                MoveDir.Up => currentPos + Vector2.up,
                MoveDir.Down => currentPos + Vector2.down,
                _ => currentPos
            };

            try
            {
                return !_maze[currentPos.X, currentPos.Y].HasFlag(moveDir.ToWallType());
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public List<PathNode> GetRoute(Vector2 startPos, Vector2 endPos)
        {
            if (_maze == null)
                throw new NullReferenceException($"{this} requires maze to be set!");
            
            _pathNodes ??= new PathNode[_maze.Width, _maze.Length];

            for (var i = 0; i < _maze.Width; i++)
            {
                for (var j = 0; j < _maze.Length; j++)
                {
                    var pathNode = new PathNode() {Coordinates = new Vector2(i, j), GCost = int.MaxValue};
                    pathNode.CalculateFCost();
                    pathNode.LinkedNode = null;
                    _pathNodes[i, j] = pathNode;
                }
            }

            var startNode = _pathNodes[startPos.X, startPos.Y];
            var endNode = _pathNodes[endPos.X, endPos.Y];
            _openList.Clear();
            _openList.Add(startNode);
            _closeList.Clear();


            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);

            while (_openList.Count > 0)
            {
                var current = GetLowestFCostNode(_openList);
                if (current.Coordinates == endPos)
                {
                    return CalculatePath(endNode);
                }

                _openList.Remove(current);
                _closeList.Add(current);

                foreach (var neighbour in GetNeighbours(current))
                {
                    if (_closeList.Contains(neighbour)) continue;

                    var aGCost = current.GCost + CalculateDistanceCost(current, neighbour);
                    if (aGCost < neighbour.GCost)
                    {
                        neighbour.LinkedNode = current;
                        neighbour.GCost = aGCost;
                        neighbour.HCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if (!_openList.Contains(neighbour))
                        {
                            _openList.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private IEnumerable<PathNode> GetNeighbours(PathNode current)
        {
            if (TryMove(current.Coordinates, MoveDir.Left, out var newPos))
                yield return _pathNodes[newPos.X, newPos.Y];

            if (TryMove(current.Coordinates, MoveDir.Right, out newPos))
                yield return _pathNodes[newPos.X, newPos.Y];

            if (TryMove(current.Coordinates, MoveDir.Up, out newPos))
                yield return _pathNodes[newPos.X, newPos.Y];

            if (TryMove(current.Coordinates, MoveDir.Down, out newPos))
                yield return _pathNodes[newPos.X, newPos.Y];
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode>();
            path.Add(endNode);
            var current = endNode;
            while (current.LinkedNode != null)
            {
                path.Add(current.LinkedNode);
                current = current.LinkedNode;
            }

            path.Reverse();
            return path;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
        {
            var minFNode = pathNodes[0];
            foreach (var pathNode in pathNodes) //No linq, frequent call!
                if (minFNode.FCost > pathNode.FCost)
                    minFNode = pathNode;
            return minFNode;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            var xDis = Math.Abs(a.Coordinates.X - b.Coordinates.X);
            var yDis = Math.Abs(a.Coordinates.Y - b.Coordinates.Y);
            return Math.Abs(xDis - yDis);
        }
    }
}