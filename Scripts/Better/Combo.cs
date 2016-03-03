// Abu Kingly
using UnityEngine;
using System.Collections.Generic;

namespace Better
{
    //public class BetterCombo : MonoBehaviour {

    //	#region Fields

    //	#endregion

    //	#region Unity Event Functions

    //	// Use this for initialization
    //	void Start () {

    //	}

    //	// Update is called once per frame
    //	void Update () {

    //	}

    //	#endregion

    //	#region Methods

    //	#endregion
    //}

    public class Combo
    {

        private Dictionary<string, Move> MoveDict = new Dictionary<string, Move>();
        private Move[] m_moves;
        private IInputTranslation Interpret;

        /// <summary>
        /// The amount of time before the sequence times out
        /// </summary>
        private float waitTime;

        public Combo(float t, params Move[] moves) {
            m_moves = moves;
            waitTime = t;

            for(int i = 0; i < m_moves.Length; i++) {
                MoveDict.Add(m_moves[i].m_name, m_moves[i]);
            }
        }

        // Think of a Input.GetButton() but for combos
        public bool GetCombo(string st_Move) {

            Move outValue;
            // Search through move dictonary
            if (MoveDict.TryGetValue(st_Move, out outValue)) {
                if (outValue.active) {
                    return true;
                } else {
                    return false;
                }
            }

            Debug.Log("Could Not Find the Value " + st_Move);
            return false;
        }


        public bool Check(float lastTime, ref int currentIndex, Move move) {

            if (Time.time > lastTime + waitTime) currentIndex = 0;

            if (currentIndex < move.sequence.Length) {
                //if (cachedGamePad.CheckForSequenceInput(move.sequence[currentIndex], cachedGamePad.GetInput(0))) {
                //    lastTime = Time.time;
                //    currentIndex += 1;
                //}

                if (Interpret.Convert(move.sequence[currentIndex])) {
                    lastTime = Time.time;
                    currentIndex += 1;
                }

                if (currentIndex >= move.sequence.Length) {
                    currentIndex = 0;
                    return true;
                } else return false;
            }
            return false;
        }

    }

    [System.Serializable]
    public class Move
    {

        public Move(string name, params Button[] seq) {
            m_name = name;
            sequence = seq;
        }

        public string m_name;
        public Button[] sequence;
        public bool active;
    }


    public struct Button
    {
        public string m_name;
        public bool comboHold;
    }

    //public abstract class ButtonState
    //{

    //}

    public abstract class AxisState
    {

    }

    public interface IInputTranslation
    {
        bool Convert(Button b);
    }

    public class Ps4Translation: IInputTranslation
    {
        public bool Convert(Button m) {

            return false;
        }
    }

    public enum eButtonState { Down, Up }

    public enum Buttons
    {
        b1 = 1,
        b2 = 2,
        b3 = 3,
        b4 = 4
    }

    public enum Ps4Buttons
    {
        X = 1,
        O = 2,
        Triangle = 3,
        Square = 4
    }
}