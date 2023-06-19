using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Components.Character;
using Components.Maze;

namespace Components.Screens.Main
{
    public sealed class MainScreenMediator : Mediator
    {
        [Inject] public MainScreenView View { get; set; }
        [Inject] public IEventMap EventMap { get; set; }

        [Start]
        private void OnStart()
        {
            EventMap.Map(View, View.OnNewMaze, () => EventMap.Dispatch(MainScreenEvent.OnNewMaze, View.GetMazeGenData()));
            EventMap.Map(View, View.OnSaveMaze, () => EventMap.Dispatch(MainScreenEvent.OnSaveMaze));
            EventMap.Map(View, View.OnToCoordinates, () => EventMap.Dispatch(MainScreenEvent.OnToCoordinates, View.GetPathfindingData()));
            EventMap.Map(View, View.OnToEntrance, () => EventMap.Dispatch(MainScreenEvent.OnToEntrance));
            EventMap.Map(View, View.OnToExit, () => EventMap.Dispatch(MainScreenEvent.OnToExit));
            EventMap.Map(View, View.OnToRandom, () => EventMap.Dispatch(MainScreenEvent.OnToRandom));
            
            EventMap.Map(MazePresentationEvents.OnMazeDrawStart, () => View.SetState(MainScreenState.DisableAll));
            EventMap.Map(MazePresentationEvents.OnMazeDrawEnd, () => View.SetState(MainScreenState.EnableAll));
            
            EventMap.Map(CharacterEvents.OnMoveSequenceStart, () => View.SetState(MainScreenState.DisableAll));
            EventMap.Map(CharacterEvents.OnMoveSequenceComplete, () => View.SetState(MainScreenState.EnableAll));
        }
        
        [OnDestroy]
        private void OnDestroying()
        {
            EventMap.UnmapAll();
        }

        [OnDestroy]
        private void OnDestroy()
        {
            EventMap.UnmapAll();
        }
    }
}
