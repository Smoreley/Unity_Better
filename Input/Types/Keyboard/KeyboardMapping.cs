// Abu Kingly - 2016
using UnityEngine;

namespace Revamped
{
    public class KeyboardMapping : InputMapping
    {
        public KeyCode[] m_keyCodes;            // Button Mapping
        public AxisMapping[] m_keyToAxis;       // Axis Mapping
    }

    [System.Serializable]
    public struct AxisMapping
    {
        public KeyCode positiveKey;
        public KeyCode negativeKey;
    }
}