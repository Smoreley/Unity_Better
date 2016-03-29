// Abu Kingly - 2016
using UnityEngine;

namespace Revamped
{
    public class KeyboardInput
    {
        public InputForm GetInputState(KeyboardMapping plyrKybrdSttngs) {
            InputForm rtnForm = new InputForm();

            KeyCode[] kyCodes = plyrKybrdSttngs.m_keyCodes;

            // Get buttons 
            for (int i = 0; i < kyCodes.Length; i++) {
                if (GetButtonStates(kyCodes[i])) {
                    rtnForm.m_button |= (GenericButtonID)((int)1 << i);
                }
            }

            rtnForm.m_axis = new float[plyrKybrdSttngs.m_keyToAxis.Length];
            // Get axis
            for (int j = 0; j < plyrKybrdSttngs.m_keyToAxis.Length; j++) {
                rtnForm.m_axis[j] = GetAxisStates(plyrKybrdSttngs.m_keyToAxis[j]);
            }

            return rtnForm;
        }

        #region >> Private Get Methods <<

        private bool GetButtonStates(KeyCode kCode) {
            return UnityEngine.Input.GetKey(kCode);
        }

        private int GetAxisStates(AxisMapping axsMapping) {
            int rtrnInt = 0;
            // check both pos and neg. add them toegeather to get the final value
            if(GetButtonStates(axsMapping.positiveKey)) { rtrnInt += 1; } 
            if(GetButtonStates(axsMapping.negativeKey)) { rtrnInt -= 1; }

            return rtrnInt;
        }

        #endregion
    }
}