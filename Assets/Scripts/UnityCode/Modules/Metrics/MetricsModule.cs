using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;
using UnityCode.Modules.Metrics.Impl;

namespace UnityCode.Modules.Metrics
{
    public sealed class MetricsModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
            InjectionBinder.Bind<IFPSController, FPSController>().ConstructOnStart();
        }
    }
}