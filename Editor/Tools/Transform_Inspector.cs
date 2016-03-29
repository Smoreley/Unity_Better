// Abu Kingly - 2016
using UnityEngine;
using UnityEditor;

namespace Revamped
{
    [CanEditMultipleObjects, CustomEditor(typeof(Transform))]
    public class Transform_Inspector : Editor
    {

        #region Fields

        private const float FIELD_WIDTH = 5f;
        private const float POSITION_MAX = 100000.0f;

        private const bool WIDE_MODE = true;

        private static GUIContent positionGUIContent = new GUIContent(LocalString("Position"), LocalString("The local position of this Game Object relative to the parent."));
        private static GUIContent rotationGUIContent = new GUIContent(LocalString("Rotation"), LocalString("The local rotation of this Game Object relative to the parent."));
        private static GUIContent scaleGUIContent = new GUIContent(LocalString("Scale"), LocalString("The local scaling of this Game Object relative to the parent."));

        private SerializedProperty positionProperty;
        private SerializedProperty rotationProperty;
        private SerializedProperty scaleProperty;

        #endregion

        #region Unity Editor Functions

        public void OnEnable() {
            this.positionProperty = this.serializedObject.FindProperty("m_LocalPosition");
            this.rotationProperty = this.serializedObject.FindProperty("m_LocalRotation");
            this.scaleProperty = this.serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI() {

            EditorGUIUtility.wideMode = Transform_Inspector.WIDE_MODE;
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / Transform_Inspector.FIELD_WIDTH; // align field to the right of inspector

            this.serializedObject.Update();

            // Position
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("RP", GUILayout.Width(28))) {
                this.positionProperty.vector3Value = Vector3.zero;
            }
            EditorGUILayout.PropertyField(this.positionProperty, positionGUIContent);
            EditorGUILayout.EndHorizontal();

            // Rotation
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("RR", GUILayout.Width(28))) {
                this.rotationProperty.quaternionValue = Quaternion.identity;
            }
            this.RotationPropertyField(this.rotationProperty, rotationGUIContent);
            EditorGUILayout.EndHorizontal();

            // Scale
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("RS", GUILayout.Width(28))) {
                this.scaleProperty.vector3Value = new Vector3(1, 1, 1);
            }
            EditorGUILayout.PropertyField(this.scaleProperty, scaleGUIContent);
            EditorGUILayout.EndHorizontal();

            // If there was a change
            if (GUI.changed) {

                // Apply changes back to the object
                this.serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion

        #region Methods

        // Handles rotation and checks if multiple objects are selected with the same rotation
        private void RotationPropertyField(SerializedProperty rotationProperty, GUIContent content) {
            Transform trans = (Transform)this.targets[0];

            Quaternion localRotation = trans.localRotation;

            foreach (UnityEngine.Object t in (UnityEngine.Object[])this.targets) {
                if (!SameRotation(localRotation, ((Transform)t).localRotation)) {
                    EditorGUI.showMixedValue = true;
                    break;
                }
            }

            EditorGUI.BeginChangeCheck();

            Vector3 eulerAngles = EditorGUILayout.Vector3Field(content, localRotation.eulerAngles);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObjects(this.targets, "Rotation Changed");

                foreach (UnityEngine.Object obj in this.targets) {
                    Transform t = (Transform)obj;
                    t.localEulerAngles = eulerAngles;
                }
            }
            EditorGUI.showMixedValue = false;
        }

        private bool SameRotation(Quaternion rot1, Quaternion rot2) {
            if (rot1.x != rot2.x) return false;
            if (rot1.y != rot2.y) return false;
            if (rot1.z != rot2.z) return false;
            if (rot1.w != rot2.w) return false;
            return true;
        }

        private Vector3 FixIfNan(Vector3 v) {
            if (float.IsNaN(v.x)) { v.x = 0; }

            if (float.IsNaN(v.y)) { v.y = 0; }

            if (float.IsNaN(v.z)) { v.z = 0; }

            return v;
        }

        private static string LocalString(string text) {
            return LocalizationDatabase.GetLocalizedString(text);
        }

        #endregion
    }
}