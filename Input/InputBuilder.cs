// Abu Kingly - 2016
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Revamped
{
    public class InputBuilder : MonoBehaviour {

        #region Fields

        [SerializeField]
        public List<InputMapping> playerInputMappings;

        public string definedPath = "_Revamped/Input/Saves/";

        #endregion

        #region Unity Event Functions

        // Use this for initialization
        void Start() {
            InputManager.SetPlayerInputs(playerInputMappings.ToArray());
        }

        #endregion

        #region Methods

        // Creates new type of (keyboard or gamepad)
        public InputMapping CreateInputType(InputDevice type, int plyrNum) {

            switch (type) {
                case InputDevice.KEYBOARD:
                    KeyboardMapping kybrd = InputMappingAssetCreator.CreateKeyboardMappingAsset(definedPath, "KeyboardMapping "+plyrNum);
                    kybrd.type = InputDevice.KEYBOARD;
                    kybrd.plyrID = (PlayerID)(plyrNum);
                    InitializeKeyboardSetup(ref kybrd);
                    return kybrd;
                    break;
                case InputDevice.GAMEPAD:
                    GamepadMapping gmpd = InputMappingAssetCreator.CreateGamepadMappingAsset(definedPath, "GamepadMapping "+plyrNum);
                    gmpd.type = InputDevice.GAMEPAD;
                    gmpd.plyrID = (PlayerID)(plyrNum);
                    InitializeGamepadSetup(ref gmpd);
                    return gmpd;
                    break;
            }

            return null;
        }

        #region >> Initialization <<

        private void InitializeGamepadSetup(ref GamepadMapping plyrGmpdSetup) {
            plyrGmpdSetup.m_buttonCodes = new GmpdButton[Enum.GetNames(typeof(AbstractButtonInput)).Length];
            plyrGmpdSetup.m_axisCodes = new GmpdAxis[Enum.GetNames(typeof(AbstractButtonInput)).Length];
        }

        private void InitializeKeyboardSetup(ref KeyboardMapping plyrKybrdSetup) {
            plyrKybrdSetup.m_keyCodes = new KeyCode[Enum.GetNames(typeof(AbstractButtonInput)).Length];
            plyrKybrdSetup.m_keyToAxis = new AxisMapping[Enum.GetNames(typeof(AbstractAxisInput)).Length];
        }

        #endregion

        #endregion
    }
}