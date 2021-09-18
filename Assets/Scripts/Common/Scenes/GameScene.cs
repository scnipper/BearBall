using System.Collections;
using UnityEngine;

namespace Common.Scenes
{
    public class GameScene : MonoBehaviour
    {
        public Fruit fruit;
        public float minX = -9;
        public float maxX = 9;
        private Transform parent;

        private void Start()
        {
            parent = transform;
            StartCoroutine(FruitCreator());
        }

        private IEnumerator FruitCreator()
        {
            while (true)
            {
                Instantiate(fruit, new Vector3(Random.Range(minX,maxX),10,0), Quaternion.identity, parent);
                yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            }
        }
    }
}