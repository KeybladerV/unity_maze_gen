using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class AStarPathfinder : IPathfinder
    {
        public async Task<Queue<PathNode>> GetRouteAsync(IMaze maze, Vector2 startPos, Vector2 endPos)
        {
            return await Task.Run(() => GetRoute(maze, startPos, endPos));
        }
        
        public Queue<PathNode> GetRoute(IMaze maze, Vector2 startPos, Vector2 endPos)
        {
            PathNode[,] pathNodes = new PathNode[maze.Width, maze.Length];
            HashSet<PathNode> openList = new();
            HashSet<PathNode> closeList = new();

            for (var i = 0; i < maze.Width; i++)
            {
                for (var j = 0; j < maze.Length; j++)
                {
                    var pathNode = new PathNode
                    {
                        Coordinates = new Vector2(i, j),
                        GCost = int.MaxValue,
                        LinkedNode = null
                    };
                    pathNodes[i, j] = pathNode;
                }
            }

            var startNode = pathNodes[startPos.X, startPos.Y];
            var endNode = pathNodes[endPos.X, endPos.Y];
            openList.Clear();
            openList.Add(startNode);
            closeList.Clear();


            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                var current = GetLowestFCostNode(openList);
                if (current.Coordinates == endPos)
                {
                    return new Queue<PathNode>(CalculatePath(endNode));
                }

                openList.Remove(current);
                closeList.Add(current);

                foreach (var neighbour in GetNeighbours(maze, pathNodes, current))
                {
                    if (closeList.Contains(neighbour)) continue;

                    var aGCost = current.GCost + CalculateDistanceCost(current, neighbour);
                    if (aGCost < neighbour.GCost)
                    {
                        neighbour.LinkedNode = current;
                        neighbour.GCost = aGCost;
                        neighbour.HCost = CalculateDistanceCost(neighbour, endNode);

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private List<PathNode> GetNeighbours(IMaze maze, PathNode[,] pathNodes, PathNode current)
        {
            var neighbours = new List<PathNode>();
            if (TryMove(maze, current.Coordinates, MoveDir.Left, out var newPos))
                neighbours.Add(pathNodes[newPos.X, newPos.Y]);

            if (TryMove(maze, current.Coordinates, MoveDir.Right, out newPos))
                neighbours.Add(pathNodes[newPos.X, newPos.Y]);

            if (TryMove(maze, current.Coordinates, MoveDir.Up, out newPos))
                neighbours.Add(pathNodes[newPos.X, newPos.Y]);

            if (TryMove(maze, current.Coordinates, MoveDir.Down, out newPos))
                neighbours.Add(pathNodes[newPos.X, newPos.Y]);

            return neighbours;
        }

        private bool TryMove(IMaze maze, Vector2 currentPos, MoveDir moveDir, out Vector2 newPos)
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
                return !maze[currentPos.X, currentPos.Y].HasFlag(moveDir.ToWallType());
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode> { endNode };
            var current = endNode;
            while (current.LinkedNode != null)
            {
                path.Add(current.LinkedNode);
                current = current.LinkedNode;
            }

            path.Reverse();
            return path;
        }

        private PathNode GetLowestFCostNode(HashSet<PathNode> pathNodes)
        {
            PathNode minFNode = null;
            foreach (var pathNode in pathNodes)
            {
                if (minFNode is null || pathNode.FCost < minFNode.FCost)
                    minFNode = pathNode;
            }

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