// Abu Kingly - 2016
using UnityEngine;
using XInputDotNetPure;

namespace Revamped
{
    public class GamepadInput
    {
        public InputForm GetInputState(GamepadMapping plyrSetup) {

            InputForm rtnForm = new InputForm();

            GamePadState gmpdState = GamePad.GetState((PlayerIndex)(plyrSetup.gmpdID), GamePadDeadZone.IndependentAxes);

            UpdateButtonStates(ref rtnForm, gmpdState, plyrSetup.m_buttonCodes);
            UpdateAxisStates(ref rtnForm, gmpdState, plyrSetup.m_axisCodes);
            return rtnForm;
        }

        #region >> CheckStates <<

        private void UpdateButtonStates(ref InputForm inptForm, GamePadState gmpdState, GmpdButton[] bttnCodes) {

            // Get buttons
            for (int i = 0; i < bttnCodes.Length; i++) {
                if (IsDPad(bttnCodes[i])) {
                    if (InputGetDPadButtonState(gmpdState.DPad, bttnCodes[i]))
                        inptForm.m_button |= (GenericButtonID)((int)1 << i);
                } else {
                    if (InputGetButtonState(gmpdState.Buttons, bttnCodes[i]))
                        inptForm.m_button |= (GenericButtonID)((int)1 << i);
                }
            }
        }

        private void UpdateAxisStates(ref InputForm inptForm, GamePadState gmpdState, GmpdAxis[] axsCodes) {
            inptForm.m_axis = new float[axsCodes.Length];

            // Get axises
            for (int j = 0; j < axsCodes.Length; j++) {
                if(IsDPad(axsCodes[j])) {
                    inptForm.m_axis[j] = InputGetDPadAxisState(gmpdState.DPad, axsCodes[j]);
                } else {
                    inptForm.m_axis[j] = InputGetAxisState(ref gmpdState, axsCodes[j]);
                }
            }
        }

        #endregion

        #region >> Getting Input States <<

        private bool InputGetButtonState(GamePadButtons buttons, GmpdButton btn) {
            
            switch (btn) {
                case (GmpdButton.A): return ButtonStateToBool(buttons.A); break;
                case (GmpdButton.B): return ButtonStateToBool(buttons.B); break;
                case (GmpdButton.X): return ButtonStateToBool(buttons.X); break;
                case (GmpdButton.Y): return ButtonStateToBool(buttons.Y); break;
                case (GmpdButton.Start): return ButtonStateToBool(buttons.Start); break;
                case (GmpdButton.Back): return ButtonStateToBool(buttons.Back); break;
                case (GmpdButton.LBumper): return ButtonStateToBool(buttons.LeftShoulder); break;
                case (GmpdButton.RBumper): return ButtonStateToBool(buttons.RightShoulder); break;
                case (GmpdButton.LStick): return ButtonStateToBool(buttons.LeftStick); break;
                case (GmpdButton.RStick): return ButtonStateToBool(buttons.RightStick); break;
            }
            return false;
        }

        private bool InputGetDPadButtonState(GamePadDPad pad, GmpdButton btn) {

            switch (btn) {
                case (GmpdButton.DPadDown): return ButtonStateToBool(pad.Down); break;
                case (GmpdButton.DPadUp): return ButtonStateToBool(pad.Up); break;
                case (GmpdButton.DPadLeft): return ButtonStateToBool(pad.Left); break;
                case (GmpdButton.DPadRight): return ButtonStateToBool(pad.Right); break;
            }
            return false;
        }

        private static float InputGetAxisState(ref GamePadState state, GmpdAxis gAxis) {

            switch (gAxis) {
                case (GmpdAxis.LStickX): return state.ThumbSticks.Left.X; break;
                case (GmpdAxis.LStickY): return state.ThumbSticks.Left.Y; break;
                case (GmpdAxis.RStickX): return state.ThumbSticks.Right.X; break;
                case (GmpdAxis.RStickY): return state.ThumbSticks.Right.Y; break;
                case (GmpdAxis.LTrigger): return state.Triggers.Left; break;
                case (GmpdAxis.RTrigger): return state.Triggers.Right; break;
            }
            return 0;
        }

        public static float InputGetDPadAxisState(GamePadDPad pad, GmpdAxis gAxis) {
            float valToReturn = 0.0f;

            if (GmpdAxis.DpadX == gAxis) {
                valToReturn = (pad.Left == ButtonState.Pressed ? -1f : 0f);
                valToReturn += (pad.Right == ButtonState.Pressed ? 1f : 0f);
            } else if (GmpdAxis.DpadY == gAxis) {
                valToReturn = (pad.Up == ButtonState.Pressed ? 1f : 0f);
                valToReturn += (pad.Down == ButtonState.Pressed ? -1f : 0f);
            }

            return valToReturn;
        }

        private bool ButtonStateToBool(ButtonState bttstate) {
            if(bttstate == ButtonState.Pressed) { return true;  }
            return false;
        }

        #endregion

        #region >> Set <<

        public bool SetRumble(float lIntensity, float rIntensity, int ctrNum) {

            if (!IsControllerNumberValid((int)ctrNum)) { return false; }

            GamePad.SetVibration((PlayerIndex)ctrNum, Mathf.Abs(lIntensity), Mathf.Abs(rIntensity));
            return true;
        }

        #endregion

        #region >> Checking <<

        public bool IsDPad(GmpdButton gBttn) {
            return (gBttn == GmpdButton.DPadDown ||
                gBttn == GmpdButton.DPadUp ||
                gBttn == GmpdButton.DPadLeft ||
                gBttn == GmpdButton.DPadRight);
        }

        public bool IsDPad(GmpdAxis gAxis) {
            return (gAxis == GmpdAxis.DpadX ||
                gAxis == GmpdAxis.DpadY);
        }

        #endregion

        #region >> Controller <<

        public int GetControllerCount() {
            int numberOfConnectedControllers = 0;

            for (int i = 0; i < 4; i++) {
                if (XInputDotNetPure.GamePad.GetState((PlayerIndex)i).IsConnected) {
                    numberOfConnectedControllers += 1;
                }
            }
            return numberOfConnectedControllers;
        }

        private string[] GetControllerNames() {
            return UnityEngine.Input.GetJoystickNames();
        }

        #endregion

        private static bool IsControllerNumberValid(int ctrlNum) {
            if (ctrlNum >= 0 && ctrlNum <= 3) { return true; } else { Debug.LogError("Invalid Controller Number! (number < 4 && number > 0)"); }
            return false;
        }
    }

    //public interface IForceFeeback
    //{
    //    void SetRumble(float firstAmnt, float secondAmnt, int plyrID);
    //}

    //public static class Extensions
    //{
    //    public static bool IsDPad(this GmpdButton gBttn) {
    //        return (gBttn == GmpdButton.DPadDown ||
    //            gBttn == GmpdButton.DPadUp ||
    //            gBttn == GmpdButton.DPadLeft ||
    //            gBttn == GmpdButton.DPadRight);
    //    }

    //    public static bool IsDPad(this GmpdAxis gAxis) {
    //        return (gAxis == GmpdAxis.DpadX ||
    //            gAxis == GmpdAxis.DpadY);
    //    }
    //}
}