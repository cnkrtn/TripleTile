using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Managers
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Level Data")]
       // public int levelIndex; 
        public List<GameObject> gridLayers; 

       

        private string GetSavePathForLevel(int levelIndex)
        {
            string levelFolderPath = "Assets/LevelData";
            if (!Directory.Exists(levelFolderPath))
            {
                Directory.CreateDirectory(levelFolderPath);
            }

            return Path.Combine(levelFolderPath, $"Level_{levelIndex + 1}.json");
        }

        private void LoadGridsForLevel(int levelIndex)
        {
            string loadPath = GetSavePathForLevel(levelIndex);

            if (File.Exists(loadPath))
            {
                string json = File.ReadAllText(loadPath);
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
                Debug.LogWarning($"Save data not found for Level {levelIndex + 1} at path: {loadPath}");
            }
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

        
        public void LoadLevel(int levelIndex)
        {
            LoadGridsForLevel(levelIndex);
        }
    }
}
