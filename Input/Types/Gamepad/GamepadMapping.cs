// Abu Kingly - 2016

namespace Revamped
{
    public class GamepadMapping : InputMapping
    {
        public GmpdButton[] m_buttonCodes;
        public GmpdAxis[] m_axisCodes;
        public int gmpdID;                  // What gamepad is this setup tied to.
    }
}