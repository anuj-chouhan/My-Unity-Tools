using UnityEngine;

namespace Anuj.Utility.FPSManager
{
    public class FPSManager : MonoBehaviour
    {
        [Header("Editor And Build")]
        [SerializeField] private int targetFPS;

        [Space(25)]
        [Header("Editor Only")]
        [SerializeField] private bool displayFPS = true;
        [SerializeField] private TextAnchor textAnchor = TextAnchor.UpperCenter;
        [SerializeField] private Color textColor;
        [SerializeField] private int size = 15;

        private float deltaTime = 0.0f;

        private void OnValidate()
        {
            if (size < 5)
            {
                size = 5;
            }
        }
        private void Awake()
        {
            Application.targetFrameRate = targetFPS;
        }
        private void Update()
        {
            if (!displayFPS)
            {
                return;
            }

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
        private void OnGUI()
        {
            if (!displayFPS)
            {
                return;
            }

            int width = Screen.width;
            int height = Screen.height;

            Rect rect = new Rect(0, 0, width, height);

            GUIStyle style = new GUIStyle();
            style.alignment = textAnchor;
            style.normal.textColor = textColor;
            style.fontSize = size;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

            GUI.Label(rect, text, style);
        }
    }

}