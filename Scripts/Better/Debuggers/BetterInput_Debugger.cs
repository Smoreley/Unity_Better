// Abu Kingly
using UnityEngine;

public class BetterInput_Debugger : MonoBehaviour {

    #region Fields

    private Color gizColor_O, gizColor_Square, gizColor_Triangle, gizColor_X, gizColor_lBumper, gizColor_rBumper;
    private Color gizColorDpadLeft, gizColorDpadRight, gizColorDpadUp, gizColorDpadDown;
    private Color gizColor_lPushin, gizColor_rPushin, gizColor_start, gizColor_back;

    private Vector3 m_lJoystick, m_rJoystick;
    private float m_lTrigger, m_rTrigger;

    public float sphereSize  = .5f, cubeSize = 1.0f;
    public float rayLength = 1f;

    private Vector3 rightOffset = new Vector3(2,0,0);
    private Vector3 leftOffset = new Vector3(-2,0,0);

    #endregion

    #region Unity Event Functions

    // Use this for initialization
    void Start () {
        m_lJoystick = Vector3.zero;
        m_rJoystick = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {
            
        // Updates the input
        Better.Input.InputUpdateAllStates();

        #region >> Action Buttons <<
        if (Better.Input.GetButtonDown(Better.GmpdButton.A, Better.ControllerNumber.First)) {
            gizColor_X = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.A, Better.ControllerNumber.First)) {
            gizColor_X = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.B, Better.ControllerNumber.First)) {
            gizColor_O = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.B, Better.ControllerNumber.First)) {
            gizColor_O = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.X, Better.ControllerNumber.First)) {
            gizColor_Square = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.X, Better.ControllerNumber.First)) {
            gizColor_Square = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.Y, Better.ControllerNumber.First)) {
            gizColor_Triangle = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.Y, Better.ControllerNumber.First)) {
            gizColor_Triangle = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.Start, Better.ControllerNumber.First)) {
            gizColor_start = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.Start, Better.ControllerNumber.First)) {
            gizColor_start = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.Back, Better.ControllerNumber.First)) {
            gizColor_back = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.Back, Better.ControllerNumber.First)) {
            gizColor_back = Color.blue;
        }

        #endregion

        #region >> Dpad <<
        if (Better.Input.GetButtonDown(Better.GmpdButton.DPadLeft, Better.ControllerNumber.First)) {
            gizColorDpadLeft = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.DPadLeft, Better.ControllerNumber.First)) {
            gizColorDpadLeft = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.DPadRight, Better.ControllerNumber.First)) {
            gizColorDpadRight = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.DPadRight, Better.ControllerNumber.First)) {
            gizColorDpadRight= Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.DPadUp, Better.ControllerNumber.First)) {
            gizColorDpadUp = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.DPadUp, Better.ControllerNumber.First)) {
            gizColorDpadUp = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.DPadDown, Better.ControllerNumber.First)) {
            gizColorDpadDown = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.DPadDown, Better.ControllerNumber.First)) {
            gizColorDpadDown= Color.blue;
        }
        #endregion

        #region >> Joysticks <<
        m_lJoystick.x = Better.Input.GetAxis(Better.GmpdAxis.LStickX, Better.ControllerNumber.First);
        m_lJoystick.y = Better.Input.GetAxis(Better.GmpdAxis.LStickY, Better.ControllerNumber.First);

        m_rJoystick.x = Better.Input.GetAxis(Better.GmpdAxis.RStickX, Better.ControllerNumber.First);
        m_rJoystick.y = Better.Input.GetAxis(Better.GmpdAxis.RStickY, Better.ControllerNumber.First);

        // Joystick Pushins
        if (Better.Input.GetButtonDown(Better.GmpdButton.LStick, Better.ControllerNumber.First)) {
            gizColor_lPushin = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.LStick, Better.ControllerNumber.First)) {
            gizColor_lPushin = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.RStick, Better.ControllerNumber.First)) {
            gizColor_rPushin = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.RStick, Better.ControllerNumber.First)) {
            gizColor_rPushin = Color.blue;
        }
        #endregion

        #region >> Triggers/Bumpers <<
        m_rTrigger = Better.Input.GetAxis(Better.GmpdAxis.RTrigger, Better.ControllerNumber.First);
        m_lTrigger = Better.Input.GetAxis(Better.GmpdAxis.LTrigger, Better.ControllerNumber.First);

        // Bumpers
        if (Better.Input.GetButtonDown(Better.GmpdButton.LBumper, Better.ControllerNumber.First)) {
            gizColor_lBumper = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.LBumper, Better.ControllerNumber.First)) {
            gizColor_lBumper = Color.blue;
        }

        if (Better.Input.GetButtonDown(Better.GmpdButton.RBumper, Better.ControllerNumber.First)) {
            gizColor_rBumper = Color.red;
        } else if (Better.Input.GetButtonUp(Better.GmpdButton.RBumper, Better.ControllerNumber.First)) {
            gizColor_rBumper = Color.blue;
        }
        #endregion

    }

    void OnDrawGizmos() {
        if(!Application.isPlaying) { return; }

        Gizmos.color = gizColor_start;
        Gizmos.DrawCube(this.transform.position + Vector3.right * .5f + Vector3.up, Vector3.one * cubeSize);

        Gizmos.color = gizColor_back;
        Gizmos.DrawCube(this.transform.position + Vector3.left * .5f + Vector3.up, Vector3.one * cubeSize);

        #region >> Left Side <<
        // pushin
        Gizmos.color = gizColor_lPushin;
        Gizmos.DrawWireSphere(this.transform.position + leftOffset, sphereSize * 2f);

        // Dpad
        Gizmos.color = gizColorDpadLeft;
        Gizmos.DrawCube(this.transform.position + leftOffset + Vector3.left, Vector3.one * cubeSize);

        Gizmos.color = gizColorDpadRight;
        Gizmos.DrawCube(this.transform.position + leftOffset + Vector3.right, Vector3.one * cubeSize);

        Gizmos.color = gizColorDpadUp;
        Gizmos.DrawCube(this.transform.position + leftOffset + Vector3.up, Vector3.one * cubeSize);

        Gizmos.color = gizColorDpadDown;
        Gizmos.DrawCube(this.transform.position + leftOffset + Vector3.down, Vector3.one * cubeSize);

        Gizmos.color = gizColor_lBumper;
        Gizmos.DrawCube(this.transform.position + leftOffset + Vector3.up * 2f , new Vector3(2, 1, 1) * cubeSize);

        // Trigger
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position + leftOffset + Vector3.up * 2.5f, Vector3.one * cubeSize * m_lTrigger);

        // Joystick
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(this.transform.position + leftOffset, m_lJoystick * rayLength);
        Gizmos.DrawSphere(this.transform.position + leftOffset + m_lJoystick * rayLength, sphereSize);

        #endregion

        #region >> Right Side <<
        // pushin
        Gizmos.color = gizColor_rPushin;
        Gizmos.DrawWireSphere(this.transform.position + rightOffset, sphereSize * 2f);

        // Action Buttons
        Gizmos.color = gizColor_O;
        Gizmos.DrawSphere(this.transform.position + rightOffset + Vector3.right, sphereSize);

        Gizmos.color = gizColor_Square;
        Gizmos.DrawSphere(this.transform.position + rightOffset + Vector3.left, sphereSize);

        Gizmos.color = gizColor_X;
        Gizmos.DrawSphere(this.transform.position +rightOffset+ Vector3.down, sphereSize);

        Gizmos.color = gizColor_Triangle;
        Gizmos.DrawSphere(this.transform.position +rightOffset+ Vector3.up, sphereSize);

        Gizmos.color = gizColor_rBumper;
        Gizmos.DrawCube(this.transform.position + rightOffset + Vector3.up * 2f, new Vector3(2, 1, 1) * cubeSize);

        // Trigger
        Gizmos.color = Color.green;
        Gizmos.DrawCube(this.transform.position + rightOffset + Vector3.up * 2.5f, Vector3.one * cubeSize * m_rTrigger);

        //Joysick
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(this.transform.position + rightOffset, m_rJoystick * rayLength);
        Gizmos.DrawSphere(this.transform.position + rightOffset + m_rJoystick * rayLength, sphereSize);

        #endregion
    }

    #endregion
}
