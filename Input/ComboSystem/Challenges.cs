// Abu Kingly - 2016
using UnityEngine;
using System.Collections;

namespace Revamped
{
    public enum AxisChallengeType { POS, NEG, }; // Pressed all the way positive or negative

    public enum ButtonChallengeType { PRESSED, LETGO, };

    public static class Challenges
    {
        public static float activationZone;

        public static bool Axis(float axisState, AxisChallengeType chalTyp) {
            bool rtnBool = false;

            switch (chalTyp) {
                case AxisChallengeType.POS:
                    rtnBool = (axisState > .1f ? true : false);
                    break;
                case AxisChallengeType.NEG:
                    rtnBool = (axisState < -.1f ? true : false);
                    break;
            }

            return rtnBool;
        }

        // Takes an int
        public static bool Button(int bttnState, ButtonChallengeType chalTyp) {
            bool rtnBool = false;

            switch (chalTyp) {
                case ButtonChallengeType.PRESSED:
                    rtnBool = (bttnState == 1 ? true : false);
                    break;
                case ButtonChallengeType.LETGO:
                    rtnBool = (bttnState == 0 ? true : false);
                    break;
            }

            return rtnBool;
        }

        // Takes an bool
        public static bool Button(bool bttnState, ButtonChallengeType chalTyp) {
            bool rtnBool = false;

            switch (chalTyp) {
                case ButtonChallengeType.PRESSED:
                    rtnBool = (bttnState == true ? true : false);
                    break;
                case ButtonChallengeType.LETGO:
                    rtnBool = (bttnState == false ? true : false);
                    break;
            }

            return rtnBool;
        }
    }

}
