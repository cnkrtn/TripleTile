
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject winPanel,losePanel;
        public  int totalPieceCount;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI timeScoreText,pieceScoreText,totalScoreText;
        
        private SaveLoadManager _saveLoadManager;
        private float _currentTime;
        private int _timeScore,_pieceScore;
        
        public bool isTimerRunning;
        private readonly WaitForSeconds _countTime = new(0.001f);

        private void Awake()
        {
            _saveLoadManager = FindObjectOfType<SaveLoadManager>();
        }

        private void Start()
        {
            levelText.text = "LEVEL " + (_saveLoadManager.levelIndex + 1);
            _currentTime = _saveLoadManager.modeIndex == 0 ? 0f : 120f;
            isTimerRunning = true;
        }

        private void Update()
        {
            if (!isTimerRunning) return;
            UpdateTimer();
            UpdateTimerUI();
        }

        private void UpdateTimer()
        {
            if (_saveLoadManager.modeIndex == 0)
            {
                _currentTime += Time.deltaTime;
            }
            else
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime <= 0)
                {
                    isTimerRunning = false;
                    losePanel.SetActive(true);
                }
            }
        }

        private void UpdateTimerUI()
        {
           
            var minutes = Mathf.FloorToInt(_currentTime / 60f);
            var seconds = Mathf.FloorToInt(_currentTime % 60f);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void ScoreCounter()
        {
            isTimerRunning = false;
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
            var currentCount = 0;

            while (currentCount <= CalculateTimeScore())
            {
                timeScoreText.text = "Time Bonus " +"       "+ currentCount.ToString();
                currentCount += 5;
                yield return _countTime;
            }
            _saveLoadManager.totalScore += _timeScore + _pieceScore;
            totalScoreText.text = "Total Score " +"       "+ ( _saveLoadManager.totalScore).ToString();
            _saveLoadManager.SaveTotalScore();
        }

        private IEnumerator CountPieceScore()
        {
            var currentCount = 0;
            while (currentCount <= CalculatePieceScore())
            {
                pieceScoreText.text = "Piece Matched " +"     "+ currentCount.ToString();
                currentCount+=10;
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
            _saveLoadManager.SaveLevelIndex();
            SceneManager.LoadScene(currentSceneIndex);
        }
        public void MainMenu()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            _saveLoadManager.SaveLevelIndex();
            SceneManager.LoadScene(currentSceneIndex -1);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
     
    }
}
