using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace MazeGenerator
{
    /// <summary>Used by Pathfinder prefab to move along the mazes. Currently not used. </summary>
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private MazeView _mazeView;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _speed = 10;

        private Vector2 _currentCoords;
        private bool _isMoving;
        private Sequence _moveSequence;
        private IPathfinder _pathfinder;
        
        public Action<bool> OnMoveStateChanged;

        public void SetPositionToEntrance()
        {
            _currentCoords = _mazeView.Maze.Entrance;
            transform.position = _mazeView.GetCellWorldPos(_currentCoords.X, _currentCoords.Y);
        }
            
        public void SetMazeView(MazeView mazeView) => _mazeView = mazeView;

        private void Start()
        {
            _pathfinder = new AStarPathfinder();
            _currentCoords = _mazeView.Maze.Entrance;
            _isMoving = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isMoving && _mazeView is not null)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out RaycastHit hit);
                if (hit.collider != null)
                {
                    var pointCell = _mazeView.GetCellByWorldPos(hit.point);
                    MoveTo(pointCell);
                }
            }
        }
        
        /// <summary>
        /// Move to the cell with given coordinates
        /// </summary>
        /// <param name="x">Cell width coordinate</param>
        /// <param name="y">Cell height coordinate</param>
        public void MoveTo(Vector2 coords)
        {
            if(_isMoving)
                return;
            
            _isMoving = true;
            OnMoveStateChanged?.Invoke(_isMoving);
            var path = _pathfinder.GetRoute(_mazeView.Maze, _currentCoords, coords);
            if (path != null)
                MoveByPath(path);
            else
                _isMoving = false;
        }

        private void MoveByPath(Queue<PathNode> path)
        {
            path.Dequeue(); //First node is current node
            _moveSequence = DOTween.Sequence();
            
            while (path.Count > 0)
            {
                var pathNode = path.Dequeue();
                var nextCoords = _mazeView.GetCellWorldPos(pathNode.Coordinates);
                _moveSequence.Append(transform.DOMove(nextCoords, 1f / _speed).OnComplete(() => _currentCoords = pathNode.Coordinates));
            }

            _moveSequence.OnComplete(() =>
            {
                _isMoving = false;
                OnMoveStateChanged?.Invoke(_isMoving);
            });
        }
    }
}