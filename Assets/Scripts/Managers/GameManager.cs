
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public int totalStoneCount;
        public int levelIndex,totalScore,modeIndex;
        public List<GameObject> gridLayers;
        public List<GameObject> stones;
        public List<RectTransform> slotsToShuffle;
        [SerializeField] private List<RectTransform> allSlots;
        private LevelLoader _levelLoader;
        private UIManager _uiManager;
        private readonly WaitForSeconds _winPanelOpenDuration = new(2);

        
       private const string LevelIndexKey = "LevelIndex"; 
       private const string ScoreIndexKey = "ScoreIndex";
       private const string ModeIndexKey = "ModeIndex";


       private void Awake()
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
            _uiManager = FindObjectOfType<UIManager>();
            LoadData();
            gridLayers = _levelLoader.gridLayers;
        }
       
        private void Start()
        {
            _levelLoader.LoadLevel(levelIndex);
            SetTheStones();
            _uiManager.totalPieceCount = totalStoneCount;
            AddToListToShuffle();
        }

        private void LoadData()
        {
            levelIndex = PlayerPrefs.GetInt(LevelIndexKey, 0);
            totalScore = PlayerPrefs.GetInt(ScoreIndexKey, 0);
            modeIndex = PlayerPrefs.GetInt(ModeIndexKey, 0);
        }
        public void AddToListToShuffle()
        {
            foreach (var parent in allSlots)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    var child = (RectTransform)parent.transform.GetChild(i);
                    if (child.childCount > 0)
                    {
                        slotsToShuffle.Add(child);
                    }
                }
            }
        }

        private void SetTheStones()
        {
            foreach (var grid in gridLayers)
            {
                for (int i = 0; i < grid.transform.childCount; i++)
                {
                    var gridCell = grid.transform.GetChild(i).GetComponent<GridCell>();
                    if (gridCell.stoneId <= 0 || gridCell.stoneId >= stones.Count) continue;
                    var stone = Instantiate(stones[gridCell.stoneId], gridCell.transform.position, Quaternion.identity);
                    stone.SetActive(true);
                    stone.transform.SetParent(gridCell.transform);
                    totalStoneCount++;
                }
            }
        }

        public void CheckTotalStoneCount()
        {
            totalStoneCount--;
            if (totalStoneCount > 0) return;
            StartCoroutine(StartLevelWinUI());
        }

        private IEnumerator StartLevelWinUI()
        {
            yield return _winPanelOpenDuration;
            _uiManager.winPanel.SetActive(true);
        }

        public void SaveLevelIndex()
        {
            levelIndex++;
            if (levelIndex > 4)
            {
                levelIndex = 0;}
            PlayerPrefs.SetInt(LevelIndexKey, levelIndex);
            PlayerPrefs.Save();
        }

        public void SaveTotalScore()
        {
            PlayerPrefs.SetInt(ScoreIndexKey, totalScore);
            PlayerPrefs.Save();
        }
       
    }
}
