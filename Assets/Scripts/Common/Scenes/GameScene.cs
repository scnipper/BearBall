using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

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

        [Header("Screens")]
        public GameObject pauseScreen;
        public GameObject playScreen;
        public GameObject mainScreen;
        public GameObject playObjects;
        


        private int scoreLeft;
        private int scoreRight;
        private bool isFruitAdded;
        private readonly List<Fruit> fruits = new List<Fruit>();

        private void Start()
        {
            parent = transform;
        }

        private void UpdateScoreView()
        {
            scoreText.text = $"{scoreLeft} : {scoreRight}";
        }

        private IEnumerator FruitCreator()
        {
            fruits.Clear();
            isFruitAdded = true;
            while (isFruitAdded)
            {
                float xPos = Random.Range(minX,maxX);
                var fruitInst = Instantiate(fruit, new Vector3(xPos,10,0), Quaternion.identity, parent);
                fruitInst.IsRight = xPos > 0;
                fruitInst.onGoal += OnGoalFruit;
                fruits.Add(fruitInst);
                yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            }
        }

        public void PlayGame()
        {
            mainScreen.SetActive(false);
            playScreen.SetActive(true);
            playObjects.SetActive(true);
            
            scoreLeft = startScore;
            scoreRight = startScore;
            UpdateScoreView();
            
            StartCoroutine(FruitCreator());
        }


        public void ExitGame()
        {
            PauseGame(false);
            isFruitAdded = false;
            mainScreen.SetActive(true);
            playScreen.SetActive(false);
            playObjects.SetActive(false);
            
            foreach (var fr in fruits)
            {
                if (fr != null)
                {
                    Destroy(fr.gameObject);
                }
            }
        }
        public void PauseGame(bool isPause)
        {
            P.isPauseGame = isPause;
            pauseScreen.SetActive(isPause);
            Time.timeScale = isPause ? 0 : 1;
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