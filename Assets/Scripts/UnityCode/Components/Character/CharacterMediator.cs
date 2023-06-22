using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Components.Maze;
using Components.Screens.Main;
using MazeGenerator;
using Modules.Maze;
using Modules.Maze.Impl;

namespace Components.Character
{
    public sealed class CharacterMediator : Mediator
    {
        [Inject] public CharacterView View { get; set; }
        [Inject] public IEventMap EventMap { get; set; }
        [Inject] public IMazePresentationController MazePresentationController { get; set; }
        [Inject] public IMazeController MazeController { get; set; }
        [Inject] public IPathfindingController PathfindingController { get; set; }

        private Vector2 _currentCell;
        private Queue<PathNode> _path;
        
        private Action _movementCompleteCallback;

        [Start]
        private void OnStart()
        {
            _movementCompleteCallback = ProcessMoveAlongPath;
            
            EventMap.Map(MazeGeneratorEvents.OnMazeGenerated, OnMazeGenerated);
            EventMap.Map(MainScreenEvent.OnToCoordinates, OnToCoordinates);
            EventMap.Map(MainScreenEvent.OnToEntrance, OnToEntrance);
            EventMap.Map(MainScreenEvent.OnToExit, OnToExit);
            EventMap.Map(MainScreenEvent.OnToRandom, OnToRandom);
            
            EventMap.Dispatch(CharacterEvents.OnCharacterCreated, View.transform);
        }
        
        [OnDestroy]
        private void OnDestroy()
        {
            EventMap.UnmapAll();
        }

        private void OnToCoordinates(Vector2 data)
        {
            EventMap.Dispatch(CharacterEvents.OnMoveSequenceStart);
            _path = PathfindingController.FindPath(_currentCell, data, PathfindingAlgorithm.AStar);
            ProcessMoveAlongPath();
        }

        private void OnToEntrance()
        {
            EventMap.Dispatch(CharacterEvents.OnMoveSequenceStart);
            _path = PathfindingController.FindPath(
                _currentCell,
                MazeController.GetEntrance(),
                PathfindingAlgorithm.AStar);
            ProcessMoveAlongPath();
        }

        private void OnToExit()
        {
            EventMap.Dispatch(CharacterEvents.OnMoveSequenceStart);
            _path = PathfindingController.FindPath(
                _currentCell,
                MazeController.GetExit(),
                PathfindingAlgorithm.AStar);
            ProcessMoveAlongPath();
        }

        private void OnToRandom()
        {
        }

        private void OnMazeGenerated(IMaze maze)
        {
            _currentCell = maze.Entrance;
            View.SetPosition(MazePresentationController.GetWorldPosByCell(_currentCell));
            
            EventMap.Dispatch(CharacterEvents.OnCharacterCoordinatesSet, _currentCell);
            EventMap.Dispatch(CharacterEvents.OnCharacterPositionSet, View.transform);
        }

        private void ProcessMoveAlongPath()
        {
            if (_path.Count <= 0)
            {
                EventMap.Dispatch(CharacterEvents.OnMoveSequenceComplete);
                return;
            }
            
            var nextNode = _path.Dequeue();
            if (_currentCell == nextNode.Coordinates)
            {
                ProcessMoveAlongPath();
            }
            else
            {
                _currentCell = nextNode.Coordinates;
                EventMap.Dispatch(CharacterEvents.OnCharacterCoordinatesSet, _currentCell);
                
                EventMap.MapOnce(View, View.OnMoveEnd, _movementCompleteCallback);   
                View.MoveTo(MazePresentationController.GetWorldPosByCell(_currentCell), false, false);
            }
        }
    }
}