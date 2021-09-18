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
        private bool isEndGame;

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
            isEndGame = false;
            mainScreen.SetActive(false);
            playScreen.SetActive(true);
            playObjects.SetActive(true);

            InitScore();
            
            StartCoroutine(FruitCreator());
        }

        private void InitScore()
        {
            scoreLeft = startScore;
            scoreRight = startScore;
            UpdateScoreView();
        }

        public void ExitGame()
        {
            isEndGame = false;
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
            if (!isPause && isEndGame)
            {
                InitScore();
                isEndGame = false;
            }
            P.isPauseGame = isPause;
            pauseScreen.SetActive(isPause);
            Time.timeScale = isPause ? 0 : 1;
        }
        
        private void OnGoalFruit(bool isWrong)
        {
            if (isWrong)
            {
                scoreLeft--;
                if (scoreLeft <= 0)
                {
                    GameOver();
                    scoreLeft = 0;
                }
                UpdateScoreView();
            }
        }

        private void GameOver()
        {
            PauseGame(true);
            isEndGame = true;
        }
    }
}