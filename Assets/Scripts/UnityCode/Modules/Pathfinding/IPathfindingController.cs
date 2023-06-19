using System.Collections.Generic;
using MazeGenerator;
using Modules.Maze.Impl;

namespace Modules.Maze
{
    public interface IPathfindingController
    {
        Queue<PathNode> FindPath(Vector2 start, Vector2 end, PathfindingAlgorithm algorithm);
    }
}