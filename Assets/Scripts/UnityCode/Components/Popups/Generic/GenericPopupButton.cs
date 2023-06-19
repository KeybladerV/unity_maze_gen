using System;

namespace Components.Popups.Generic
{
    [Flags]
    public enum GenericPopupButton
    {
        Yes    = 1 << 0,
        Ok     = 1 << 0,
        No     = 1 << 1
    }
}