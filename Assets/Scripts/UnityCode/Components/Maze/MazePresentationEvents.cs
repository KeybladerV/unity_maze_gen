using Build1.PostMVC.Core.MVCS.Events;

namespace Components.Maze
{
    public static class MazePresentationEvents
    {
        public static readonly Event OnMazeDrawStart = new(typeof(MazePresentationEvents), nameof(OnMazeDrawStart));
        public static readonly Event OnMazeDrawEnd  = new(typeof(MazePresentationEvents), nameof(OnMazeDrawEnd));
    }
}