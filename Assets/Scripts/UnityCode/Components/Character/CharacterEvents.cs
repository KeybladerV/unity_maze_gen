using Build1.PostMVC.Core.MVCS.Events;
using UnityEngine;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;
using Vector2 = MazeGenerator.Vector2;

namespace Components.Character
{
    public static class CharacterEvents
    {
        public static Event OnMoveSequenceStart = new(typeof(CharacterEvents), nameof(OnMoveSequenceStart));
        public static Event OnMoveSequenceComplete = new(typeof(CharacterEvents), nameof(OnMoveSequenceComplete));
        public static Event<Transform> OnCharacterCreated = new(typeof(CharacterEvents), nameof(OnCharacterCreated));
        public static Event<Transform> OnCharacterPositionSet = new(typeof(CharacterEvents), nameof(OnCharacterPositionSet));
        public static Event<Vector2> OnCharacterCoordinatesSet = new(typeof(CharacterEvents), nameof(OnCharacterCoordinatesSet)); 
    }
}