using System;
using System.Collections.Generic;

namespace Components.Popups.Generic
{
    public sealed class GenericPopupData
    {
        public string                     Title          { get; }
        public GenericPopupTitleStyle     TitleStyle     { get; private set; } = GenericPopupTitleStyle.Default;
        public string                     Message        { get; }
        public bool                       OverlayVisible { get; private set; } = true;
        public GenericPopupButton         Buttons        { get; private set; } = GenericPopupButton.Ok;
        public Action<GenericPopupButton> OnClosed       { get; private set; }

        public bool HasOkButton => (Buttons & GenericPopupButton.Ok) == GenericPopupButton.Ok;
        public bool HasNoButton => (Buttons & GenericPopupButton.No) == GenericPopupButton.No;

        private Dictionary<GenericPopupButton, string> _buttonLabels;

        public GenericPopupData(string message)
        {
            Message = message;
        }

        public GenericPopupData(string title, string message)
        {
            Title = title;
            Message = message;
        }

        /*
         * Public.
         */

        public GenericPopupData SetTitleStyle(GenericPopupTitleStyle style)
        {
            TitleStyle = style;
            return this;
        }

        public GenericPopupData SetOverlayVisible(bool visible)
        {
            OverlayVisible = visible;
            return this;
        }

        public GenericPopupData SetButtons(GenericPopupButton buttons)
        {
            Buttons = buttons;
            return this;
        }

        public GenericPopupData SetButtonLabel(GenericPopupButton button, string label)
        {
            _buttonLabels ??= new Dictionary<GenericPopupButton, string>();
            _buttonLabels[button] = label;
            return this;
        }

        public string GetButtonLabel(GenericPopupButton button)
        {
            if (_buttonLabels != null && _buttonLabels.TryGetValue(button, out var label))
                return label;

            return button switch
            {
                GenericPopupButton.Yes => "Ok",
                GenericPopupButton.No  => "No",
                _                      => throw new ArgumentOutOfRangeException(nameof(button), button, null)
            };
        }

        public GenericPopupData OnClose(Action<GenericPopupButton> onClose)
        {
            OnClosed = onClose;
            return this;
        }

        public void Dispose()
        {
            OnClosed = null;
        }

        /*
         * Static.
         */

        public static GenericPopupData New(string message)
        {
            return new GenericPopupData(message);
        }

        public static GenericPopupData New(string title, string message)
        {
            return new GenericPopupData(title, message);
        }
    }
}