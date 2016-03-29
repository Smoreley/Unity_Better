// Abu Kingly
using UnityEngine;
using System;

namespace Revamped
{
    [Flags]
    public enum GenericButtonID
    {
        NONE = 0,
        BUTTON01 = 1 << 0,
        BUTTON02 = 1 << 1,
        BUTTON03 = 1 << 2,
        BUTTON04 = 1 << 3,
        BUTTON05 = 1 << 4,
        BUTTON06 = 1 << 5,
        BUTTON07 = 1 << 6,
        BUTTON08 = 1 << 7,
        BUTTON09 = 1 << 8,
        BUTTON10 = 1 << 9,
        BUTTON11 = 1 << 10,
        BUTTON12 = 1 << 11,
        BUTTON13 = 1 << 12,
        BUTTON14 = 1 << 13,
        BUTTON15 = 1 << 14,
        BUTTON16 = 1 << 15,
        BUTTON17 = 1 << 16,
        BUTTON18 = 1 << 17,
        BUTTON19 = 1 << 18,
        BUTTON20 = 1 << 19,
    };

    [Flags]
    public enum GenericAxisID
    {
        NONE = 0,
        AXIS1 = 1 << 0,
        AXIS2 = 1 << 1,
        AXIS3 = 1 << 2,
        AXIS4 = 1 << 3,
        AXIS5 = 1 << 4,
        AXIS6 = 1 << 5,
        AXIS7 = 1 << 6,
        AXIS8 = 1 << 7,
        AXIS9 = 1 << 8,
    };

}