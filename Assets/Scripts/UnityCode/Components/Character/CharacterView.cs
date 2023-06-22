using System;
using System.Collections;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using UnityCode;
using UnityEngine;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Components.Character
{
    [Mediator(typeof(CharacterMediator))]
    
    public sealed class CharacterView : UnityViewDispatcher
    {
        public readonly Event OnMoveEnd = new(typeof(CharacterView), nameof(OnMoveEnd));
        
        [SerializeField] private float _speed = 10;

        private bool _isMoving;
        private Action _moveEndCallback;
        private Coroutine _moveCoroutine;
        private Vector3 _targetPosition;

        [Awake]
        private void OnAwake()
        {
            _moveEndCallback = MoveEndCallback;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ResetState(bool isActive)
        {
            _isMoving = false;
            transform.position = Vector3.zero;
            gameObject.SetActive(isActive);
        }
        
        public void MoveTo(Vector3 pos, bool force, bool immediate)
        {
            if(_isMoving && !force)
                return;
            
            if (force && _moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
                SetPosition(_targetPosition);
            }

            if (immediate)
            {
                MoveImmediately(pos);
                return;
            }
            
            _targetPosition = pos;
            _moveCoroutine = StartCoroutine(MoveTo(_targetPosition, _moveEndCallback));
        }

        private IEnumerator MoveTo(Vector3 end, Action onComplete)
        {
            while(!transform.position.Approximately(end))
            {
                transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * _speed);
                yield return null;
            }
            
            transform.position = end;
            _isMoving = false;
            onComplete?.Invoke();
        }
        
        private void MoveImmediately(Vector3 end)
        {
            transform.position = end;
            _isMoving = false;
        }

        private void MoveEndCallback() => Dispatch(OnMoveEnd);
    }
}
