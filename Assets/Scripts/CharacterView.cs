using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace MazeGenerator
{
    /// <summary>Used by Pathfinder prefab to move along the mazes. Currently not used. </summary>
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _speed = 10;

        private Vector2 _currentCoords;
        private bool _isMoving;
        private Sequence _moveSequence;

        private MazeView _mazeView;
        private MazePathfinder _pathfinder;

        private void Start()
        {
            _mazeView = GetComponentInParent<MazeView>();
            _pathfinder = new MazePathfinder(_mazeView.Maze);
            _currentCoords = _mazeView.Maze.Entrance;
            _isMoving = false;
            transform.position = _mazeView.GetCellWorldPos(_currentCoords.X, _currentCoords.Y);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isMoving)
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out RaycastHit hit);
                if (hit.collider != null)
                {
                    var pointCell = _mazeView.GetCellByWorldPos(hit.point);
                    _isMoving = true;
                     var path = _pathfinder.GetRoute(_currentCoords, pointCell);
                     if (path != null)
                         MoveByPath(path);
                     else
                         _isMoving = false;
                }
            }
        }

        private void MoveByPath(List<PathNode> path)
        {
            var route = path.Skip(1).Select(p => p.Coordinates);
            _moveSequence = DOTween.Sequence();
            foreach (var node in route)
            {
                var nextCoords = _mazeView.GetCellWorldPos(node);
                _moveSequence.Append(transform.DOMove(nextCoords, 1f / _speed).OnComplete(() => _currentCoords = node));
            }

            _moveSequence.OnComplete(() => _isMoving = false);
        }
    }
}