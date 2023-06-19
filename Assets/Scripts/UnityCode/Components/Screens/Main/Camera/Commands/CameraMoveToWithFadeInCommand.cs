using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Components.HUD.World.Overlay;
using DG.Tweening;
using Modules.HUD;
using UnityEngine;

namespace Components.Screens.Game.Camera.Commands
{
    public sealed class CameraMoveToWithFadeInCommand : Command<Vector2, float>
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        [Inject] public IHUDController HUDController { get; set; }

        public override void Execute(Vector2 destination, float time)
        {
            Retain();

            Dispatcher.Dispatch(CameraEvents.CameraMoveTo, destination + Random.insideUnitCircle.normalized * 10, 0, Ease.Linear);
            
            Dispatcher.AddListenerOnce(CameraEvents.OnMoveComplete, OnMoveComplete);
            Dispatcher.Dispatch(CameraEvents.CameraMoveTo, destination, time, Ease.Unset);
            
            HUDController.GetHUDView<IHUDFadeOverlayView>(HUDViewType.FadeOverlay).DoFade(0, time * 0.25f, null);
        }

        private void OnMoveComplete()
        {
            Release();
        }
    }
}