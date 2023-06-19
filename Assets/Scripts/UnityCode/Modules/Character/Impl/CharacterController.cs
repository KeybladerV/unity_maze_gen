using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;

namespace Modules.Character.Impl
{
    public sealed class CharacterController : ICharacterController
    {
        [Inject] public IEventMap EventMap { get; set; }
        

        //private Model.Character.Character _character;

        [PostConstruct]
        private void PostConstruct()
        {
        }

        [OnDestroy]
        private void OnDestroy()
        {
            EventMap.UnmapAll();
        }

        // public void SetCharacter(Model.Character.Character character)
        // {
        //     _character = character;
        // }
    }
}