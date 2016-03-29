// Abu Kingly
using System;

namespace Revamped
{
    /// <summary>
    /// NOTICE you can store a maximum of 32 bit flags in an enum
    /// </summary>

    // HIGHEST LEVEL
    [Flags]
    public enum AbstractButtonInput
    {
        NONE = 0,
        ATTACK = 1 << 0,
        DODGE = 1 << 1,
        UP = 1 << 2,
        DOWN = 1 << 3,
        LEFT = 1 << 4,
        RIGHT = 1 << 5,
        SWITCH = 1 << 6,
    };

    public enum AbstractAxisInput
    {
        NONE = 0,
        HORIZONTAL = 1 << 0,
        VERTICAL = 1 << 1,
        UP = 1 << 2,
        DOWN = 1 << 3,
        LEFT = 1 << 4,
        RIGHT = 1 << 5,
    };
}