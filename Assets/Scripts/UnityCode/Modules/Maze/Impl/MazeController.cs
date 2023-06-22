using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Mediation;
using MazeGenerator;

namespace Modules.Maze.Impl
{
    public class MazeController : IMazeController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        
        private IMaze _maze;
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            Dispatcher.AddListener(MazeGeneratorEvents.OnMazeGenerated, OnMazeGenerated);
        }
        
        [OnDestroy]
        public void OnDestroy()
        {
            Dispatcher.RemoveListener(MazeGeneratorEvents.OnMazeGenerated, OnMazeGenerated);
        }
        
        public Vector2 GetEntrance()
        {
            return _maze.Entrance;
        }

        public Vector2 GetExit()
        {
            return _maze.Exit;
        }
        
        private void OnMazeGenerated(IMaze maze)
        {
            _maze = maze;
        }
    }
}