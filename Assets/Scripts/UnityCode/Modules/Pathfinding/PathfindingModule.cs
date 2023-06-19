using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Modules.Maze.Impl;

namespace Modules.Maze
{
    public sealed class PathfindingModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IPathfindingController, PathfindingController>().ConstructOnStart();
        }
    }
}
