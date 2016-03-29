// Abu Kingly - 2016
using UnityEngine;
using UnityEditor;

namespace Revamped
{
    public class InputMappingAssetCreator
    {

        #region >> Keyboard <<

        [MenuItem("Tools/Input/KeyboardMapping")]
        public static KeyboardMapping CreateKeyboardMappingAsset() {
            return CreateKeyboardMappingAsset("", "KeyboardMapping");
        }

        public static KeyboardMapping CreateKeyboardMappingAsset(string path, string assetName) {
            return CreateScriptableAsset<KeyboardMapping>(path, assetName);
        }

        #endregion

        #region >> Gamepad <<

        [MenuItem("Tools/Input/GamepadMapping")]
        public static GamepadMapping CreateGamepadMappingAsset() {
            return CreateGamepadMappingAsset("", "GamepadMapping");
        }

        public static GamepadMapping CreateGamepadMappingAsset(string path, string assetName) {
            return CreateScriptableAsset<GamepadMapping>(path, assetName);
        }

        #endregion

        public static T CreateScriptableAsset<T>(string path, string assetName, bool focus = false) where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, "Assets/" + path + assetName + ".asset");
            AssetDatabase.SaveAssets();

            if (focus) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }

            return asset;
        }
    }
}