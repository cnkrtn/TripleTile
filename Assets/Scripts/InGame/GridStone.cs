using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class GridStone : MonoBehaviour
    {
        public bool isClickable;

        public List<GridCell> cellsToCheck;
        public List<GridStone> stonesToCheck;
        public int stoneID;
        private LevelLoader _levelLoader;
        [SerializeField] private Image image;


        private void OnEnable()
        {
            EventManager.OnStoneAddedToPlayerHand += OnStoneAddedToPlayerHand;
        }

        private void OnDisable()
        {
            EventManager.OnStoneAddedToPlayerHand -= OnStoneAddedToPlayerHand;
        }


        private void Awake()
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
        }

        private void Start()
        {
            AddToCellsToCheck();
            IsItClickable();
        }

        public void IsItClickable()
        {
            if (stonesToCheck.Count <= 0)
            {
                isClickable = true;
                image.color = Color.white;
            }
            else
            {
                isClickable = false;
                image.color = Color.gray;
            }
        }

        public void AddToCellsToCheck()
        {
            var rowIndex = transform.GetComponentInParent<GridCell>().rowIndex;
            var colIndex = transform.GetComponentInParent<GridCell>().colIndex;
            var gridLayerId = GetComponentInParent<GridLayer>().gridLayerID;
            if (gridLayerId + 1 >= _levelLoader.gridLayers.Count) return;

            var gridLayerAbove = _levelLoader.gridLayers[gridLayerId + 1];
            if (gridLayerId % 2 == 0)
            {
                for (var i = 0; i < gridLayerAbove.transform.childCount; i++)
                {
                    var cell = gridLayerAbove.transform.GetChild(i).GetComponent<GridCell>();
                    if ((cell.rowIndex == rowIndex && cell.colIndex == colIndex) || (cell.rowIndex == rowIndex + 1 &&
                            cell.colIndex == colIndex)
                        || (cell.rowIndex == rowIndex &&
                            cell.colIndex == colIndex - 1) ||
                        (cell.rowIndex == rowIndex + 1 &&
                         cell.colIndex == colIndex - 1))
                    {
                        cellsToCheck.Add(cell);
                    }
                }
            }
            else
            {
                for (var i = 0; i < gridLayerAbove.transform.childCount; i++)
                {
                    var cell = gridLayerAbove.transform.GetChild(i).GetComponent<GridCell>();
                    if ((cell.rowIndex == rowIndex && cell.colIndex == colIndex) || (cell.rowIndex == rowIndex - 1 &&
                            cell.colIndex == colIndex)
                        || (cell.rowIndex == rowIndex &&
                            cell.colIndex == colIndex + 1) ||
                        (cell.rowIndex == rowIndex - 1 &&
                         cell.colIndex == colIndex + 1))
                    {
                        cellsToCheck.Add(cell);
                    }
                }
            }


            foreach (var cell in cellsToCheck)
            {
                if (cell.transform.childCount > 0)
                {
                    var stone = cell.transform.GetChild(0).GetComponent<GridStone>();
                    if (stone != null)
                    {
                        stonesToCheck.Add(stone);
                    }
                }
            }
        }

        private void OnStoneAddedToPlayerHand()
        {
            if (transform.parent.GetComponent<GridCell>() == null) return;
            cellsToCheck.Clear();
            stonesToCheck.Clear();
            AddToCellsToCheck();
            IsItClickable();
        }
    }
}