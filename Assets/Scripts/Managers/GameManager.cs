using System.Collections;
using System.Collections.Generic;
using Data;
using InGame;
using Unity.Mathematics;
using UnityEngine;


namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public int totalStoneCount;

        public List<GameObject> gridLayers;
        public List<GameObject> stones;
        private ShuffleLogic _shuffleLogic;
        private LevelLoader _levelLoader;
        private SaveLoadManager _saveLoadManager;
        private UIManager _uiManager;
        private readonly WaitForSeconds _winPanelOpenDuration = new(2);


        private void Awake()
        {
            _saveLoadManager = FindObjectOfType<SaveLoadManager>();
            _levelLoader = FindObjectOfType<LevelLoader>();
            _uiManager = FindObjectOfType<UIManager>();
            _shuffleLogic = FindObjectOfType<ShuffleLogic>();

            _saveLoadManager.LoadData();
            gridLayers = _levelLoader.gridLayers;
        }

        private void Start()
        {
            _levelLoader.LoadLevel(_saveLoadManager.levelIndex);
            SetTheStones();
            _uiManager.totalPieceCount = totalStoneCount;
            _shuffleLogic.AddToListToShuffle();
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
    }
}