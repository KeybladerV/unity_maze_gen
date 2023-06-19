using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Components.Maze;
using MazeGenerator;
using UnityEngine;
using Vector2 = MazeGenerator.Vector2;

namespace Modules.Maze.Impl
{
    public sealed class MazePresentationController : IMazePresentationController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        
        private MazeView _mazeView;
        private IMaze _maze;

        [PostConstruct]
        private void OnPostConstruct()
        {
            Dispatcher.AddListener(MazeGeneratorEvents.OnMazeGenerated, maze => _maze = maze);
        }
        
        public void RegisterMazeView(MazeView view)
        {
            if(_mazeView != null)
                return;
            _mazeView = view;
        }
        
        public Vector2 GetCellByWorldPos(Vector3 worldPos)
        {
            return _mazeView.GetCellByWorldPos(worldPos);
        }

        public Vector3 GetWorldPosByCell(Vector2 cell)
        {
            return _mazeView.GetWorldPosByCell(cell);
        }

        public Vector3 GetWorldPosByCell(int width, int length)
        {
            return _mazeView.GetWorldPosByCell(width, length);
        }
        
        public Vector3 GetEntrancePos()
        {
            return _mazeView.GetWorldPosByCell(_maze.Entrance);
        }
        
        public Vector3 GetExitPos()
        {
            return _mazeView.GetWorldPosByCell(_maze.Exit);
        }
    }
}