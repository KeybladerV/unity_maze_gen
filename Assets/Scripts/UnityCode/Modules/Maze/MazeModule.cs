using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Modules.Maze.Impl;

namespace Modules.Maze
{
    public sealed class MazeModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IMazeGeneratorController, MazeGeneratorController>().ConstructOnStart();
            InjectionBinder.Bind<IMazePresentationController, MazePresentationController>().ConstructOnStart();
        }
    }
}
