using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Modules.GameInput.Impl;

namespace Modules.GameInput
{
    public sealed class GameInputModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IGameInputController, GameInputController>().ConstructOnStart();
        }
    }
}
