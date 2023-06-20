using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Components.Character;
using DG.Tweening;
using UnityEngine;

namespace Components.Screens.Game.Camera
{
    public sealed class CameraMediator : Mediator
    {
        [Inject] public CameraView            View                 { get; set; }
        [Inject] public IEventMap             EventMap             { get; set; }

        [Awake]
        public void OnAwake()
        {
            EventMap.Map(View, CameraEvents.OnPositionChangeByTween, OnCameraPositionChangeByTween);
            EventMap.Map(CameraEvents.CameraMoveTo, OnCameraMoveToEvent);
            EventMap.Map(View,CameraEvents.OnMoveComplete, OnCameraMoveComplete);

            EventMap.Map(CharacterEvents.OnCharacterCreated, TryLockOnCharacter);
            EventMap.Map(CharacterEvents.OnCharacterPositionSet, TryLockOnCharacter);

            OnSettingLoadSuccess();
        }

        private void TryLockOnCharacter(Transform character)
        {
            View.LockOn(character);
        }

        [OnDestroy]
        public void OnDestroy()
        {
            EventMap.UnmapAll();
        }

        private void OnSettingLoadSuccess()
        {
            View.SetCameraZoomValue(10);
        }

        /*
         * Event Handlers.
         */

        private void OnMapBoundsChanged(Bounds bounds)
        {
            View.UpdateBounds(bounds);
        }
        
        private void OnCameraMoveToEvent(Vector2 position, float time, Ease ease)
        {
            View.MoveTo(position, time, ease);
        }

        private void OnCameraMoveComplete()
        {
            EventMap.Dispatch(CameraEvents.OnMoveComplete);
        }

        private void OnCameraPositionChangeByTween(float percentage)
        {
            EventMap.Dispatch(CameraEvents.OnPositionChangeByTween, percentage);
        }
    }
}