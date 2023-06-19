using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using Modules.HUD;

namespace Components.HUD.World.Overlay
{
    public sealed class HUDFadeOverlayMediator : Mediator
    {
        [Inject] public HUDFadeOverlayView View { get; set; }
        [Inject] public IHUDController           HUDController           { get; set; }

        [Start]
        public void OnStart()
        {
            HUDController.RegisterHUDView(HUDViewType.FadeOverlay, View);
        }

        [OnDestroy]
        public void OnDestroying()
        {
            HUDController.UnregisterHUDView(HUDViewType.FadeOverlay, View);
        }
    }
}