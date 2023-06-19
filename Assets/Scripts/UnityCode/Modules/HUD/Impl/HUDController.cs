using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Modules.Logging;
using Build1.PostMVC.Unity.App.Modules.UI.HUD;
using Build1.PostMVC.Unity.App.Modules.UI.Screens;

namespace Modules.HUD.Impl
{
    public sealed class HUDController : HUDControllerBase, IHUDController
    {
        [Log(LogLevel.Warning)] public ILog                Log                { get; set; }
        [Inject]                public IEventDispatcher    Dispatcher         { get; set; }
        [Inject]                public IInjectionBinder    InjectionBinder    { get; set; }

        private Dictionary<HUDViewType, IView> _views;

        [PostConstruct]
        public void PostConstruct()
        {
            Dispatcher.AddListener(ScreenEvent.Shown, OnScreenShown);
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Dispatcher.RemoveListener(ScreenEvent.Shown, OnScreenShown);
        }

        /*
         * Public.
         */

        public void RegisterHUDView(HUDViewType type, IView view)
        {
            _views ??= new Dictionary<HUDViewType, IView>();
            _views.Add(type, view);
        }

        public void UnregisterHUDView(HUDViewType type, IView view)
        {
            if (_views != null && _views[type] == view)
                _views?.Remove(type);
        }

        public T GetHUDView<T>(HUDViewType type) where T : class
        {
            if (_views.TryGetValue(type, out var view))
                return (T)view;
            return null;
        }

        public void SetViewState<T>(HUDViewType type, T state) where T : Enum
        {
            if (_views == null || !_views.TryGetValue(type, out var view))
                return;

            switch (type)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /*
         * HUD.
         */

        private void UpdateHUD(ScreenBase screen)
        {
            Log.Debug(s => $"Updating HUD... current screen: {s}", screen);

            if (screen == Screens.Screen.Loading)
                return;
        }

        /*
         * Event Handlers.
         */

        private void OnScreenShown(ScreenBase screen)
        {
            UpdateHUD(screen);
        }
    }
}