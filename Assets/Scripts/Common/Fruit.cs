using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class Fruit : MonoBehaviour
    {
        public Sprite[] fruits;
        public Sprite[] wrongFruits;
        private Rigidbody2D rbFruit;

        private void Start()
        {
            rbFruit = GetComponent<Rigidbody2D>();
            var spriteRenderer = GetComponent<SpriteRenderer>();

            if (Random.value > 0.8f)
            {
                IsWrong = true;
                spriteRenderer.sprite = wrongFruits[Random.Range(0, wrongFruits.Length)];
            }
            else
            {
                spriteRenderer.sprite = fruits[Random.Range(0, fruits.Length)];
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            rbFruit.AddForce(new Vector2(1.5f,10),ForceMode2D.Impulse);
            print(other.collider.name);
        }

        public bool IsWrong { get; set; }
    }
}