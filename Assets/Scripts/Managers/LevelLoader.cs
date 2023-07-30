using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

namespace Managers
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Level Data")]
        public List<GameObject> gridLayers;
        public TextAsset levelData1;
        public TextAsset levelData2;
        public TextAsset levelData3;
        public TextAsset levelData4;
        public TextAsset levelData5;

      

        public void LoadLevel(int levelIndex)
        {
            string json = GetJsonForLevel(levelIndex);
            if (json != null)
            {
                SerializableLevel serializableLevel = JsonUtility.FromJson<SerializableLevel>(json);

                List<Cell[,]> levelData = new List<Cell[,]>();

                foreach (SerializableGrid serializedGrid in serializableLevel.Grids)
                {
                    int rows = 8;
                    int cols = 8;

                    Cell[,] grid = new Cell[cols, rows];

                    foreach (SerializableCell serializedCell in serializedGrid.Cells)
                    {
                        int rowIndex = serializedCell.RowIndex;
                        int colIndex = serializedCell.ColIndex;
                        int id = serializedCell.ID;

                        Cell cell = new Cell(id, null, rowIndex, colIndex);

                        grid[colIndex, rowIndex] = cell;
                    }

                    levelData.Add(grid);
                }

                ApplyGridDataForLevel(levelData);
                Debug.Log($"Level {levelIndex + 1}: Grid data loaded and set of IDs completed.");
            }
            else
            {
                Debug.LogWarning($"Save data not found for Level {levelIndex + 1}.");
            }
        }

        private string GetJsonForLevel(int levelIndex)
        {
            TextAsset[] levelDatas = { levelData1, levelData2, levelData3, levelData4, levelData5 };
            if (levelIndex >= 0 && levelIndex < levelDatas.Length)
            {
                return levelDatas[levelIndex].text;
            }
            return null;
        }

        private void ApplyGridDataForLevel(List<Cell[,]> levelData)
        {
            for (int i = 0; i < gridLayers.Count; i++)
            {
                if (i < levelData.Count)
                {
                    Cell[,] gridData = levelData[i];
                    ApplyGridDataToGridLayer(gridData, gridLayers[i]);
                }
            }

            Debug.Log("Grid data applied to GridLayers.");
        }

        private void ApplyGridDataToGridLayer(Cell[,] gridData, GameObject gridLayer)
        {
            for (int row = 0; row < gridData.GetLength(1); row++)
            {
                for (int col = 0; col < gridData.GetLength(0); col++)
                {
                    Cell cell = gridData[col, row];

                    // Assuming each grid layer has a child for each cell in the grid
                    Transform gridCellTransform = gridLayer.transform.GetChild(row * gridData.GetLength(0) + col);

                    if (gridCellTransform != null)
                    {
                        GridCell gridCell = gridCellTransform.GetComponent<GridCell>();
                        gridCell.stoneId = cell.ID;
                    }
                }
            }

            Debug.Log($"Grid data applied to GridLayer: {gridLayer.name}.");
        }

       

    }
}
