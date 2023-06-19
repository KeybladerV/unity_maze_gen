using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.Logging;
using UnityEngine;

namespace Components.Screens.LoadingScreen
{
    public sealed class LoadingScreenMediator : Mediator
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }
        
        [Inject] public IEventMap EventMap { get; set; }

        [OnEnable]
        public void OnStart()
        {
            EventMap.Map(LoadingScreenEvent.Complete, OnComplete);
        }

        [OnDestroy]
        public void OnDestroy()
        {
            EventMap.UnmapAll();
        }

        private void OnComplete()
        {
            EventMap.Dispatch(LoadingScreenEvent.Completed);
        }
    }
}