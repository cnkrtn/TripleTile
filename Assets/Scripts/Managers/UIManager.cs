using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject winPanel;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timerText;
        private GameManager _gameManager;
        private float _currentTime;
        private bool _isTimerRunning;

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
