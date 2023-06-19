using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;
using Components.HUD.World.Overlay;

namespace Modules.HUD.Commands
{
    [Poolable]
    public sealed class HUDSetOverlayStateCommand : Command<HUDOverlayState>
    {
        [Inject] public IHUDController HUDController { get; set; }
        
        public override void Execute(HUDOverlayState state)
        {
            HUDController.SetViewState(HUDViewType.Overlay, state);
        }
    }
}