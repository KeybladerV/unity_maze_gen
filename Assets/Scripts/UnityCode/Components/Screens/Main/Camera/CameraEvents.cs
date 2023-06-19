using Build1.PostMVC.Core.MVCS.Events;
using DG.Tweening;
using UnityEngine;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Components.Screens.Game.Camera
{
    public static class CameraEvents
    {
        public static readonly Event                 OnPositionChange        = new(typeof(CameraEvents), nameof(OnPositionChange));
        public static readonly Event<float>          OnPositionChangeByTween = new(typeof(CameraEvents), nameof(OnPositionChangeByTween));
        public static readonly Event                 OnZoomComplete          = new(typeof(CameraEvents), nameof(OnZoomComplete));
        public static readonly Event                 OnMoveComplete          = new(typeof(CameraEvents), nameof(OnMoveComplete));
        public static readonly Event<Vector2, float, Ease> CameraMoveTo            = new(typeof(CameraEvents), nameof(CameraMoveTo));
    }
}