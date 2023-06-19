using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Modules.GameInput
{
    public sealed class GameInputEvent
    {
        public static readonly Event EscPerformed = new(typeof(GameInputEvent), nameof(EscPerformed));
        public static readonly Event JumpPerformed = new(typeof(GameInputEvent), nameof(JumpPerformed));
    }
}