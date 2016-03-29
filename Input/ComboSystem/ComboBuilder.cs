// Abu Kingly
using UnityEngine;
using System.Collections;

namespace Revamped
{
    public class ComboBuilder : MonoBehaviour
    {

        #region Fields

        public MoveDefinition[] moveDef;
        public string[] comboNames;     // Initialized ComboNames

        #endregion

        #region Unity Event Functions

        // Use this for initialization
        void Start() {
            IntializeCombos();
        }

        #endregion

        #region Methods

        // Creates the moves that the Combo system will use
        public void IntializeCombos() {
            comboNames = new string[moveDef.Length];
            int comboCount = 0;

            // Loop through the moves
            for (int i = 0; i < moveDef.Length; i++) {

                ComboAction[] challSequence = new ComboAction[moveDef[i].comboDef.Length];
                // loop through sequence
                for (int j = 0; j < moveDef[i].comboDef.Length; j++) {

                    // Test to see what typ of combo it is (AXIS or BUTTON)
                    switch (moveDef[i].comboDef[j].inptType) {

                        case InputType.AXIS: // Its a axis type
                            challSequence[j] = CreateAxisAction(ref moveDef[i].comboDef[j]);

                            break;
                        case InputType.BUTTON: // Its a button type
                            challSequence[j] = CreateButtonAction(ref moveDef[i].comboDef[j]);

                            break;
                    }
                }
                comboCount += 1;
                // The created Move that can be added to the dictonary
                Move moveRev = new Move(moveDef[i].name, challSequence);

                comboNames[i] = moveRev.name;

                Combo.AddCombo(moveRev);
            }
        }

        private AxisAction CreateAxisAction(ref ComboDefinition def) {
            return new AxisAction(def.axs, def.aTyp, def.isChained);
        }

        private ButtonAction CreateButtonAction(ref ComboDefinition def) {
            return new ButtonAction(def.bttn, def.bTyp, def.isChained);
        }

        #endregion
    }

    [System.Serializable]
    public class MoveDefinition
    {
        public string name;
        public ComboDefinition[] comboDef;
    }

    [System.Serializable]
    public class ComboDefinition
    {
        public InputType inptType;

        [Header("Button")]
        public AbstractButtonInput bttn;
        public ButtonChallengeType bTyp;

        [Header("Axis")]
        public AbstractAxisInput axs;
        public AxisChallengeType aTyp;

        [Header("Other")]
        [Range(.1f,10)]
        public float time; // How much time to complete action
        public bool isChained; // Is chained to the next one meaning both this and the next move have to be true to move ahead
    }
}