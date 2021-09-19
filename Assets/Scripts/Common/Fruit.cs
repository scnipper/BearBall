using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class Fruit : MonoBehaviour
    {
        public Sprite[] fruits;
        public Sprite[] wrongFruits;
        public int fruitIndexClearWeight;

        public event Action<bool,int,bool> onGoal;
        
        private Rigidbody2D rbFruit;
        private bool isWrong;
        private int lastIdBear;
        private Bear lastBear;
        private bool isClearWeightBear;

        private void Start()
        {
            rbFruit = GetComponent<Rigidbody2D>();
            var spriteRenderer = GetComponent<SpriteRenderer>();

            if (Random.value > 0.8f)
            {
                isWrong = true;
                spriteRenderer.sprite = wrongFruits[Random.Range(0, wrongFruits.Length)];
            }
            else
            {
                int fruitIndex = Random.Range(0, fruits.Length);
                isClearWeightBear = fruitIndex == fruitIndexClearWeight;
                spriteRenderer.sprite = fruits[fruitIndex];
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var identificator = other.collider.GetComponent<ColliderIdentificator>();
            
            if (identificator != null)
            {
                switch (identificator.type)
                {
                    case ColliderIdentificator.TypeCollides.Bear:
                        lastIdBear = identificator.id;
                        lastBear = identificator.GetComponentInParent<Bear>();

                        if (IsDestroyAfterBearCollide)
                        {
                            if (isWrong && lastBear != null)
                            {
                                lastBear.SlowEffect();
                            }
                            onGoal?.Invoke(isWrong,lastIdBear,isClearWeightBear);
                            Destroy(gameObject);
                        }
                        else
                        {
                            float force = 4.5F + Random.Range(-0.4f, 1f);
                            rbFruit.AddForce(new Vector2(IsRight ? -force : force,15),ForceMode2D.Impulse);
                        }
                        break;
                    case ColliderIdentificator.TypeCollides.Basket:
                        var trBasket = other.transform;
                        var posBasket = trBasket.position;
                        trBasket.DOMove(new Vector3(posBasket.x - 0.2f, posBasket.y, 0), 0.04f)
                            .SetLoops(4,LoopType.Yoyo)
                            .SetEase(Ease.Linear);
                        Destroy(gameObject);
                        if (isWrong && lastBear != null)
                        {
                            lastBear.SlowEffect();
                        }
                        onGoal?.Invoke(isWrong,lastIdBear,isClearWeightBear);
                        break;
                    case ColliderIdentificator.TypeCollides.Ground:
                        Destroy(gameObject);
                        onGoal?.Invoke(!isWrong,lastIdBear,isClearWeightBear);
                        break;
                }
            }
        }

        public bool IsDestroyAfterBearCollide { get; set; }
        public bool IsRight { get; set; }
    }
}