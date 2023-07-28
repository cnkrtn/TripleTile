using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class GridStone : MonoBehaviour
{
    public bool isClickable;
    
    public List<GridCell> cellsToCheck;
    public List<GridStone> stonesToCheck;
    
    private LevelLoader _levelLoader;
    [SerializeField] private Image image;

    private void Awake()
    {
        _levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void Start()
    {
        AddToCellsToCheck();

        IsItClickable();
    }

    private void IsItClickable()
    {
        if (stonesToCheck.Count>0) return;
        isClickable = true;
        image.color = Color.white;
        
    }

    private void AddToCellsToCheck()
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
                if ((cell.rowIndex == rowIndex && cell.colIndex == colIndex) || (cell.rowIndex == rowIndex  +1 &&
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
    
    private void OnMouseDown()
    {
        Debug.Log("Hit successful: GridStone clicked!");
    }
}
