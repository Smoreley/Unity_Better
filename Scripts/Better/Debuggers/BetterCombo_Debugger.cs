// Abu Kingly
using UnityEngine;
using System.Collections;

public class BetterCombo_Debugger : MonoBehaviour {

    #region Fields

    Better.Combo bCombo;
    public Better.Move[] moves;

    public string searchWord;

	#endregion

	#region Unity Event Functions

	// Use this for initialization
	void Start () {

        bCombo = new Better.Combo(1f, moves);
    }

	// Update is called once per frame
	void Update () {

	}
	
	#endregion

	#region Methods

    [ContextMenu("Search")]
    public void SearchFor() {

        if(bCombo.GetCombo(searchWord)) {
            Debug.Log("It is active");
        }
    }

	#endregion
}
