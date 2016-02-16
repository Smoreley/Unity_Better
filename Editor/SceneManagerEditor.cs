//Abu Kingly
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneManagerEditor : EditorWindow {

    string[] paths;                                             // find paths to all scenes (found by label)
    int numOfScenes;                                            // number of found scenes
    bool searchByTag = true;                                    // search type bool
    GUIStyle rowStyles = new GUIStyle();
    static Texture2D texOne, texTwo;                            // background texture
    Vector2 scrollPos = Vector2.zero;                           // scroll position

    [MenuItem("Tools/Scene Selection %#r")]
    static void Init() {
        EditorWindow.GetWindow(typeof(SceneManagerEditor), false, "SceneManager");
    }

    void OnEnable() {
        searchByTag = EditorPrefs.GetBool("searchByTag");       // Load Presets so you don't have to keep opening it for each object.

        FindFiles();                                            // Search for scene files

        this.minSize = new Vector2(200, 100);                   // set the minimal allowed size the window will go

        // Create background textures
        CreateSimpleColorTex(ref texOne, new Color(0, 0, 0, .15f));
        CreateSimpleColorTex(ref texTwo, new Color(1, 1, 1, .15f));
    }

    void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Found Scenes: " + numOfScenes);

        GUILayout.FlexibleSpace();
        EditorGUIUtility.labelWidth = 50;
        searchByTag = EditorGUILayout.Toggle("Tag", searchByTag);
        EditorPrefs.SetBool("searchByTag", searchByTag);        // store manager settings in EditorPrefs

        EditorGUIUtility.labelWidth = 0;                        // reset label width to default

        GUILayout.EndHorizontal();

        if(GUI.changed) {

            FindFiles();                                        // Search for scene files if GUI changed
        }
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

        for(int i = 0; i < numOfScenes; i++) {

            rowStyles.normal.background = (i % 2 < 1) ? texOne : texTwo;    // Aternates the color of the background for every other row

            GUILayout.BeginHorizontal(rowStyles);

            string pathName, parsed;

            if(searchByTag) {
                pathName = AssetDatabase.GUIDToAssetPath(paths[i]);

                // Parse out name of scene
                parsed = pathName.Substring(pathName.LastIndexOf('/') + 1, pathName.LastIndexOf('.') - (int)pathName.LastIndexOf('/') - 1);
            } else {
                int cachedIndex = paths[i].LastIndexOf("Assets");
                pathName = paths[i].Substring(cachedIndex, paths[i].Length - cachedIndex);

                // Parse out name of scene
                parsed = paths[i].Substring(paths[i].LastIndexOf("\\") +1, paths[i].LastIndexOf(".unity") - paths[i].LastIndexOf("\\") - 1);
            }

            // Load Button
            if(GUILayout.Button("Load", GUILayout.Width(45))) {
                OpenSceneByPath(pathName);
            }

            // Display, path to scene
            EditorGUILayout.LabelField(parsed, pathName);
            
            // Delete Button
            if(GUILayout.Button("Delete", GUILayout.Width(55))) {

                string deletePopUpMessage = "Do you want to delete the scene " + parsed + ".unity\n\n" + "You will not be able undo this.";
                if(EditorUtility.DisplayDialog("Scene Deletion Confirmation", deletePopUpMessage, "Delete", "Cancel")) {
                    DeleteSceneByPath(pathName);
                    FindFiles();
                }    
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

    }
    #region Methods

    // Create a texture filled in with one color
    private static void  CreateSimpleColorTex(ref Texture2D tex, Color col) {
        tex = new Texture2D(1, 1);
        tex.SetPixels(new Color[] { col });
        tex.Apply();
        tex.hideFlags = HideFlags.DontSave;
    }

    // Finds files depending on search method
    private void FindFiles() {
        if(searchByTag == true) {
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
        paths = AssetDatabase.FindAssets("l:scene");
        numOfScenes = paths.Length;                         // number of found scenes
    }

    // Takes name of the scene
    public static void OpenSceneByName(string name) {

        string[] path = AssetDatabase.FindAssets(name + " l:Scene");

        // If there exists more then one scene with the same name throw error
        if(path.Length > 1) {
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