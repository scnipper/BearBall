using UnityEngine;

namespace Common
{
    public class Bear : MonoBehaviour
    {
        public float minX = -9;
        public float maxX = 9;
        
        private Transform trBear;
        private Camera mainCamera;
        private float offsetX;

        private void Awake()
        {
            mainCamera = Camera.main;
            trBear = transform;
        }
        private void OnMouseDown()
        {
            var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            offsetX = trBear.position.x - mousePos.x;
        }

        private void OnMouseDrag()
        {
            var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePos.x += offsetX;
            mousePos.z = 0;
            var trBearPosition = trBear.position;

            mousePos.y = trBearPosition.y;


            if (mousePos.x < minX) mousePos.x = minX;
            if (mousePos.x > maxX) mousePos.x = maxX;
            
            trBear.position = mousePos;
        }
    }
}