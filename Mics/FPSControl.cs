// Abu Kingly - 2016
using UnityEngine;

namespace Revamped
{
    public class FPSControl : Singleton<FPSControl>
    {

        #region Fields

        [Range(-1,100)]
        [Tooltip("Caps fps to target. Set to 0 for no cap")]
        public int fpsTarget;

        private float m_deltaTime = 0.0f;
        private float m_lowestFPS = 100f;   // The lowest hit frame
        private float m_timeSinceLastUpdate, m_resetTime = 5f;  // Reset lowestFPS every 5 seconds

        #endregion

        #region Properties

        #endregion

        #region Unity Event Functions

        void Awake() {
            RefreshFramesPerSecond();
        }

        // Update is called once per frame
        void Update() {
            if (Time.timeScale != 0)
                m_deltaTime += (Time.deltaTime - m_deltaTime) * 0.1f;
        }

        #endregion

        #region Methods

        public void FpsGUIDisplay(ref Color debugTextColor) {

            // Calculates how much time it took to complete the last frame
            float msec = m_deltaTime * 1000.0f;   // change deltaTime(seconds) into milliseconds
            float fps = 1f / m_deltaTime;

            // if fps is lower then recored then update lowest
            // or if a certain amount of time has passed, update lowest
            if (m_lowestFPS > fps || (Time.realtimeSinceStartup - m_timeSinceLastUpdate) > m_resetTime) {
                m_lowestFPS = fps;
                m_timeSinceLastUpdate = Time.realtimeSinceStartup;
            }

            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            text += "\nLow: " + m_lowestFPS.ToString("F1") + " nUp:" +    // next Update (nUp)
                (m_resetTime - (Time.realtimeSinceStartup - m_timeSinceLastUpdate)).ToString("F1");

            int w = Screen.width, h = Screen.height;
            Rect rect = new Rect(0, 0, w, h*2 /100);

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            if (fps > fpsTarget / 2f)
                style.normal.textColor = debugTextColor;
            else style.normal.textColor = Color.red;

            GUI.Label(rect, text, style);
        }

        [ContextMenu("Refresh_FPS")]
        public void RefreshFramesPerSecond() {
#if UNITY_EDITOR
            if (fpsTarget > 0) {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = fpsTarget;
            } else {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
            }
#endif
        }

        #endregion
    }
}