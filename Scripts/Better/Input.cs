// Abu Kingly
using UnityEngine;
using XInputDotNetPure;

namespace Better
{
    public enum GmpdButton { A, B, X, Y, Start, Back, LBumper, RBumper, LStick, RStick, DPadUp, DPadDown, DPadLeft, DPadRight };

    public enum GmpdAxis { LStickX, LStickY, RStickX, RStickY, LTrigger, RTrigger, DpadX, DpadY}

    public enum ControllerNumber { All = 0, First = 1, Second = 2, Third = 3, Fourth = 4 }

    public static class Input
    {

        /// <summary>
        /// Current state of the gamepad
        /// </summary>
        private static GamePadState[] currInputControllers = new GamePadState[4];

        /// <summary>
        /// Previous state of the gamepad
        /// </summary>
        private static GamePadState[] prevInputControllers = new GamePadState[4];

        #region >> Public Get Methods <<

        /// <summary>
        /// Returns the state of a specified button on a specified controller
        /// </summary>
        /// <param name="gBttn">ID of button to check</param>
        /// <param name="ctrNum">ID of controller to check</param>
        /// <returns></returns>
        public static bool GetButton(GmpdButton gBttn, ControllerNumber ctrNum ) {

            GamePadState cState = GetCurrentState((int)ctrNum);

            if (gBttn.IsDPad()) {
                if (InputGetDPadButtonState(cState.DPad, gBttn) == ButtonState.Pressed)
                    return true;
            } else if (InputGetButtonState(cState.Buttons, gBttn) == ButtonState.Pressed) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true during the frame the user pressed down the button
        /// </summary>
        /// <param name="gBttn">ID of button to check</param>
        /// <param name="ctrNum">ID of controller to check</param>
        /// <returns></returns>
        public static bool GetButtonDown(GmpdButton gBttn, ControllerNumber ctrNum) {

            GamePadState cState = GetCurrentState((int)ctrNum);
            GamePadState prevState = GetPreviousState((int)ctrNum);

            if(gBttn.IsDPad()) {
                if ((InputGetDPadButtonState(cState.DPad, gBttn) == ButtonState.Pressed) &&
                    (InputGetDPadButtonState(prevState.DPad, gBttn) == ButtonState.Released)) {
                    return true;
                }
            } else if ((InputGetButtonState(cState.Buttons, gBttn) == ButtonState.Pressed) &&
                    (InputGetButtonState(prevState.Buttons, gBttn) == ButtonState.Released)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true during the frame the user releases the button
        /// </summary>
        /// <param name="button">ID of button to check</param>
        /// <param name="ctrNum">ID of controller to check</param>
        /// <returns></returns>
        public static bool GetButtonUp(GmpdButton button, ControllerNumber ctrNum) {

            GamePadState cState = GetCurrentState((int)ctrNum);
            GamePadState prevState = GetPreviousState((int)ctrNum);

            if(button.IsDPad()) {
                if ((InputGetDPadButtonState(cState.DPad, button) == ButtonState.Released) &&
                    (InputGetDPadButtonState(prevState.DPad, button) == ButtonState.Pressed)) {
                    return true;
                }
            } else if ((InputGetButtonState(cState.Buttons, button) == ButtonState.Released) &&
                    (InputGetButtonState(prevState.Buttons, button) == ButtonState.Pressed)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the value of the axis identified
        /// </summary>
        /// <param name="axis">ID of axis to check</param>
        /// <param name="ctrNum">ID of controller to check</param>
        /// <returns></returns>
        public static float GetAxis(GmpdAxis axis, ControllerNumber ctrNum) {

            GamePadState cState = GetCurrentState((int)ctrNum);

            if(axis.IsDPad()) { return InputGetDPadAxisState(ref cState, axis); }

            return InputGetAxisState(ref cState, axis);
        }

        #endregion

        #region >> Public Set Methods  <<

        /// <summary>
        /// Sets the rumble intensity (Remember to set rumble to zero to stop motor)
        /// </summary>
        /// <param name="lIntensity">Left motor intensity</param>
        /// <param name="rIntensity">Right motor intensity</param>
        /// <param name="ctrNum">ID for specific Controller</param>
        /// <returns></returns>
        public static bool SetRumble(float lIntensity, float rIntensity, ControllerNumber ctrNum) {

            if (IsControllerNumberValid((int)ctrNum)) {
                GamePad.SetVibration((PlayerIndex)ctrNum, Mathf.Abs(lIntensity), Mathf.Abs(rIntensity));
                return true;
            }

            return false;
        }

        #endregion

        #region >> Updating States <<

        /// <summary>
        /// Updates the state of all the controllers
        /// </summary>
        public static void InputUpdateAllStates() {

            for(int i = 0; i < 4; i++) {
                InputUpdateSingleState((ControllerNumber)i);
            }
        }

        /// <summary>
        /// Updates the state of one specified controller
        /// </summary>
        /// <param name="ctrNum">ID for a specific Controller</param>
        public static void InputUpdateSingleState(ControllerNumber ctrNum) {
            prevInputControllers[(int)ctrNum] = currInputControllers[(int)ctrNum];
            currInputControllers[(int)ctrNum] = GamePad.GetState((PlayerIndex)ctrNum, GamePadDeadZone.IndependentAxes);
        }

        #endregion

        #region >> Getting States <<

        /// <summary>
        /// Returns current state 
        /// </summary>
        /// <param name="ctrlNumber">ID for a specific Controller</param>
        /// <returns></returns>
        private static GamePadState GetCurrentState(int ctrlNumber) {
            if (!IsControllerNumberValid(ctrlNumber)) { return currInputControllers[0]; }
            return currInputControllers[ctrlNumber -1];
        }

        /// <summary>
        /// Returns previous state
        /// </summary>
        /// <param name="ctrlNumber">ID for a specific Controller</param>
        /// <returns></returns>
        private static GamePadState GetPreviousState(int ctrlNumber) {
            if (!IsControllerNumberValid(ctrlNumber)) { return prevInputControllers[0]; }
            return prevInputControllers[ctrlNumber -1];
        }

        #endregion

        #region >> Getting Inputs <<

        /// <summary>
        /// Returns the state of the specified Button
        /// </summary>
        /// <param name="buttons">State of buttons to get information from</param>
        /// <param name="btn">ID for a specific button to be tested</param>
        /// <returns></returns>
        private static ButtonState InputGetButtonState(GamePadButtons buttons, GmpdButton btn) {
            ButtonState stateToReturn = ButtonState.Pressed;

            switch (btn) {
                case (GmpdButton.A):         stateToReturn = buttons.A; break;
                case (GmpdButton.B):         stateToReturn = buttons.B; break;
                case (GmpdButton.X):         stateToReturn = buttons.X; break;
                case (GmpdButton.Y):         stateToReturn = buttons.Y; break;
                case (GmpdButton.Start):     stateToReturn = buttons.Start; break;
                case (GmpdButton.Back):      stateToReturn = buttons.Back; break;
                case (GmpdButton.LBumper):   stateToReturn = buttons.LeftShoulder; break;
                case (GmpdButton.RBumper):   stateToReturn = buttons.RightShoulder; break;
                case (GmpdButton.LStick):    stateToReturn = buttons.LeftStick; break;
                case (GmpdButton.RStick):    stateToReturn = buttons.RightStick; break;
            }

            return stateToReturn;
        }

        private static float InputGetAxisState(ref GamePadState state, GmpdAxis gAxis) {
            float valToReturn = 0.0f;

            switch (gAxis) {
                case (GmpdAxis.LStickX):     valToReturn = state.ThumbSticks.Left.X; break;
                case (GmpdAxis.LStickY):     valToReturn = state.ThumbSticks.Left.Y; break;
                case (GmpdAxis.RStickX):     valToReturn = state.ThumbSticks.Right.X; break;
                case (GmpdAxis.RStickY):     valToReturn = state.ThumbSticks.Right.Y; break;
                case (GmpdAxis.LTrigger):    valToReturn = state.Triggers.Left; break;
                case (GmpdAxis.RTrigger):    valToReturn = state.Triggers.Right; break;
            }

            return valToReturn;
        }

        public static float InputGetDPadAxisState(ref GamePadState state, GmpdAxis gAxis) {
            float valToReturn = 0.0f;

            if (GmpdAxis.DpadX == gAxis) {
                valToReturn = (state.DPad.Left == ButtonState.Pressed ? -1f : 0f);
                valToReturn += (state.DPad.Right == ButtonState.Pressed ? 1f : 0f);
            } else if (GmpdAxis.DpadY == gAxis) {
                valToReturn = (state.DPad.Up == ButtonState.Pressed ? 1f : 0f);
                valToReturn += (state.DPad.Down == ButtonState.Pressed ? -1f : 0f);
            }

            return valToReturn;
        }

        public static ButtonState InputGetDPadButtonState(GamePadDPad pad, GmpdButton btn) {
            ButtonState stateToReturn = ButtonState.Pressed;

            switch(btn) {
                case (GmpdButton.DPadDown): stateToReturn = pad.Down; break;
                case (GmpdButton.DPadUp): stateToReturn = pad.Up; break;
                case (GmpdButton.DPadLeft): stateToReturn = pad.Left; break;
                case (GmpdButton.DPadRight): stateToReturn = pad.Right; break;
            }
            return stateToReturn;
        }

        #endregion

        #region >> Error Checking <<

        /// <summary>
        /// Returns true if the controller number is valid
        /// </summary>
        /// <param name="ctrlNum">ID for a specific Controller</param>
        /// <returns></returns>
        private static bool IsControllerNumberValid(int ctrlNum) {
            if(ctrlNum > 0 && ctrlNum <= 4) { return true; }
            else { Debug.LogError("Invalid Controller Number! (number < 4 && number > 0)"); }
            return false;
        }

        #endregion

    }

    public static class Extensions
    {
        public static bool IsDPad(this GmpdButton gBttn) {
            return (gBttn == GmpdButton.DPadDown ||
                gBttn == GmpdButton.DPadUp ||
                gBttn == GmpdButton.DPadLeft ||
                gBttn == GmpdButton.DPadRight);
        }

        public static bool IsDPad(this GmpdAxis gAxis) {
            return (gAxis == GmpdAxis.DpadX ||
                gAxis == GmpdAxis.DpadY);
        }

    }

    public struct gp
    {
        float timeStamp;
        GamePadState state;

    }
}