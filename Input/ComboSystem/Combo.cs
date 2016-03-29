// Abu Kingly - 2016
using UnityEngine;
using System.Collections.Generic;

namespace Revamped
{
    public enum InputType { NONE, AXIS, BUTTON };

    public static class Combo {

        public static float[] lastComboTime = new float[InputManager.numberOfPlayers];

        private static Dictionary<string, Move> moveDictonary = new Dictionary<string, Move>();

        #region >> Public GetMethods <<

        public static bool GetCombo(string cmbName, PlayerID plyrID) {

            Move cachedMove;
            if (moveDictonary.TryGetValue(cmbName, out cachedMove)) {
                if (CheckCombo(cachedMove, ref lastComboTime[(int)plyrID], ref cachedMove.lastIndex, plyrID)) { return true; }
            }

            return false;
        }

        #endregion

        #region >> Test Methods <<

        // Checks if a combo has been succesful
        private static bool CheckCombo(Move move, ref float m_timeLastInput, ref int currentIndex, PlayerID plyrID) {

            // if enough time has past set the index back to zero (combo timeout)
            if (Time.time > m_timeLastInput + move.sequence[currentIndex].executionTime) currentIndex = 0;

            // If current index is less then the sequence
            if (currentIndex < move.sequence.Length) {

                bool chainWasSucces = false;
                int offset = 0;
                do {
                    if (ActionCheck(move.sequence[currentIndex + offset], plyrID)) {
                        chainWasSucces = true;
                        offset += 1;

                        // If not part of a combo chain break
                        if (!move.sequence[currentIndex + offset - 1].connecting) { break; }
                    } else {
                        chainWasSucces = false;
                        break;
                    }

                } while ((currentIndex + offset) < move.sequence.Length);

                if (chainWasSucces) {
                    currentIndex += offset;
                    m_timeLastInput = Time.time;
                }

                //DEBUGING THE INPUT TYPES
                //InputType[] it = new InputType[move.sequence.Length];
                //for (int i = 0; i < move.sequence.Length; i++) {
                //    it[i] = GetComboActionType(move.sequence[i]);
                //    Debug.Log(it[i] + "  " + i);
                //}

                // If there are no more combos left return true and reset index
                if (currentIndex >= move.sequence.Length) {
                    currentIndex = 0;
                    return true;
                } else return false;
            }

            return false;
        }

        private static bool ActionCheck(ComboAction chall, PlayerID plyrID) {
            bool returnVal = false;

            InputType inpType = GetComboActionType(chall);
            switch (inpType) {
                case InputType.AXIS:
                    AxisAction a = (AxisAction)chall;
                    float someFloat = Input.GetAxis(a.axisID, plyrID);

                    if (Challenges.Axis(someFloat, a.challengeID)) { returnVal = true; }
                    break;

                case InputType.BUTTON:
                    ButtonAction b = (ButtonAction)chall;
                    bool someBool = Input.GetButtonDown(b.buttonID, plyrID);

                    if (Challenges.Button(someBool, b.challengeID)) { returnVal = true; }
                    break;
            }
            return returnVal;
        }

        private static InputType GetComboActionType(ComboAction chall) {

            if (chall.GetType() == typeof(ButtonAction)) {
                return InputType.BUTTON;
            } else if (chall.GetType() == typeof(AxisAction)) {
                return InputType.AXIS;
            } else {
                Debug.LogError("InputType is invalid (must be axis or button)");
                return InputType.NONE;
            }
        }

        #endregion

        #region >> Adding/Removing Combos <<

        // Adding combo to the dictonary
        public static void AddCombo(Move mv) {

            if (!moveDictonary.ContainsKey(mv.name)) {
                moveDictonary.Add(mv.name, mv);
            } else { Debug.LogError("Combo Already Present In Dictonary"); }
        }

        // Quick way to add many combos to the dictonary
        public static void AddMultpleCombos(Move[] mvs) {
            for (int i = 0; i < mvs.Length; i++) {
                AddCombo(mvs[i]);
            }
        }

        // Removes Combo from dictonary
        public static void RemoveCombo(string moveName) {
            if (moveDictonary.ContainsKey(moveName)) {
                moveDictonary.Remove(moveName);
            }
        }

        // Returns Number of comboes in the dict
        public static int GetComboCount() {
            return moveDictonary.Count;
        }

        // Resets combo moves and index move
        public static void Reset() {
            foreach (var it in moveDictonary) {
                it.Value.lastIndex = 0;
            }
        }

        #endregion

        #region >> Debugging <<

        public static void DebugCombo(Move combo) {
            InputType[] it = new InputType[combo.sequence.Length];
            for (int i = 0; i < combo.sequence.Length; i++) {
                it[i] = GetComboActionType(combo.sequence[i]);
                Debug.Log(it[i] + "  " + i);
            }
        }

        #endregion

    }

    [System.Serializable]
    public class Move {

        public Move(string nm, ComboAction[] seq) {
            name = nm;
            sequence = seq;
        }

        public string name = "Unknown";
        public int lastIndex = 0;
        public ComboAction[] sequence;
    }
}