using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using MazeGenerator;
using Modules.Maze;
using Modules.Maze.Impl;

namespace Components.Maze
{
    public sealed class MazeMediator : Mediator
    {
        [Inject] public MazeView View { get; set; }
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        [Inject] public IMazePresentationController MazePresentationController { get; set; }

        [Start]
        public void OnStart()
        {
            MazePresentationController.RegisterMazeView(View);
            Dispatcher.AddListener(MazeGeneratorEvents.OnMazeGenerated, OnMazeGenerated);
        }

        private void OnMazeGenerated(IMaze maze)
        {
            View.Clear(true);
            View.SetMaze(maze);
            
            Dispatcher.Dispatch(MazePresentationEvents.OnMazeDrawStart);
            View.DrawMaze();
            Dispatcher.Dispatch(MazePresentationEvents.OnMazeDrawEnd);
        }
    }
}