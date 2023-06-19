using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Async;
using Build1.PostMVC.Unity.App.Modules.Logging;
using Build1.PostMVC.Unity.App.Modules.UI.Popups;
using Build1.PostMVC.Unity.App.Modules.UI.Screens;
using Components.Popups.Generic;
using Screen = Modules.Screens.Screen;

namespace Modules.App.Commands
{
    public sealed class AppLoadingFailCommand : Command<Exception, Event, string>
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        [Inject] public IAsyncResolver AsyncResolver { get; set; }
        [Inject] public IPopupController PopupController { get; set; }
        [Inject] public IScreensController ScreensController { get; set; }

        public override void Execute(Exception exception, Event retryEvent, string phase)
        {
            Log.Error((p, e) => $"Exception on {p} phase: {e}", phase, exception);

            Retain();

            var data = GenericPopupData
                .New("Oops...", "Something went wrong\nwhile app was loading.\nPlease try again.")
                .SetTitleStyle(GenericPopupTitleStyle.Error)
                .SetOverlayVisible(false)
                .SetButtons(GenericPopupButton.Yes)
                .SetButtonLabel(GenericPopupButton.Yes, "Try Again")
                .OnClose(_ =>
                {
                    AsyncResolver.Resolve(Dispatcher.Dispatch, Param02);
                    Release();
                });

            PopupController.Open(Popups.Popups.Generic, data);
        }
    }
}