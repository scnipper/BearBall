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
        public GameObject playModeBasket;
        public GameObject playModeFalling;

        [Header("Bears")]
        public Bear bear1;
        public Bear bear2;
        public Bear bear1Falling;
        public Bear bear2Falling;
        


        private int scoreLeft;
        private int scoreRight;
        private bool isFruitAdded;
        private readonly List<Fruit> fruits = new List<Fruit>();
        private bool isEndGame;
        private GameMode currentGameMode;

        private void Start()
        {
            parent = transform;
        }

        private void UpdateScoreView()
        {
            scoreText.text = $"{scoreLeft} : {scoreRight}";
        }

        private IEnumerator FruitCreator(GameMode gameMode)
        {
            fruits.Clear();
            isFruitAdded = true;
            while (isFruitAdded)
            {
                float xPos = Random.Range(minX,maxX);
                var fruitInst = Instantiate(fruit, new Vector3(xPos,10,0), Quaternion.identity, parent);
                fruitInst.IsRight = xPos > 0;
                fruitInst.onGoal += OnGoalFruit;
                fruitInst.IsDestroyAfterBearCollide = gameMode == GameMode.Falling;
                fruits.Add(fruitInst);
                yield return new WaitForSeconds(Random.Range(1.5f, 3f));
            }
        }

        public void PlayGame(int gameMode)
        {
            currentGameMode = (GameMode) gameMode;
            isEndGame = false;
            playScreen.SetActive(true);
            EnableGameModeView((GameMode) gameMode,true);

            InitScore();
            
            StartCoroutine(FruitCreator((GameMode) gameMode));
        }

        private void EnableGameModeView(GameMode gameMode, bool isEnable)
        {
            switch (gameMode)
            {
                case GameMode.Basket:
                    playModeBasket.SetActive(isEnable);
                    break;
                case GameMode.Falling:
                    playModeFalling.SetActive(isEnable);
                    break;
            }
        }
        private void InitScore()
        {
            bear1.gameObject.SetActive(true);
            bear1Falling.gameObject.SetActive(true);
            bear2.gameObject.SetActive(true);
            bear2Falling.gameObject.SetActive(true);
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
            EnableGameModeView(currentGameMode,false);
            
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
        
        private void OnGoalFruit(bool isWrong,int idBear,bool isClearWeight)
        {
            if (currentGameMode == GameMode.Basket)
            {
                if (idBear == 1)
                {
                    bear1.UpdateBodyBear(isClearWeight);
                    bear1Falling.UpdateBodyBear(isClearWeight);
                }
                if (idBear == 2)
                {
                    bear2.UpdateBodyBear(isClearWeight);
                    bear2Falling.UpdateBodyBear(isClearWeight);
                }

                if (isWrong)
                {
                    WrongScore(idBear);
                }
            }
            else if(currentGameMode == GameMode.Falling)
            {
                if (!isWrong)
                {
                    if (idBear == 1)
                    {
                        bear1.UpdateBodyBear(isClearWeight);
                        bear1Falling.UpdateBodyBear(isClearWeight);
                    }
                    if (idBear == 2)
                    {
                        bear2.UpdateBodyBear(isClearWeight);
                        bear2Falling.UpdateBodyBear(isClearWeight);
                    }
                    WrongScore(idBear);
                }
            }
        }

        private void WrongScore(int idBear)
        {
            if(idBear == 1)
                scoreLeft--;
            if(idBear == 2)
                scoreRight--;
            if (scoreLeft <= 0)
            {
                bear1.gameObject.SetActive(false);
                bear1Falling.gameObject.SetActive(false);
                scoreLeft = 0;
                if(currentGameMode == GameMode.Falling) GameOver();
            }
            else if(scoreRight <= 0)
            {
                bear2.gameObject.SetActive(false);
                bear2Falling.gameObject.SetActive(false);
                scoreRight = 0;
                if(currentGameMode == GameMode.Falling) GameOver();

            }
                
            if(scoreLeft <= 0 && scoreRight <= 0 && currentGameMode != GameMode.Falling)
            {
                GameOver();
            }
            UpdateScoreView();
        }

        private void GameOver()
        {
            PauseGame(true);
            isEndGame = true;
        }
    }
}