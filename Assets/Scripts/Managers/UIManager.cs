using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject winPanel;
        public  int totalPieceCount;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI timeScoreText,pieceScoreText,totalScoreText;
        
        private GameManager _gameManager;
        private float _currentTime;
        private int _timeScore,_pieceScore;
        
        public bool _isTimerRunning,_firstScoreRoutineIsFinished;
        private readonly WaitForSeconds _countTime = new(0.001f);

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            
        }

        private void Start()
        {
            levelText.text = "Level " + _gameManager.levelIndex;
            _currentTime = 0f;
            _isTimerRunning = true;
        }

        private void Update()
        {
            if (!_isTimerRunning) return;
            _currentTime += Time.deltaTime;
            UpdateTimerUI();
        }
        private void UpdateTimerUI()
        {
           
            var minutes = Mathf.FloorToInt(_currentTime / 60f);
            var seconds = Mathf.FloorToInt(_currentTime % 60f);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void ScoreCounter()
        {
            _isTimerRunning = false;
            StartCoroutine(CountPieceScore());
            
        }

        private int CalculateTimeScore()
        {
            _timeScore = (int)(1/_currentTime * 10000f);
            return _timeScore;
        }
        
        private int CalculatePieceScore()
        {
            _pieceScore = (totalPieceCount/3) *100;
            return _pieceScore;
        }

        
        private IEnumerator CountTimeScore()
        {
           // yield return new WaitUntil(() => _firstScoreRoutineIsFinished);
            var currentCount = 0;

            while (currentCount <= CalculateTimeScore())
            {
               
                timeScoreText.text = "Time Bonus " +"       "+ currentCount.ToString();
                
                currentCount++;
                
                 yield return _countTime;
            }

            _gameManager.totalScore += _timeScore + _pieceScore;
            totalScoreText.text = "Total Score " +"       "+ ( _gameManager.totalScore).ToString();
            _gameManager.SaveTotalScore();
        }

        private IEnumerator CountPieceScore()
        {
            var currentCount = 0;

            while (currentCount <= CalculatePieceScore())
            {
               
                pieceScoreText.text = "Piece Matched " +"     "+ currentCount.ToString();
                
                currentCount++;
                
                yield return _countTime;
            }

            StartCoroutine(CountTimeScore());
        }

        public void RestartScene()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        
        public void NextLevel()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            _gameManager.SaveLevelIndex();
            
            SceneManager.LoadScene(currentSceneIndex);
            
        }
        
     
    }
}
