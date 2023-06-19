using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Events;
using Build1.PostMVC.Unity.App.Mediation;
using Build1.PostMVC.Unity.App.Modules.Async;
using Build1.PostMVC.Unity.App.Modules.UI.Popups;

namespace Components.Popups.Generic
{
    public sealed class GenericPopupMediator : Mediator
    {
        [Inject] public GenericPopupView View            { get; set; }
        [Inject] public GenericPopupData Data            { get; set; }
        [Inject] public IEventMap        EventMap        { get; set; }
        [Inject] public IAsyncResolver   AsyncResolver   { get; set; }

        private GenericPopupButton _buttonType;

        [OnEnable]
        public void OnEnabled()
        {
            EventMap.Map(View, GenericPopupView.OnButton, OnButtonClicked);
            EventMap.Map(View, PopupViewDispatcher.OnHidden, OnHidden);

            View.ShowData(Data);
        }

        [OnDisable]
        public void OnDisabled()
        {
            EventMap.UnmapAll();
        }

        /*
         * View Handlers.
         */

        private void OnButtonClicked(GenericPopupButton buttonType)
        {
            _buttonType = buttonType;

            View.Close();
        }

        private void OnHidden()
        {
            AsyncResolver.Resolve((d, b) =>
            {
                d.OnClosed?.Invoke(b);
                d.Dispose();    
            }, Data, _buttonType);
        }
    }
}