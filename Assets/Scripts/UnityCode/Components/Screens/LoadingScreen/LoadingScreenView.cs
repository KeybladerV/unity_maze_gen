using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;

namespace Components.Screens.LoadingScreen
{
    [Mediator(typeof(LoadingScreenMediator))]
    public sealed class LoadingScreenView : UnityViewDispatcher
    {
        
    }
}