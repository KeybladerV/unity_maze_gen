using Build1.PostMVC.Core.MVCS.Events;
using Components.Screens.AppLoading;

namespace Components.Screens.LoadingScreen
{
    public static class LoadingScreenEvent
    {
        public static readonly Event Complete  = new(typeof(LoadingScreenEvent), nameof(Complete));
        public static readonly Event Completed = new(typeof(LoadingScreenEvent), nameof(Completed));
    }
}