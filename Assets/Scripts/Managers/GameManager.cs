using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public int totalStoneCount;
        public int levelIndex,totalScore;
        public List<GameObject> gridLayers;
        public List<GameObject> stones;
        public List<RectTransform> slotsToShuffle;
        [SerializeField] private List<RectTransform> allSlots;
        private LevelLoader _levelLoader;
        private UIManager _uiManager;

       
       
       private const string LevelIndexKey = "LevelIndex"; 
       private const string ScoreIndexKey = "ScoreIndex";
        
       
        void Awake()
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
        }
        public void AddToListToShuffle()
        {
            foreach (RectTransform parent in allSlots)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    RectTransform child = (RectTransform)parent.GetChild(i);
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
                    var stone = Instantiate(stones[gridCell.stoneId], gridCell.transform.position, quaternion.identity);
                    stone.SetActive(true);
                    stone.transform.SetParent(gridCell.transform);
                    totalStoneCount++;
                    Debug.Log("Done");

                }
            }
            
           
        }

        public void CheckTotalStoneCount()
        {
            totalStoneCount--;
            if (totalStoneCount <= 0)
            {
                // Level Finished
                _uiManager.winPanel.SetActive(true);
                Debug.Log("Level Finished");
            }
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
