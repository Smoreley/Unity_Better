// Abu Kingly - 2016
using UnityEngine;
using System.Collections;

namespace Revamped
{
    public enum buttonState { PRESSED, RELEASED };

    public static class Input
    {

        #region >> Public GetMethods <<

        /// <summary>
        /// Returns the state of a specified button on a specified controller
        /// </summary>
        /// <param name="bttnInpt">ID of button to check against</param>
        /// <param name="plyrID">ID of corresponding player to check against</param>
        /// <returns></returns>
        public static bool GetButton(AbstractButtonInput bttnInpt, PlayerID plyrID = PlayerID.FIRST) {
            InputForm curInForm = GetCurrentState(plyrID);          // Current Input form

            if (GetButtonState(curInForm, AbstractToButtonID(bttnInpt)) == buttonState.PRESSED) { return true; }

            return false;
        }

        /// <summary>
        /// Returns true during the frame the user pressed down the button
        /// </summary>
        /// <param name="bttnInpt">ID of button to check against</param>
        /// <param name="plyrID">ID of corresponding player to check against</param>
        /// <returns></returns>
        public static bool GetButtonDown(AbstractButtonInput bttnInpt, PlayerID plyrID = PlayerID.FIRST) {
            InputForm curInForm = GetCurrentState(plyrID);
            InputForm prevInForm = GetPreviousState(plyrID);

            if ((GetButtonState(curInForm, AbstractToButtonID(bttnInpt)) == buttonState.PRESSED) &&
                    (GetButtonState(prevInForm, AbstractToButtonID(bttnInpt)) == buttonState.RELEASED)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true during the frame the user releases the button
        /// </summary>
        /// <param name="bttnInpt">ID of button to check against</param>
        /// <param name="plyrID">ID of corresponding player to check against</param>
        /// <returns></returns>
        public static bool GetButtonUp(AbstractButtonInput bttnInpt, PlayerID plyrID = PlayerID.FIRST) {
            InputForm curInForm = GetCurrentState(plyrID);
            InputForm prevInForm = GetPreviousState(plyrID);

            if ((GetButtonState(curInForm, AbstractToButtonID(bttnInpt)) == buttonState.RELEASED) &&
                    (GetButtonState(prevInForm, AbstractToButtonID(bttnInpt)) == buttonState.PRESSED)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the value of the axis identified
        /// </summary>
        /// <param name="axisInpt">ID of axis to check against</param>
        /// <param name="plyrID">ID of corresponding player to check against</param>
        /// <returns></returns>
        public static float GetAxis(AbstractAxisInput axisInpt, PlayerID plyrID = PlayerID.FIRST) {
            InputForm curInForm = GetCurrentState(plyrID);
            return GetAxisState(curInForm, AbstractToAxisID(axisInpt));
        }

        #endregion

        #region >> Private GetState Methods <<

        /// <summary>
        /// Returns state of the specified button on the InputFrom
        /// </summary>
        /// <param name="form">Input form to check against</param>
        /// <param name="bttnID">ID of what button to check</param>
        /// <returns></returns>
        private static buttonState GetButtonState(InputForm form, GenericButtonID bttnID) {

            // Bitwise operation to see if it has the button (means it is/was pressed)
            if ((form.m_button & bttnID) != 0) { return buttonState.PRESSED; }

            return buttonState.RELEASED;
        }

        /// <summary>
        /// Returns state of the specified axis on the InputFrom
        /// </summary>
        /// <param name="form">Input form to check against</param>
        /// <param name="axsID">ID of what axis to check</param>
        /// <returns></returns>
        private static float GetAxisState(InputForm form, GenericAxisID axsID) {
            float rtrnVal = 0f;

            if (form.m_axis.Length > 0 && (int)axsID <= form.m_axis.Length && axsID != GenericAxisID.NONE) {
                rtrnVal = form.m_axis[(int)axsID - 1];
            }

            return rtrnVal;
        }

        #endregion

        #region >> GetStates <<

        /// <summary>
        /// Returns current input state
        /// </summary>
        /// <param name="plyrID">ID for a spcific player</param>
        /// <returns></returns>
        private static InputForm GetCurrentState(PlayerID plyrID) {
            return InputManager.GetCurrentState(plyrID);
        }

        /// <summary>
        /// Returns previous input state
        /// </summary>
        /// <param name="plyrID">ID for a spcific player</param>
        /// <returns></returns>
        private static InputForm GetPreviousState(PlayerID plyrID) {
            return InputManager.GetPreviousState(plyrID);
        }

        #endregion

        #region >> Enum Translation <<

        /// <summary>
        /// Converts abstract button input enum into generic ButtonID
        /// </summary>
        /// <param name="bttnID">Enum to translate</param>
        /// <returns></returns>
        public static GenericButtonID AbstractToButtonID(this AbstractButtonInput bttnID) {
            return (GenericButtonID)((bttnID));
        }

        /// <summary>
        /// Converts abstract axis input into generic AxisID enum
        /// </summary>
        /// <param name="axisID">Enum to translate</param>
        /// <returns></returns>
        public static GenericAxisID AbstractToAxisID(this AbstractAxisInput axisID) {
            return (GenericAxisID)((axisID));
        }

        #endregion
    }
}