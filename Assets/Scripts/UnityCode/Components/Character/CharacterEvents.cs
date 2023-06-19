using Build1.PostMVC.Core.MVCS.Events;

namespace Components.Character
{
    public static class CharacterEvents
    {
        public static Event OnMoveSequenceStart = new(typeof(CharacterEvents), nameof(OnMoveSequenceStart));
        public static Event OnMoveSequenceComplete = new(typeof(CharacterEvents), nameof(OnMoveSequenceComplete));
    }
}