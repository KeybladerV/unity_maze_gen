using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using DG.Tweening;
using UnityEngine;

namespace Components.Screens.Game.Camera.Commands
{
    [Poolable]
    public sealed class CameraMoveToCommand : Command<Vector2, float>
    {
        [Inject] public IEventDispatcher Dispatcher      { get; set; }

        public override void Execute(Vector2 position, float time)
        {
            Retain();

            Dispatcher.AddListenerOnce(CameraEvents.OnMoveComplete, OnMoveComplete);
            Dispatcher.Dispatch(CameraEvents.CameraMoveTo, position, time, Ease.Unset);
        }

        private void OnMoveComplete()
        {
            Release();
        }
    }
}