// Abu Kingly
using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    #region Fields

    [Tooltip("Object will not be destoryed when loading a new scene")]
    public bool persistent = false;

    protected static T instance;

    #endregion

    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null) {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                    go.name = typeof(Time).Name;
                }
            }
            return instance;
        }
    }

    #region Unity Event Functions

    // Use this for initialization
    void Awake() {

        if (instance == null) {
            instance = this.GetComponent<T>();

            if(persistent)
                DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    #region Methods

    #endregion
}
