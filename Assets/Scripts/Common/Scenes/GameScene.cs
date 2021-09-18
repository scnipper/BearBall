using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Scenes
{
    public class GameScene : MonoBehaviour
    {
        public Fruit fruit;
        public float minX = -9;
        public float maxX = 9;
        public int startScore = 10;
        public Text scoreText;
        private Transform parent;


        private int scoreLeft;
        private int scoreRight;
        private void Start()
        {
            parent = transform;

            scoreLeft = startScore;
            scoreRight = startScore;
            UpdateScoreView();
            
            StartCoroutine(FruitCreator());
        }

        private void UpdateScoreView()
        {
            scoreText.text = $"{scoreLeft} : {scoreRight}";
        }

        private IEnumerator FruitCreator()
        {
            while (true)
            {
                float xPos = Random.Range(minX,maxX);
                var fruitInst = Instantiate(fruit, new Vector3(xPos,10,0), Quaternion.identity, parent);
                fruitInst.IsRight = xPos > 0;
                fruitInst.onGoal += OnGoalFruit;
                yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            }
        }

        private void OnGoalFruit(bool isWrong)
        {
            if (isWrong)
            {
                scoreLeft--;
                if (scoreLeft < 0) scoreLeft = 0;
                UpdateScoreView();
            }
        }
    }
}