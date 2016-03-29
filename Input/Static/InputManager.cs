// Abu Kingly - 2016
using UnityEngine;
using System.Collections.Generic;

namespace Revamped
{

    public enum PlayerID { NONE = 0, FIRST = 1, SECOND = 2, THIRD = 3, FOURTH = 4 };

    public enum InputDevice { KEYBOARD, GAMEPAD };

    public static class InputManager
    {
        private static int PrevFrameCount = -1;
        public static int numberOfPlayers = 4;

        private static InputMapping[] playerInputSetup = new InputMapping[numberOfPlayers];

        private static GamepadInput gmpdInput = new GamepadInput();      // refrence to gamepad input method
        private static KeyboardInput kybrdInput = new KeyboardInput();   // refrence to keyboard input method

        /// <summary>
        /// Current state of input
        /// </summary>
        public static InputForm[] currentInputFormStates = new InputForm[numberOfPlayers];

        /// <summary>
        /// Previous state of input
        /// </summary>
        public static InputForm[] previousInputFormStates = new InputForm[numberOfPlayers];

        #region >> Public SetMethods <<

        public static void SetRumble(float leftMotor, float rightMotor, params PlayerID[] plyrIDs) {
            for(int i = 0; i < plyrIDs.Length; i++) {
                if(playerInputSetup[CorrectedPlayerNumber(plyrIDs[i])].type == InputDevice.GAMEPAD) {
                    gmpdInput.SetRumble(leftMotor, rightMotor, CorrectedPlayerNumber(plyrIDs[i]));
                }
            }
        }

        public static void SetPlayerInputs(params InputMapping[] pSetup) {

            for (int i = 0; i < pSetup.Length; i++) {
                playerInputSetup[CorrectedPlayerNumber(pSetup[i].plyrID)] = pSetup[i];
            }
        }

        #endregion

        #region >> GetState Methods <<

        public static InputForm GetCurrentState(PlayerID plyrID) {
            UpdateAllInputForms();
            return currentInputFormStates[CorrectedPlayerNumber(plyrID)];
        }

        public static InputForm GetPreviousState(PlayerID plyrID) {
            UpdateAllInputForms();
            return previousInputFormStates[CorrectedPlayerNumber(plyrID)];
        }

        #endregion

        #region >> Update <<

        public static void UpdateAllInputForms() {

            if (!InputUpdateStillCurrentFrame()) {
                for (int i = 0; i < numberOfPlayers; i++) {
                    if(playerInputSetup[i] != null)
                        UpdateInputForm(i);
                }
            }
        }

        private static void UpdateInputForm(int plyrNum) {
            previousInputFormStates[plyrNum] = currentInputFormStates[plyrNum];

            // Depending on what type of input a player is assigned (keyboard, Gamepad)
            switch (playerInputSetup[plyrNum].type) {
                case InputDevice.GAMEPAD:
                    currentInputFormStates[plyrNum] = gmpdInput.GetInputState((GamepadMapping)playerInputSetup[plyrNum]);
                    break;
                case InputDevice.KEYBOARD:
                    currentInputFormStates[plyrNum] = kybrdInput.GetInputState((KeyboardMapping)playerInputSetup[plyrNum]);
                    break;
                default:
                    currentInputFormStates[plyrNum] = new InputForm();
                    break;
            }
        }

        private static bool InputUpdateStillCurrentFrame() {
            if (PrevFrameCount == Time.frameCount) { return true; }

            PrevFrameCount = Time.frameCount;
            return false;
        }

        #endregion

        #region >> Helper Methods <<

        private static int CorrectedPlayerNumber(PlayerID plyrID) {
            if (plyrID == PlayerID.NONE) {
                return 0;
            } else {
                return (int)plyrID - 1;
            }
        }

        private static int CorrectedPlayerNumber(int plyrID) {
            if(plyrID == 0) {
                return 0;
            } else {
                return (plyrID - 1);
            }
        }

        private static bool IsValidPlayerID(int plyrNum) {
            if (plyrNum > 0 && plyrNum <= numberOfPlayers) { return true; } 
            else { Debug.LogError("Invalid player Number! (number < " + numberOfPlayers + " && number > 0)"); }
            return false;
        }

        private static bool ActivePlayerInput(int plyrNum) {
            if(plyrNum < numberOfPlayers) { return true; }
            return false;
        }

        #endregion
    }

    // filled out by keybaord or gamepad and passed to the inputmanager to be used by the getbutton or axis
    [System.Serializable]
    public struct InputForm
    {
        public GenericButtonID m_button; // Buttons are stored in a flagged enum
        public float[] m_axis;
    }

    // This is the abstract base class
    // information pertaining to players control setup should go here
    [System.Serializable]
    public class PlayerInputSetup
    {
        public PlayerID plyrID;     // ID of the player
        public InputDevice type;      // What type of input the player is using
    }
}