// Abu Kingly - 2016
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;
using System;

namespace Revamped {

    [CustomEditor(typeof(InputBuilder))]
    public class InputBuilder_Inspector : Editor {

        #region Fields

        private static GUIContent fileLocationGUIContent = new GUIContent("File Location", "Location in unity to save the file");

        private SerializedProperty fileLocationProperty;

        private static AnimBool[,] nAnim;
        private const int depthCount = 4;

        private InputBuilder cachedInputBuilder;
        private InputDevice inputCreationType = InputDevice.KEYBOARD;

        #endregion

        #region Unity Event Functions

        // Use this for initialization
        void OnEnable() {
            cachedInputBuilder = (InputBuilder)target;

            this.fileLocationProperty = this.serializedObject.FindProperty("definedPath");

            if (cachedInputBuilder.playerInputMappings == null)
                cachedInputBuilder.playerInputMappings = new System.Collections.Generic.List<InputMapping>();

            SetupAnimBools();
        }

        // Update is called once per frame
        public override void OnInspectorGUI() {
            cachedInputBuilder = (InputBuilder)target;
            serializedObject.Update();

            EditorGUILayout.PropertyField(this.fileLocationProperty, fileLocationGUIContent);

            EditorGUILayout.BeginHorizontal();
            // Adding 
            if(GUILayout.Button("Add")) {
                AddInput();
            }

            inputCreationType = (InputDevice)EditorGUILayout.EnumPopup(inputCreationType);

            // Removing
            if (cachedInputBuilder.playerInputMappings.Count >= 1 && GUILayout.Button("Remove")) {
                RemoveInput();
            }
            EditorGUILayout.EndHorizontal();

            // Loop through for displaying
            for (int i = 0; i < cachedInputBuilder.playerInputMappings.Count; i++) {
                if(cachedInputBuilder.playerInputMappings[i] != null)
                    PlayerSetup(nAnim[i,0], "Player: " + (i+1)+" " +cachedInputBuilder.playerInputMappings[i].type, i);
            }

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Number of Setup Inputs: " + cachedInputBuilder.playerInputMappings.Count);

            EditorGUILayout.Separator();

            if (GUI.changed) {
                serializedObject.ApplyModifiedProperties();

                for(int j = 0; j < cachedInputBuilder.playerInputMappings.Count; j++) {
                    EditorUtility.SetDirty(cachedInputBuilder.playerInputMappings[j]);
                }
            }

            //EditorGUILayout.Separator();
            //DrawDefaultInspector();
        }

        #endregion

        #region Methods

        #region >> Creation/Deletion <<

        public void AddInput() {
            cachedInputBuilder.playerInputMappings.Add(cachedInputBuilder.CreateInputType(inputCreationType, (cachedInputBuilder.playerInputMappings.Count+1)));
            SetupAnimBools();
        }

        public void RemoveInput() {
            cachedInputBuilder.playerInputMappings.Remove(cachedInputBuilder.playerInputMappings[cachedInputBuilder.playerInputMappings.Count - 1]);
            SetupAnimBools();
        }

        // Changes a players input type
        public void ChangeInputType(int i, InputDevice targetType) {
            cachedInputBuilder.playerInputMappings[i] = cachedInputBuilder.CreateInputType(targetType, i);
        }

        #endregion

        // Makes sure the animation bools are good to go.
        public void SetupAnimBools() {
            nAnim = new AnimBool[cachedInputBuilder.playerInputMappings.Count, depthCount];

            for (int i = 0; i < cachedInputBuilder.playerInputMappings.Count; i++) {
                for(int j = 0; j < depthCount; j++) {
                    nAnim[i,j] = new AnimBool(false);
                    nAnim[i,j].valueChanged.AddListener(Repaint);
                }
            }
        }

        private void PlayerSetup(AnimBool animBool, string name, int plyrNum) {

            animBool.target = EditorGUILayout.Foldout(animBool.target, name);
            if (EditorGUILayout.BeginFadeGroup(animBool.faded)) {

                InputDevice t = (InputDevice)EditorGUILayout.EnumPopup("Input Type", cachedInputBuilder.playerInputMappings[plyrNum].type);
                if(t != cachedInputBuilder.playerInputMappings[plyrNum].type) {
                    cachedInputBuilder.playerInputMappings[plyrNum] = cachedInputBuilder.CreateInputType(t, plyrNum+1);
                    cachedInputBuilder.playerInputMappings[plyrNum].type = t;
                }

                DisplayMapping(plyrNum);
            }
            EditorGUILayout.EndFadeGroup();
        }

        public void DisplayMapping(int plyrNum) {
            // Depending on input type, method of display is decided 
            switch (cachedInputBuilder.playerInputMappings[plyrNum].type) {
                case InputDevice.KEYBOARD:
                    KeyboardMapping k = cachedInputBuilder.playerInputMappings[plyrNum] as KeyboardMapping;
                    KeyboardDisplaySetup(ref k, plyrNum, "Keyboard"); break;
                case InputDevice.GAMEPAD:
                    GamepadMapping p = cachedInputBuilder.playerInputMappings[plyrNum] as GamepadMapping;
                    GamepadDisplaySetup(ref p, plyrNum, "Gamepad"); break;
            }
        }

