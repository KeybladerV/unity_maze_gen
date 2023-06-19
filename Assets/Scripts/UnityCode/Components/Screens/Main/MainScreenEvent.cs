using Build1.PostMVC.Core.MVCS.Events;
using MazeGenerator;
using Models.UI;

namespace Components.Screens.Main
{
    public static class MainScreenEvent
    {
        public static Event<Vector2> OnNewMaze = new(typeof(MainScreenEvent), nameof(OnNewMaze));
        public static Event OnSaveMaze = new(typeof(MainScreenEvent), nameof(OnSaveMaze));
        public static Event<Vector2> OnToCoordinates = new(typeof(MainScreenEvent), nameof(OnToCoordinates));
        public static Event OnToEntrance = new(typeof(MainScreenEvent), nameof(OnToEntrance));
        public static Event OnToExit = new(typeof(MainScreenEvent), nameof(OnToExit));
        public static Event OnToRandom = new(typeof(MainScreenEvent), nameof(OnToRandom));
    }
}