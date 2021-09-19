using UnityEngine;
using Util;

namespace Common
{
    public class Bear : MonoBehaviour
    {
        public float minX = -9;
        public float maxX = 9;
        public Rigidbody2D body;
        
        private Transform trBear;
        private Camera mainCamera;
        private float offsetX;
        private Rigidbody2D bodyInst;

        private void Awake()
        {
            mainCamera = Camera.main;
            trBear = transform;
            bodyInst = Instantiate(body,trBear.parent);
            bodyInst.position = trBear.position;
            Destroy(body.gameObject);
        }
        private void OnMouseDown()
        {
            if(P.isPauseGame) return;
            
            var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            offsetX = trBear.position.x - mousePos.x;
        }

        private void FixedUpdate()
        {
            trBear.position = bodyInst.position;
        }

        private void OnMouseDrag()
        {
            if(P.isPauseGame) return;
            
            
            var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePos.x += offsetX;
            mousePos.z = 0;
            var trBearPosition = trBear.position;

            mousePos.y = trBearPosition.y;
            
            mousePos = ClampPosition(mousePos);
            
            bodyInst.MovePosition(mousePos);

        }

        private Vector2 ClampPosition(Vector2 mousePos)
        {
            if (mousePos.x < minX) mousePos.x = minX;
            if (mousePos.x > maxX) mousePos.x = maxX;
            return mousePos;
        }
    }
}