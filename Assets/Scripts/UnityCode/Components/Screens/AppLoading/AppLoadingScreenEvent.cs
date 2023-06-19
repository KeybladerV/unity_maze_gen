using Build1.PostMVC.Core.MVCS.Events;

namespace Components.Screens.AppLoading
{
    public static class AppLoadingScreenEvent
    {
        public static readonly Event Complete  = new(typeof(AppLoadingScreenEvent), nameof(Complete));
        public static readonly Event Completed = new(typeof(AppLoadingScreenEvent), nameof(Completed));
    }
}