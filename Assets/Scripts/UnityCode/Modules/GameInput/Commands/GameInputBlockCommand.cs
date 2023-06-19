using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Modules.GameInput.Commands
{
    public sealed class GameInputBlockCommand : Command<GameInputType, object>
    {
        [Inject] public IGameInputController GameInputController { get; set; }
        public override void Execute(GameInputType inputType, object blocker)
        {
            GameInputController.BlockInput(inputType, blocker);
        }
    }
}