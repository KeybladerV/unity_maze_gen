using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.App;
using Build1.PostMVC.Unity.App.Modules.Assets;
using Build1.PostMVC.Unity.App.Modules.Logging;
using UnityEngine;
using AssetBundle = Modules.Assets.AssetBundle;

namespace Components.Screens.AppLoading
{
    public sealed class AppLoadingScreenMediator : Mediator
    {
        [Log(LogLevel.Warning)] public ILog                 Log              { get; set; }
        [Inject]                public AppLoadingScreenView View             { get; set; }
        [Inject]                public IEventMap            EventMap         { get; set; }

        private float _progress;

        [OnEnable]
        public void OnEnable()
        {
            EventMap.Map(AssetsEvent.BundleLoadingSuccess, OnBundleLoaded);
            EventMap.Map(AppLoadingScreenEvent.Complete, OnComplete);

            EventMap.Map(View, View.OnComplete, OnViewComplete);
        }

        [OnDisable]
        public void OnDisabled()
        {
            EventMap.UnmapAll();
        }

        /*
         * Private.
         */

        private void IncrementProgress(float delta)
        {
            _progress = Mathf.Min(_progress + delta, 1);

            Log.Debug(p1 => $"Progress: {p1}", _progress);

            View.SetProgress(_progress);
        }

        /*
         * Event Handlers.
         */

        private void OnBundleLoaded(AssetBundleInfo info)
        {
            if (info == AssetBundle.UI)
                IncrementProgress(0.20F);
        }
        
        private void OnComplete() { IncrementProgress(0.20F); }

        /*
         * View Handlers.
         */

        private void OnViewComplete()
        {
            Log.Debug("OnViewComplete");

            EventMap.Dispatch(AppLoadingScreenEvent.Completed);
        }
    }
}