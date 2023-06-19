using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Modules.App
{
    public static class AppEvent
    {
        public static readonly Event            Initialize         = new(typeof(AppEvent), nameof(Initialize));
        public static readonly Event            InitializeComplete = new(typeof(AppEvent), nameof(InitializeComplete));
        public static readonly Event<Exception> InitializeFail     = new(typeof(AppEvent), nameof(InitializeFail));

        public static readonly Event            Preload         = new(typeof(AppEvent), nameof(Preload));
        public static readonly Event            PreloadComplete = new(typeof(AppEvent), nameof(PreloadComplete));
        public static readonly Event<Exception> PreloadFail     = new(typeof(AppEvent), nameof(PreloadFail));

        public static readonly Event            Load         = new(typeof(AppEvent), nameof(Load));
        public static readonly Event            LoadFinished = new(typeof(AppEvent), nameof(LoadFinished));
        public static readonly Event<Exception> LoadFail     = new(typeof(AppEvent), nameof(LoadFail));

        public static readonly Event            PostLoad         = new(typeof(AppEvent), nameof(PostLoad));
        public static readonly Event            PostLoadComplete = new(typeof(AppEvent), nameof(PostLoadComplete));
        public static readonly Event<Exception> PostLoadFail     = new(typeof(AppEvent), nameof(PostLoadFail));

        // Game fully loaded and player is presented with game view.
        public static readonly Event Loaded = new(typeof(AppEvent), nameof(Loaded));
    }
}