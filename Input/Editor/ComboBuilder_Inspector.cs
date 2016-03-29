// Abu Kingly - 2016
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;
using System;

namespace Revamped
{
    [CustomEditor(typeof(ComboBuilder))]
    public class ComboBuilder_Inspector : Editor
    {

        #region Fields

        #endregion

        #region Unity Event Functions

        void OnEnable() { 

        }

        public override void OnInspectorGUI() {



            EditorGUILayout.Separator();
            DrawDefaultInspector();
        }

        #endregion

        #region Methods

        #endregion
    }
}