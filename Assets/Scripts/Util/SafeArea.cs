using UnityEngine;

namespace Util
{
    public class SafeArea : MonoBehaviour
    {


        public bool left;
        public bool top;
        public bool right;
        public bool bottom;
        public float scale = 2;
        

        private Vector2 sizeScreen;
        private Vector2 leftBottom;
        private Vector2 rightBottom;

        private void Awake()
        {
            var safeArea = Screen.safeArea;
            rightBottom = safeArea.position;
            leftBottom = new Vector2(Screen.width - safeArea.width,Screen.height - safeArea.height);

            var rectTransform = GetComponent<RectTransform>();

            var pos = rectTransform.anchoredPosition;

            if (left)
            {
                pos.x += leftBottom.x * scale;
            }
            if (right)
            {
                pos.x -= rightBottom.x * scale;
            }
            if (bottom)
            {
                pos.y += leftBottom.y * scale;
            }
            if (top)
            {
                pos.y -= leftBottom.y * scale;
            }

            rectTransform.anchoredPosition = pos;
        }
    }
}