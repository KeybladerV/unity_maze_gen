using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Modules.UI.Screens;

namespace Modules.Screens.Commands
{
    public sealed class ScreensMenuOpenInBackgroundCommand : Command
    {
        [Inject] public IEventMap          EventMap          { get; set; }
        [Inject] public IScreensController ScreensController { get; set; }

        public override void Execute()
        {
            if (ScreensController.CheckScreenIsActive(Screen.MainScreen))
                return;

            ScreensController.Show(Screen.MainScreen, ScreenBehavior.OpenInBackground);
        }
    }
}