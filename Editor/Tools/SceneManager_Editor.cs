//Abu Kingly - 2016
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace Revamped
{
    public class SceneManager_Editor : EditorWindow
    {

        private string[] paths;                                     // find paths to all scenes (found by label)
        private int numOfScenes;                                    // number of found scenes
        private bool searchByTag = true;                            // search type bool
        private string assetLabelToSearch = "scene";                // asset label to use for sarching
        private GUIStyle rowStyles = new GUIStyle();
        private static Texture2D texOne, texTwo;                    // background texture
        private Vector2 scrollPos = Vector2.zero;                   // scroll position

        private GUIStyle deleteButtonStyle;

        public bool foldOut = false;

        [MenuItem("Tools/Scene Selection %#r")]
        static void Init() {
            EditorWindow.GetWindow(typeof(SceneManager_Editor), false, "SceneManager");

            // Create background textures
            CreateSimpleColorTex(ref texOne, new Color(0, 0, 0, .25f));
            CreateSimpleColorTex(ref texTwo, new Color(1, 1, 1, .25f));
        }

        void OnEnable() {
            searchByTag = EditorPrefs.GetBool("searchByTag");       // Load Presets so you don't have to keep opening it for each object.

            FindFiles();                                            // Search for scene files

            this.minSize = new Vector2(260, 75);                    // set the minimal allowed size the window will go
        }

        void OnGUI() {
            SetUpGUIStyles();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Found Scenes: " + numOfScenes);

            GUILayout.Label("Info:");
            foldOut = EditorGUILayout.Toggle(foldOut);

            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 1;
            GUILayout.Label("Label:");
            assetLabelToSearch = GUILayout.TextField(assetLabelToSearch, GUILayout.Width(45));
            searchByTag = EditorGUILayout.Toggle(searchByTag);

            EditorGUIUtility.labelWidth = 0;                        // reset label width to default

            GUILayout.EndHorizontal();

            if (GUI.changed) {
                FindFiles();                                        // Search for scene files if GUI changed
                EditorPrefs.SetBool("searchByTag", searchByTag);    // store manager settings in EditorPrefs
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

            for (int i = 0; i < numOfScenes; i++) {

                rowStyles.normal.background = (i % 2 < 1) ? texOne : texTwo;    // Aternates the color of the background for every other row

                GUILayout.BeginHorizontal(rowStyles);

                string pathName, parsed;
                if (searchByTag) {
                    pathName = AssetDatabase.GUIDToAssetPath(paths[i]);

                    // Parse out name of scene
                    parsed = pathName.Substring(pathName.LastIndexOf('/') + 1, pathName.LastIndexOf('.') - (int)pathName.LastIndexOf('/') - 1);
                } else {
                    int cachedIndex = paths[i].LastIndexOf("Assets");
                    pathName = paths[i].Substring(cachedIndex, paths[i].Length - cachedIndex);

                    // Parse out name of scene
                    parsed = paths[i].Substring(paths[i].LastIndexOf("\\") + 1, paths[i].LastIndexOf(".unity") - paths[i].LastIndexOf("\\") - 1);
                }

                // Load Button
                if (GUILayout.Button("Load", GUILayout.Width(45))) {
                    OpenSceneByPath(pathName);
                }

                //foldOut = EditorGUILayout.Foldout(foldOut, parsed);
                EditorGUILayout.LabelField(parsed);                 // Display, name of scene
                if (foldOut) {
                    EditorGUILayout.LabelField(pathName);           // Display, path to scene
                }

                // Delete Button
                if (GUILayout.Button("X", deleteButtonStyle)) {
                    string deletePopUpMessage = "Do you want to delete the scene " + parsed + ".unity\n\n" + "You will not be able undo this.";
                    if (EditorUtility.DisplayDialog("Scene Deletion Confirmation", deletePopUpMessage, "Delete", "Cancel")) {
                        DeleteSceneByPath(pathName);
                        FindFiles();
                    }
                }

                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

        }
        #region Methods

        public void SetUpGUIStyles() {

            // Delete Button Style
            deleteButtonStyle = new GUIStyle(GUI.skin.button);
            deleteButtonStyle.alignment = TextAnchor.MiddleRight;
            deleteButtonStyle.stretchWidth = false;
        }

        // Create a texture filled in with one color
        private static void CreateSimpleColorTex(ref Texture2D tex, Color col) {
            tex = new Texture2D(1, 1);
            tex.SetPixels(new Color[] { col });
            tex.Apply();

            tex.hideFlags = HideFlags.DontSave;
        }

        private void FindFiles() {
            if (searchByTag == true) {
                SearchByLabel();
            } else {
                SearchByFileType();
            }
        }

        // Find scene files by looking for .unity file type
        private void SearchByFileType() {
            paths = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
            numOfScenes = paths.Length;                         // number of found scenes
        }

        // Find scene files through Label
        private void SearchByLabel() {
            paths = AssetDatabase.FindAssets("l:" + assetLabelToSearch);
            numOfScenes = paths.Length;                         // number of found scenes
        }

        // Takes name of the scene
        public static void OpenSceneByName(string name) {

            string[] path = AssetDatabase.FindAssets(name + " l:Scene");

            // If there exists more then one scene with the same name throw error
            if (path.Length > 1) {
                Debug.LogError("More then one scene with the same name");
            } else {
                OpenSceneByPath(AssetDatabase.GUIDToAssetPath(path[0]));
            }
        }

        // Takes the path to the scene
        public static void OpenSceneByPath(string path) {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorSceneManager.OpenScene(path);
            }
        }

        // Delete a scene, given a path to the scene
        public static void DeleteSceneByPath(string path) {
            AssetDatabase.DeleteAsset(path);
        }
        #endregion
    }
}