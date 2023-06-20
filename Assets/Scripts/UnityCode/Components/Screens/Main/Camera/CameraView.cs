using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using DG.Tweening;
using UnityEngine;

namespace Components.Screens.Game.Camera
{
    [Mediator(typeof(CameraMediator))]
    public sealed class CameraView : UnityViewDispatcher
    {
        [SerializeField] private float zoomDuration = 0.35F;
        [SerializeField] private Ease zoomEasing = Ease.OutCubic;
        [SerializeField] private float cullingValue = 5;
        [SerializeField] private float[] zoomValues = { 3.25F, 5F, 10F, 15F };
        [SerializeField] private float followSpeed = 5f;

        [Header("Parts"), SerializeField] private UnityEngine.Camera gameCamera;

        private Tweener _tweenInertia;
        private Tweener _tweenMove;
        private Tweener _tweenZoom;

        private Bounds _bounds;
        private Vector2 _dragVelocity;
        private Vector2 _dragDelta;

        private bool _isPinch;
        private bool _isCloseScale;

        private Transform _lockOnTarget;
        private bool _isLockedOn;

        [OnDisable]
        public void OnDisabled()
        {
            StopInertia();
            StopMove();
            StopZoom();
        }

        private void LateUpdate()
        {
            if (_isLockedOn && _tweenMove?.active != true)
            {
                var targetPosition = _lockOnTarget.position;
                targetPosition.y = gameCamera.transform.position.y;

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
            }
        }

        /*
         * Public.
         */

        public void UpdateBounds(Bounds bounds)
        {
            StopInertia();

            _bounds = bounds;

            ClampCamera();
        }

        /*
         * Move.
         */

        public void MoveTo(Vector2 position, float timeToMove, Ease ease)
        {
            var position3 = new Vector3(position.x, gameCamera.transform.position.y, position.y);

            if (timeToMove <= 0)
            {
                gameCamera.transform.position = position3;

                StopMove();
                ClampCamera();
                Dispatch(CameraEvents.OnMoveComplete);
                return;
            }

            _tweenMove = gameCamera.transform.DOMove(position3, timeToMove)
                .SetEase(ease)
                .OnUpdate(() =>
                {
                    // TODO: Try clamping. If clamp was applied, kill the animation.

                    Dispatch(CameraEvents.OnPositionChangeByTween, _tweenMove.ElapsedPercentage());
                })
                .OnComplete(() =>
                {
                    //ClampCamera();
                    Dispatch(CameraEvents.OnMoveComplete);
                });
        }

        private void StopMove()
        {
            if (_tweenMove?.active != true)
                return;
            _tweenMove.Kill();
            _tweenMove = null;
        }

        /*
         * Camera.
         */

        private void ScrollCamera(Vector2 canvasDelta)
        {
            // transform canvas delta (0 to +size) to viewport delta (-1 to +1)
            var viewportDelta = new Vector3(-canvasDelta.x / Screen.width,
                -canvasDelta.y / Screen.height,
                0f) * 2;

            // transform viewport delta (-1 to +1) to world camera delta (leftmost visible world point to rightmost visible world point)
            var worldDelta = gameCamera.projectionMatrix.inverse * viewportDelta;

            // apply delta to camera
            gameCamera.transform.position += (Vector3)worldDelta;

            ClampCamera();
            Dispatch(CameraEvents.OnPositionChange);
        }

        private void ClampCamera()
        {
            var delta = gameCamera.projectionMatrix.inverse * new Vector3(-0.5f, -0.5f, 0f);

            var min = new Vector3(_bounds.min.x - delta.x,
                _bounds.min.y - delta.y,
                0f);

            var max = new Vector3(_bounds.max.x + delta.x,
                _bounds.max.y + delta.y,
                0f);

            max.y = min.y > max.y ? min.y : max.y;
            max.x = min.x > max.x ? min.x : max.x;

            var pos = gameCamera.transform.position;
            gameCamera.transform.position = new Vector3(Mathf.Clamp(pos.x, min.x, max.x),
                pos.z,
                Mathf.Clamp(pos.y, min.y, max.y));
        }

        /*
         * Zoom.
         */

        public float GetZoomValue()
        {
            return gameCamera.orthographicSize;
        }

        public void Zoom()
        {
            StopInertia();
            StopZoom();

            var zoomValue = zoomValues[^1];
            var zoomValueCurrent = GetZoomValue();

            for (var i = zoomValues.Length - 1; i > 0; i--)
            {
                var valueHigher = zoomValues[i];
                var valueLower = zoomValues[i - 1];

                if (zoomValueCurrent <= valueHigher && zoomValueCurrent > valueLower)
                {
                    zoomValue = valueLower;
                    break;
                }
            }

            UpdateCullingMask(zoomValue);

            _tweenZoom = gameCamera.DOOrthoSize(zoomValue, zoomDuration)
                .SetEase(zoomEasing)
                .OnUpdate(ClampCamera)
                .OnComplete(() => { Dispatch(CameraEvents.OnZoomComplete); });
        }

        private void StopZoom()
        {
            if (_tweenZoom?.active != true)
                return;
            _tweenZoom.Kill();
            _tweenZoom = null;
        }

        public void SetCameraZoomValue(float zoomValue)
        {
            zoomValue = Mathf.Clamp(zoomValue, zoomValues[0], zoomValues[^1]);

            UpdateCullingMask(zoomValue);

            gameCamera.orthographicSize = zoomValue;
        }

        private void UpdateCullingMask(float zoomValue)
        {
            var mask = gameCamera.cullingMask;

            if (zoomValue <= cullingValue)
            {
                mask &= ~(1 << LayerMask.NameToLayer("FarCameraScale"));
                mask |= (1 << LayerMask.NameToLayer("CloseCameraScale"));
            }
            else
            {
                mask |= 1 << LayerMask.NameToLayer("FarCameraScale");
                mask &= ~(1 << LayerMask.NameToLayer("CloseCameraScale"));
            }

            gameCamera.cullingMask = mask;
        }

        /*
         * Inertia.
         */

        private void StopInertia()
        {
            if (_tweenInertia?.active != true)
                return;
            _tweenInertia.Kill();
            _tweenInertia = null;
        }

        public void LockOn(Transform target)
        {
            if (_isLockedOn && _lockOnTarget == target)
                return;

            MoveTo(new Vector2(target.position.x, target.position.z), 0.5f, Ease.Linear);
            _lockOnTarget = target;

            _isLockedOn = true;
        }
    }
}