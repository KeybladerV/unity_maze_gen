using System;
using Build1.PostMVC.Core.MVCS.Mediation;

namespace Modules.HUD
{
    public interface IHUDController
    {
        void RegisterHUDView(HUDViewType type, IView view);
        void UnregisterHUDView(HUDViewType type, IView view);

        T GetHUDView<T>(HUDViewType type) where T : class;

        void SetViewState<T>(HUDViewType type, T state) where T : Enum;
    }
}