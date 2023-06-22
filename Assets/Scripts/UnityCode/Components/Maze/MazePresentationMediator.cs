using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Components.Character;
using MazeGenerator;
using Modules.Maze;
using UnityEngine;
using Vector2 = MazeGenerator.Vector2;

namespace Components.Maze
{
    public sealed class MazePresentationMediator : Mediator
    {
        [Inject] public MazePresentationView View { get; set; }
        [Inject] public IEventMap EventMap { get; set; }
        [Inject] public IMazePresentationController MazePresentationController { get; set; }

        private IMaze _maze;

        [Start]
        public void OnStart()
        {
            MazePresentationController.RegisterMazeView(View);
            EventMap.Map(MazeGeneratorEvents.OnMazeGenerated, OnMazeGenerated);
            EventMap.Map(CharacterEvents.OnCharacterCoordinatesSet, OnCharacterCoordinatesSet);
            
            EventMap.Map(View, View.OnMazeDrawStart, () => EventMap.Dispatch(MazePresentationEvents.OnMazeDrawStart));
            EventMap.Map(View, View.OnMazeDrawEnd, () => EventMap.Dispatch(MazePresentationEvents.OnMazeDrawEnd));
        }

        [OnDestroy]
        public void OnDestroy()
        {
            EventMap.UnmapAll();
        }

        private void OnCharacterCoordinatesSet(Vector2 coords)
        {
            DrawMazePart(coords);
        }

        private void OnMazeGenerated(IMaze maze)
        {
            _maze = maze;
            
            View.Clear(MazePresentationController.ReleaseCell);
            View.SetMaze(maze);
            
            DrawMazePart(_maze.Entrance);
        }
        
        private void DrawMazePart(Vector2 coordinate)
        {
            var radius = 5; //TODO: get from settings or model TEST PURPOSES ONLY
            int minX = Mathf.Max(0, coordinate.X - radius);
            int maxX = Mathf.Min(_maze.Width, coordinate.X + radius);
            int minY = Mathf.Max(0, coordinate.Y - radius);
            int maxY = Mathf.Min(_maze.Length, coordinate.Y + radius);
            
            View.DrawMazePart(
                new Vector2(minX,maxX),
                new Vector2(minY,maxY),
                MazePresentationController.GetCell,
                MazePresentationController.ReleaseCell
                );
        }
    }
}