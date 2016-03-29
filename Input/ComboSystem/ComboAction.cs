// Abu Kingly
using UnityEngine;
using System.Collections;

namespace Revamped
{
    public abstract class ComboAction
    {
        public float executionTime = 1f;    // how much time is given to complete this challenge
        public bool connecting = false;     // is chained to the next action
    }

    [System.Serializable]
    public class AxisAction : ComboAction
    {
        public AxisAction(AbstractAxisInput axis, AxisChallengeType type, bool chained) {
            axisID = axis;
            challengeID = type;
            connecting = chained;
        }

        public AbstractAxisInput axisID;        // what axis to test
        public AxisChallengeType challengeID;
    }

    [System.Serializable]
    public class ButtonAction : ComboAction
    {
        public ButtonAction(AbstractButtonInput bt, ButtonChallengeType type, bool chained) {
            buttonID = bt;
            challengeID = type;
            connecting = chained;
        }
        public AbstractButtonInput buttonID;
        public ButtonChallengeType challengeID;
    }
}