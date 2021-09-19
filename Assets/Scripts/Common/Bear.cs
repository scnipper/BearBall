using System.Collections;
using DG.Tweening;
using UnityEngine;
using Util;

namespace Common
{
    public class Bear : MonoBehaviour
    {
        public float minX = -9;
        public float maxX = 9;
        public Rigidbody2D body;
        public SpriteRenderer moveButton;
        public Sprite slowSprite;
        
        private Transform trBear;
        private Camera mainCamera;
        private float offsetX;
        private Rigidbody2D bodyInst;
        private bool isSlow;
        private Sprite moveButtonSprite;
        private float timeDurationSlow;

        private void Awake()
        {
            moveButtonSprite = moveButton.sprite;
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

        private void OnEnable()
        {
            StartCoroutine(UpdatePos());
        }

        private IEnumerator UpdatePos()
        {
            while (true)
            {
                if (isSlow)
                {
                    trBear.DOMove(bodyInst.position,0.2f).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(0.2f);
                    timeDurationSlow -= 0.2f;
                    if (timeDurationSlow <= 0)
                    {
                        isSlow = false;
                        moveButton.sprite = moveButtonSprite;
                    }
                }
                else
                {
                    trBear.position = bodyInst.position;
                    yield return null;
                }
                
            }
        }

        public void SlowEffect()
        {
            timeDurationSlow = 5;
            moveButton.sprite = slowSprite;
            isSlow = true;
        }

        private Vector2 ClampPosition(Vector2 mousePos)
        {
            if (mousePos.x < minX) mousePos.x = minX;
            if (mousePos.x > maxX) mousePos.x = maxX;
            return mousePos;
        }
    }
}