using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using Modules.HUD.Impl;

namespace Modules.HUD
{
    public sealed class HUDModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IHUDController, HUDController>().ConstructOnStart();
        }
    }
}