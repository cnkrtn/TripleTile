using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {

        public int levelIndex;
        public List<GameObject> gridLayers;
        public List<GameObject> stones;
        private LevelLoader _levelLoader;
        private UIManager _uiManager;

       [SerializeField] public int totalStoneCount;
       
       private const string LevelIndexKey = "LevelIndex"; 
        
       
        void Awake()
        {
            
            _levelLoader = FindObjectOfType<LevelLoader>();
            _uiManager = FindObjectOfType<UIManager>();
            levelIndex = PlayerPrefs.GetInt(LevelIndexKey, 0);
            gridLayers = _levelLoader.gridLayers;
        }

        private void Start()
        {
            _levelLoader.LoadLevel(levelIndex);
            SetTheStones();

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
       
    }
}
