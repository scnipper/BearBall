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
                float xPos = Random.Range(minX,maxX);
                var fruitInst = Instantiate(fruit, new Vector3(xPos,10,0), Quaternion.identity, parent);
                fruitInst.IsRight = xPos > 0;
                yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            }
        }
    }
}