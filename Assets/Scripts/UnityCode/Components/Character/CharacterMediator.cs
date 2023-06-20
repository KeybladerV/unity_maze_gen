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
        [Inject] public IPathfindingController PathfindingController { get; set; }

        private Vector2 _currentPos;
        private Queue<PathNode> _path;
        
        private Action _movementCompleteCallback;

        [Start]
        private void OnStart()
        {
            _movementCompleteCallback = ProcessMoveAlongPath;
            
            EventMap.Map(MazePresentationEvents.OnMazeDrawEnd, OnMazeDrawEnd);
            EventMap.Map(MainScreenEvent.OnToCoordinates, OnToCoordinates);
            EventMap.Map(MainScreenEvent.OnToEntrance, OnToEntrance);
            EventMap.Map(MainScreenEvent.OnToExit, OnToExit);
            EventMap.Map(MainScreenEvent.OnToRandom, OnToRandom);

            EventMap.Map(View, View.OnMoveEnd, OnMoveEnd);
            
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
            _path = PathfindingController.FindPath(_currentPos, data, PathfindingAlgorithm.AStar);
            ProcessMoveAlongPath();
        }

        private void OnToEntrance()
        {
            EventMap.Dispatch(CharacterEvents.OnMoveSequenceStart);
            _path = PathfindingController.FindPath(
                _currentPos,
                MazePresentationController.GetCellByWorldPos(MazePresentationController.GetEntrancePos()),
                PathfindingAlgorithm.AStar);
            ProcessMoveAlongPath();
        }

        private void OnToExit()
        {
            EventMap.Dispatch(CharacterEvents.OnMoveSequenceStart);
            _path = PathfindingController.FindPath(
                _currentPos,
                MazePresentationController.GetCellByWorldPos(MazePresentationController.GetExitPos()),
                PathfindingAlgorithm.AStar);
            ProcessMoveAlongPath();
        }

        private void OnToRandom()
        {
        }

        private void OnMoveEnd()
        {
        }

        private void OnMazeDrawEnd()
        {
            var entrance = MazePresentationController.GetEntrancePos();
            _currentPos = MazePresentationController.GetCellByWorldPos(entrance);
            View.SetPosition(MazePresentationController.GetEntrancePos());
            
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
            if (_currentPos == nextNode.Coordinates)
            {
                ProcessMoveAlongPath();
            }
            else
            {
                _currentPos = nextNode.Coordinates;
                EventMap.MapOnce(View, View.OnMoveEnd, _movementCompleteCallback);   
                View.MoveTo(MazePresentationController.GetWorldPosByCell(_currentPos), false);
            }
        }
    }
}