using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;

namespace Modules.Character.Impl
{
    public sealed class CharacterController : ICharacterController
    {
        [Inject] public IEventMap EventMap { get; set; }

        [PostConstruct]
        private void PostConstruct()
        {
        }

        [OnDestroy]
        private void OnDestroy()
        {
            EventMap.UnmapAll();
        }
    }
}