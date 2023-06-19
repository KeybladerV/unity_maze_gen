namespace Modules.GameInput
{
    public interface IGameInputController
    {
        void BlockInput(GameInputType type, object blocker);
        void UnblockInput(GameInputType type, object blocker);
        
        //DefaultControls Controls { get; }
    }
}