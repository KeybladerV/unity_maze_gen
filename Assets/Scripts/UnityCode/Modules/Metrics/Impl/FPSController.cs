using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.Update;

namespace UnityCode.Modules.Metrics.Impl
{
    public sealed class FPSController : IFPSController
    {
        [Inject] public IUpdateController UpdateController { get; set; }
        
        public float FPS => _fps;
        
        private float _fps;
        
        [PostConstruct]
        public void PostConstruct()
        {
            UpdateController.SubscribeForUpdate(OnUpdate);
        }
        
        [OnDestroy]
        public void OnDestroy()
        {
            UpdateController.Unsubscribe(OnUpdate);
        }

        private void OnUpdate(float deltaTime)
        {
            _fps = 1 / deltaTime;
        }
    }
}