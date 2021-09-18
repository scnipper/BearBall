using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class Fruit : MonoBehaviour
    {
        public Sprite[] fruits;
        public Sprite[] wrongFruits;

        public event Action<bool> onGoal;
        
        private Rigidbody2D rbFruit;
        private bool isWrong;

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
                spriteRenderer.sprite = fruits[Random.Range(0, fruits.Length)];
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
                        float force = 1.5f + Random.Range(-0.2f, 1f);
                        rbFruit.AddForce(new Vector2(IsRight ? -force : force,10),ForceMode2D.Impulse);
                        break;
                    case ColliderIdentificator.TypeCollides.Basket:
                        Destroy(gameObject);
                        onGoal?.Invoke(isWrong);
                        break;
                    case ColliderIdentificator.TypeCollides.Ground:
                        Destroy(gameObject);
                        onGoal?.Invoke(!isWrong);
                        break;
                }
            }
        }

        public bool IsRight { get; set; }
    }
}