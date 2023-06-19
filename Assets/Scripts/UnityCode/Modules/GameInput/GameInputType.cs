using System;

namespace Modules.GameInput
{
    [Flags]
    public enum GameInputType
    {
        General   = 1 << 0,
        PlayerControls = 1 << 1,

        All = General | PlayerControls
    }
}