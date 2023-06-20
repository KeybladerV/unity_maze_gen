using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Contexts;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Assets;
using Build1.PostMVC.Unity.App.Modules.Assets.Commands;
using Build1.PostMVC.Unity.App.Modules.Async.Commands;
using Build1.PostMVC.Unity.App.Modules.Logging;
using Build1.PostMVC.Unity.App.Modules.UI.Screens.Commands;
using Modules.App;
using Modules.App.Commands;
using Modules.Assets;
using Modules.Assets.Commands;
using Modules.Character;
using Modules.GameInput;
using Modules.HUD;
using Modules.Maze;
using Modules.Screens.Commands;
using UnityCode.Modules.Metrics;
using UnityEngine;
using AssetBundle = Modules.Assets.AssetBundle;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;
using Screen = Build1.PostMVC.Unity.App.Modules.UI.Screens.Screen;

public sealed class RootModule : Module
{
    [Inject] public ICommandBinder   CommandBinder   { get; set; }

    [PostConstruct]
    public void PostConstruct()
    {
        // Logging.ForceAll = true; // To force all logs regardless of their levels.
        Logging.Print = Debug.isDebugBuild;
        Logging.PrintLevel = Debug.isDebugBuild ? LogLevel.Debug : LogLevel.Warning;
        Logging.Record = true;
        Logging.RecordLevel = Debug.isDebugBuild ? LogLevel.Debug : LogLevel.Warning;
        Logging.SaveToFile = true;
        Logging.FlushThreshold = 32;

        /*
         * Modules.
         */

        AddModule<AssetsModule>();
        AddModule<GameInputModule>();
        AddModule<HUDModule>();
        AddModule<CharacterModule>();
        AddModule<MazeModule>();
        AddModule<PathfindingModule>();
        AddModule<MetricsModule>();

        /*
         * Initializing.
         */

        CommandBinder.Bind(ContextEvent.Started)
                     .To1<DelayCommand>(0.01F)
                     .OnComplete(AppEvent.Initialize)
                     .Once();

        CommandBinder.Bind(AppEvent.Initialize)
                     .To0<AppInitializeCommand>()
                     .OnComplete(AppEvent.InitializeComplete)
                     .OnFail(AppEvent.InitializeFail)
                     .InSequence()
                     .Once(OnceBehavior.UnbindOnComplete);

        CommandBinder.Bind(AppEvent.InitializeFail).To3<AppLoadingFailCommand, Event>(AppEvent.Initialize, "Initialize");

        /*
         * Preloading.
         */

        CommandBinder.Bind(AppEvent.InitializeComplete)
                     .OnComplete(AppEvent.Preload)
                     .Once();

        CommandBinder.Bind(AppEvent.Preload)
                     .To1<AssetBundleTryLoadCommand, AssetBundleInfo>(AssetBundle.AppLoading)
                     .To1<ScreenTryShowCommand, Screen>(Modules.Screens.Screen.Loading)
                     .OnComplete(AppEvent.PreloadComplete)
                     .OnFail(AppEvent.PreloadFail)
                     .InSequence()
                     .Once(OnceBehavior.UnbindOnComplete | OnceBehavior.UnbindOnTriggerFail);

        CommandBinder.Bind(AppEvent.PreloadFail).To3<AppLoadingFailCommand, Event>(AppEvent.Preload, "PreLoad");

        /*
         * Loading.
         */

        CommandBinder.Bind(AppEvent.PreloadComplete)
                     .OnComplete(AppEvent.Load)
                     .Once();

        CommandBinder.Bind(AppEvent.Load)
                     .To1<AssetBundleTryLoadCommand, AssetBundleInfo>(AssetBundle.UI)
                     .To0<AssetsLoadContentGroupsCommand>()
                     .To0<ScreensMenuOpenInBackgroundCommand>()
                     .To0<AppWaitLoadingScreenCompleteCommand>()
                     .OnComplete(AppEvent.LoadFinished)
                     .OnFail(AppEvent.LoadFail)
                     .InSequence()
                     .Once(OnceBehavior.UnbindOnComplete);

        CommandBinder.Bind(AppEvent.LoadFail).To3<AppLoadingFailCommand, Event>(AppEvent.Load, "Loading");

        /*
         * Post Loading.
         */

        CommandBinder.Bind(AppEvent.LoadFinished)
                     .To1<DelayCommand>(0.5f)
                     .OnComplete(AppEvent.PostLoad)
                     .Once();
        
        CommandBinder.Bind(AppEvent.PostLoad)
            .OnComplete(AppEvent.PostLoadComplete)
            .Once(OnceBehavior.UnbindOnComplete | OnceBehavior.UnbindOnTriggerFail);

        CommandBinder.Bind(AppEvent.PostLoadComplete)
                     .To1<ScreenTryHideCommand, Screen>(Modules.Screens.Screen.Loading)
                     .OnComplete(AppEvent.Loaded)
                     .InSequence()
                     .Once();

        CommandBinder.Bind(AppEvent.PostLoadFail).To3<AppLoadingFailCommand, Event>(AppEvent.PostLoad, "PostLoad");

        /*
         * App Shutdown.
         */

        CommandBinder.Bind(ContextEvent.Stopped)
                     .To<AppDeinitializeCommand>();
    }
}