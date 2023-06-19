using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.UI.Screens;
using Modules.GameInput;
using Modules.HUD;
using Screen = Modules.Screens.Screen;

namespace Components.HUD.World.Overlay
{
    public sealed class HUDOverlayMediator : Mediator
    {
        [Inject] public HUDOverlayView           View                    { get; set; }
        [Inject] public IEventMap                EventMap                { get; set; }
        [Inject] public IGameInputController     GameInputController     { get; set; }
        [Inject] public IScreensController       ScreensController       { get; set; }
        [Inject] public IHUDController           HUDController           { get; set; }

        [Start]
        public void OnStart()
        {
            HUDController.RegisterHUDView(HUDViewType.Overlay, View);
        }

        [OnDestroy]
        public void OnDestroying()
        {
            EventMap.UnmapAll();
            HUDController.UnregisterHUDView(HUDViewType.Overlay, View);
        }
    }
}