        #region >> Gamepad <<

        private void GamepadDisplaySetup(ref GamepadMapping gmpdInptSetup, int plyrNum, string name) {

            nAnim[plyrNum, 1].target = EditorGUILayout.Foldout(nAnim[plyrNum, 1].target, name);
            if (EditorGUILayout.BeginFadeGroup(nAnim[plyrNum, 1].faded)) {
                EditorGUI.indentLevel++;

                // Button
                nAnim[plyrNum, 2].target = EditorGUILayout.Foldout(nAnim[plyrNum, 2].target, "Buttons");
                GamepadButtonList<AbstractButtonInput>(nAnim[plyrNum, 2], ref gmpdInptSetup);

                // Axis
                nAnim[plyrNum,3].target = EditorGUILayout.Foldout(nAnim[plyrNum, 3].target, "Axis");
                GamepadAxisList<AbstractAxisInput>(nAnim[plyrNum, 3], ref gmpdInptSetup);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        private static void GamepadButtonList<T>(AnimBool anim, ref GamepadMapping gmpdInptSetup) where T : struct, IConvertible {

            if (EditorGUILayout.BeginFadeGroup(anim.faded)) {

                int i = 0;
                foreach (string t in Enum.GetNames(typeof(T))) {
                    if (t == "NONE") continue;      // Doesn't show the NONE enum

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(t + ":", GUILayout.Width(100));       // Button Label

                    gmpdInptSetup.m_buttonCodes[i] = (GmpdButton)EditorGUILayout.EnumPopup(gmpdInptSetup.m_buttonCodes[i]);
                    i += 1;

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private static void GamepadAxisList<T>(AnimBool anim, ref GamepadMapping gmpdInptSetup) where T : struct, IConvertible {

            if (EditorGUILayout.BeginFadeGroup(anim.faded)) {

                int i = 0;
                foreach (string t in Enum.GetNames(typeof(T))) {
                    if (t == "NONE") continue;      // Doesn't show the NONE enum
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(t + ":", GUILayout.Width(100));       // Axis Label
                    gmpdInptSetup.m_axisCodes[i] = (GmpdAxis)EditorGUILayout.EnumPopup(gmpdInptSetup.m_axisCodes[i]);
                    i += 1;

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        #endregion

        #region >> Keyboard <<

        private void KeyboardDisplaySetup(ref KeyboardMapping kd, int plyrNum, string name) {

            nAnim[plyrNum, 1].target = EditorGUILayout.Foldout(nAnim[plyrNum, 1].target, name);
            if (EditorGUILayout.BeginFadeGroup(nAnim[plyrNum, 1].faded)) {
                EditorGUI.indentLevel++;

                // Button
                nAnim[plyrNum, 2].target = EditorGUILayout.Foldout(nAnim[plyrNum, 2].target, "Buttons");
                KeyboardButtonList<AbstractButtonInput>(nAnim[plyrNum, 2], ref kd);

                // Axis
                nAnim[plyrNum, 3].target = EditorGUILayout.Foldout(nAnim[plyrNum, 3].target, "Axis -/+");
                KeyboardAxisList<AbstractAxisInput>(nAnim[plyrNum, 3], ref kd);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }

        // T is the genericType 
        private static void KeyboardButtonList<T>(AnimBool anim, ref KeyboardMapping k) where T : struct, IConvertible {

            if (EditorGUILayout.BeginFadeGroup(anim.faded)) {

                int i = 0;
                foreach (string t in Enum.GetNames(typeof(T))) {
                    if (t == "NONE") continue;      // Doesn't show the NONE enum

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(t + ":", GUILayout.Width(100));       // Button Label

                    k.m_keyCodes[i] = (KeyCode) EditorGUILayout.EnumPopup(k.m_keyCodes[i]);
                    i += 1;

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        // Keyboard Axis Setup
        private static void KeyboardAxisList<T>(AnimBool anim, ref KeyboardMapping k) where T : struct, IConvertible {

            if (EditorGUILayout.BeginFadeGroup(anim.faded)) {

                int i = 0;
                foreach (string t in Enum.GetNames(typeof(T))) {
                    if (t == "NONE") continue;      // Doesn't show the NONE enum
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(t + ":", GUILayout.Width(100));       // Axis Label

                    k.m_keyToAxis[i].negativeKey = (KeyCode)EditorGUILayout.EnumPopup(k.m_keyToAxis[i].negativeKey);
                    k.m_keyToAxis[i].positiveKey = (KeyCode)EditorGUILayout.EnumPopup(k.m_keyToAxis[i].positiveKey);
                    i += 1;

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        #endregion

        #endregion
    }
}