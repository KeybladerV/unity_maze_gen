using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using MazeGenerator;

namespace Modules.Maze.Impl
{
    public enum PathfindingAlgorithm
    {
        AStar,
    }
    
    public sealed class PathfindingController : IPathfindingController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        
        private Dictionary<PathfindingAlgorithm, IPathfinder> _pathfinders;
        private IMaze _maze;

        [PostConstruct]
        private void PostConstruct()
        {
            _pathfinders = new Dictionary<PathfindingAlgorithm, IPathfinder>();
            Dispatcher.AddListener(MazeGeneratorEvents.OnMazeGenerated, OnMazeGenerated);
        }


        public Queue<PathNode> FindPath(Vector2 start, Vector2 end, PathfindingAlgorithm algorithm)
        {
            if(!_pathfinders.TryGetValue(algorithm, out var pathfinder))
                _pathfinders[algorithm] = pathfinder = CreatePathfinder(algorithm);
            
            return pathfinder.GetRoute(_maze, start, end);
        }

        private IPathfinder CreatePathfinder(PathfindingAlgorithm algorithm)
        {
            return algorithm switch
            {
                PathfindingAlgorithm.AStar => new AStarPathfinder(),
                _ => throw new System.NotImplementedException()
            };
        }
        
        private void OnMazeGenerated(IMaze maze)
        {
            _maze = maze;
        }
    }
}
