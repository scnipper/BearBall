using UnityEngine;

namespace Util
{
    public class Scaler : MonoBehaviour
    {
        public const float DefWidth = 2668;

        public RectTransform mainCanvas;
        public Transform world;
        private float scaleRatio;

        private void Start()
        {
            scaleRatio = mainCanvas.rect.width / DefWidth;
            world.localScale = new Vector3(scaleRatio, scaleRatio, 1);
            world.position = new Vector3(0, (scaleRatio -1) * 5 , 0);
        }
    }
}