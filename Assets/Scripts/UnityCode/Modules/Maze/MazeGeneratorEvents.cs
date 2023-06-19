using Build1.PostMVC.Core.MVCS.Events;
using MazeGenerator;

namespace Modules.Maze
{
    public static class MazeGeneratorEvents
    {
        public static readonly Event<IMaze> OnMazeGenerated  = new(typeof(MazeGeneratorEvents), nameof(OnMazeGenerated));
    }
}