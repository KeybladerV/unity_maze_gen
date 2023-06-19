using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Logging;
using Build1.PostMVC.Unity.App.Modules.UI.Screens;
using Components.Screens.AppLoading;
using Components.Screens.LoadingScreen;

namespace Modules.App.Commands
{
    public sealed class AppWaitLoadingScreenCompleteCommand : Command
    {
        [Log(LogLevel.Warning)] public ILog               Log               { get; set; }
        [Inject]                public IEventDispatcher   Dispatcher        { get; set; }
        [Inject]                public IScreensController ScreensController { get; set; }

        public override void Execute()
        {
            if (ScreensController.CheckScreenIsCurrent(Screens.Screen.Loading))
            {
                Retain();
                Dispatcher.AddListenerOnce(LoadingScreenEvent.Completed, Release);
                Dispatcher.Dispatch(LoadingScreenEvent.Complete);
            }
            else
            {
                throw new Exception("Unknown screen when finishing app loading");
            }
        }
    }
}