using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;
using Modules.HUD;

namespace Components.HUD.World.Overlay.Commands
{
    [Poolable]
    public sealed class HUDOverlayFadeInCommand : Command<float>
    {
        [Inject] public IHUDController HUDController { get; set; }

        public override void Execute(float duration)
        {
            Retain();

            var view = HUDController.GetHUDView<IHUDFadeOverlayView>(HUDViewType.FadeOverlay);
            view.DoFade(0, duration, Release);
        }
    }
}