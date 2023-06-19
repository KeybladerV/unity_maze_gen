using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Modules.Character.Impl;

namespace Modules.Character
{
    public sealed class CharacterModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<ICharacterController, CharacterController>().ConstructOnStart();
        }
    }
}