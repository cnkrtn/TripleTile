using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class StoneEditorTool : EditorWindow
{
    private int cellSize = 40;
    private Stone[] stones = new Stone[5];
    private Stone selectedStone;
    private List<List<Cell[,]>> gridsByLevel = new List<List<Cell[,]>>();
    private int selectedGrid = 0;
    private int selectedLevel = 0;
  
    private int selectedGridIndex = 0;
    private int selectedLevelIndex = 0;

    private string[] levelOptions = new string[5] { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", };
    private string[] gridOptions = new string[3] { "Grid 1", "Grid 2", "Grid 3" };

    [MenuItem("Window/Stone Editor Tool")]
    private static void ShowWindow()
    {
        StoneEditorTool window = GetWindow<StoneEditorTool>();
        window.titleContent = new GUIContent("Stone Editor Tool");
        window.Show();
    }

    private void OnEnable()
    {
        // Initialize the stone buttons with different colors and sprites
        Sprite stone1Sprite = Resources.Load<Sprite>("Sprites/Red"); // Replace "Stone1Sprite" with the actual path to your sprite
        Sprite stone2Sprite = Resources.Load<Sprite>("Sprites/Green");
        Sprite stone3Sprite = Resources.Load<Sprite>("Sprites/Blue");
        Sprite stone4Sprite = Resources.Load<Sprite>("Sprites/Yellow");
        Sprite stone5Sprite = Resources.Load<Sprite>("Sprites/Orange");

        stones[0] = new Stone(1, Color.red, stone1Sprite);
        stones[1] = new Stone(2, Color.green, stone2Sprite);
        stones[2] = new Stone(3, Color.blue, stone3Sprite);
        stones[3] = new Stone(4, Color.yellow, stone4Sprite);
        stones[4] = new Stone(5, Color.cyan, stone5Sprite);

        // Add grids for each level
        for (int i = 0; i < 10; i++)
        {
            gridsByLevel.Add(CreateEmptyLevel());
        }

        for (int i = 0; i < gridsByLevel.Count; i++)
        {
            LoadGridsForLevel(i);
        }
        
        
    }

    private List<Cell[,]> CreateEmptyLevel()
    {
        List<Cell[,]> level = new List<Cell[,]>();
        for (int i = 0; i < 5; i++)
        {
            level.Add(CreateEmptyGrid());
        }
        return level;
    }

    private Cell[,] CreateEmptyGrid()
    {
        Cell[,] grid = new Cell[8, 8];
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                grid[col, row] = new Cell(0, null, col, row);
            }
        }
        return grid;
    }

    private void DrawSaveButton()
    {
        if (GUILayout.Button("Save All Levels"))
        {
            for (int levelIndex = 0; levelIndex < gridsByLevel.Count; levelIndex++)
            {
                SaveGridsForLevel(levelIndex);
            }
        }
    }

    private void DrawClearButtons()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid(selectedLevel,selectedGrid);
        }

        if (GUILayout.Button("Clear Level"))
        {
            ClearLevel(selectedLevel);
        }

        if (GUILayout.Button("Clear All"))
        {
            ClearAllGrids();
        }

        GUILayout.EndHorizontal();
    }
    private void ClearAllGrids()
    {
        // Loop through all levels and grids and reset them to empty grids
        for (int levelIndex = 0; levelIndex < gridsByLevel.Count; levelIndex++)
        {
            for (int gridIndex = 0; gridIndex < gridsByLevel[levelIndex].Count; gridIndex++)
            {
                ClearGrid(levelIndex, gridIndex);
            }
        }

        // Repaint the editor window to reflect the changes
        Repaint();
    }
    private void ClearLevel(int levelIndex)
    {
        // Check if the level index is within valid range
        if (levelIndex < 0 || levelIndex >= gridsByLevel.Count)
        {
            Debug.LogError("Invalid level index.");
            return;
        }

        // Reset all the grids in the selected level to empty grids
        for (int gridIndex = 0; gridIndex < gridsByLevel[levelIndex].Count; gridIndex++)
        {
            ClearGrid(levelIndex, gridIndex);
        }

        // Repaint the editor window to reflect the changes
        Repaint();
    }
    private void ClearGrid(int levelIndex, int gridIndex)
    {
        // Check if the level and grid are within valid ranges
        if (levelIndex < 0 || levelIndex >= gridsByLevel.Count || gridIndex < 0 || gridIndex >= gridsByLevel[levelIndex].Count)
        {
            Debug.LogError("Invalid level or grid index.");
            return;
        }

        // Reset the selected grid to an empty grid with all cells having ID=0 and no sprites
        Cell[,] grid = gridsByLevel[levelIndex][gridIndex];
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                grid[col, row] = new Cell(0, null, col, row);
            }
        }

        // Repaint the editor window to reflect the changes
        Repaint();
    }
    private void OnGUI()
    {
        GUILayout.Space(10);
        DrawButtons();

        GUILayout.Space(10);
        DrawDropdownMenus();

        GUILayout.Space(10);
        DrawGrid();

        GUILayout.Space(10);
        DrawSaveButton();

        GUILayout.Space(10);
        DrawClearButtons();
    }

    private void DrawButtons()
    {
        // Draw buttons in a row
        GUILayout.BeginHorizontal();
        foreach (Stone stone in stones)
        {
            if (GUILayout.Button(stone.Sprite.texture, GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
            {
                // Handle stone button click
                OnStoneButtonClick(stone);
            }
        }
        GUILayout.EndHorizontal();
    }

    private void OnStoneButtonClick(Stone stone)
    {
        selectedStone = stone;
    }

 
    private void DrawDropdownMenus()
    {
        GUILayout.BeginHorizontal();

        selectedLevelIndex = EditorGUILayout.Popup("Select Level", selectedLevelIndex, levelOptions);
        selectedLevel = selectedLevelIndex;
        Debug.Log("Selected Level: " + (selectedLevel + 1));

        selectedGridIndex = EditorGUILayout.Popup("Select Grid", selectedGridIndex, gridOptions);
        selectedGrid = selectedGridIndex;
        Debug.Log("Selected Grid: " + (selectedGrid + 1));

        GUILayout.EndHorizontal();
    }

    private void DrawGrid()
    {
        // Draw the selected grid
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label($"Level: {selectedLevel + 1}, Grid: {selectedGrid + 1}");

        GUILayout.BeginHorizontal();
        for (int row = 0; row < 8; row++)
        {
            GUILayout.BeginVertical();
            for (int col = 0; col < 8; col++)
            {
                Cell cell = gridsByLevel[selectedLevel][selectedGrid][col, row];
                if (cell.Sprite != null)
                {
                    if (GUILayout.Button(cell.Sprite.texture, GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                    {
                        OnCellClick(cell);
                    }
                }
                else
                {
                    if (GUILayout.Button("", GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                    {
                        OnCellClick(cell);
                    }
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    
    private void OnCellClick(Cell cell)
    {
        if (selectedStone != null)
        {
            cell.ID = selectedStone.ID;
            cell.Sprite = selectedStone.Sprite;

            int row =  cell.RowIndex;
            int column = cell.ColIndex;

            Debug.Log($"Cell Clicked: ID={cell.ID}, Sprite={cell.Sprite.name}, Row={row}, Column={column}");
        }
        else
        {
            Debug.Log("No stone selected. Click a stone button first.");
        }
    }

   

    private void SaveGridsForLevel(int levelIndex)
    {
        if (levelIndex >= gridsByLevel.Count)
        {
            Debug.LogError($"Level index {levelIndex} is out of range.");
            return;
        }

        List<Cell[,]> levelData = gridsByLevel[levelIndex];
        List<SerializableGrid> serializedGrids = new List<SerializableGrid>();

        for (int gridIndex = 0; gridIndex < levelData.Count; gridIndex++)
        {
            Cell[,] grid = levelData[gridIndex];
            List<SerializableCell> serializedCells = new List<SerializableCell>();

            for (int row = 0; row < 8; row++) // Loop through rows (grid size is 8x8)
            {
                for (int col = 0; col < 8; col++) // Loop through columns (grid size is 8x8)
                {
                    Cell cell = grid[col, row];
                    serializedCells.Add(new SerializableCell(cell));
                }
            }

            serializedGrids.Add(new SerializableGrid(gridIndex, serializedCells));
        }

        SerializableLevel serializableLevel = new SerializableLevel(levelIndex, serializedGrids);

        string savePath = GetSavePathForLevel(levelIndex);
        string json = JsonUtility.ToJson(serializableLevel, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Grid data saved for Level {levelIndex + 1} to: {savePath}");
    }


    private void LoadGridsForLevel(int levelIndex)
    {
        if (levelIndex >= gridsByLevel.Count)
        {
            Debug.LogError($"Level index {levelIndex} is out of range.");
            return;
        }

        string loadPath = GetSavePathForLevel(levelIndex);

        if (File.Exists(loadPath))
        {
            string json = File.ReadAllText(loadPath);
            SerializableLevel serializableLevel = JsonUtility.FromJson<SerializableLevel>(json);

            List<Cell[,]> levelData = new List<Cell[,]>();

            foreach (SerializableGrid serializedGrid in serializableLevel.Grids)
            {
                int rows = 8; // Assuming the grid size is always 8x8
                int cols = 8;

                Cell[,] grid = new Cell[cols, rows];

                foreach (SerializableCell serializedCell in serializedGrid.Cells)
                {
                    int rowIndex = serializedCell.RowIndex;
                    int colIndex = serializedCell.ColIndex;
                    int id = serializedCell.ID;

                    // Now set the Sprite for the cell
                    string spriteName = serializedCell.SpriteName;
                    Sprite sprite = LoadSpriteByName(spriteName);

                    Cell cell = new Cell(id, sprite, rowIndex, colIndex);

                    grid[colIndex, rowIndex] = cell;
                }

                levelData.Add(grid);
            }

            if (levelIndex < gridsByLevel.Count)
            {
                gridsByLevel[levelIndex] = levelData;
            }
            else
            {
                gridsByLevel.Add(levelData);
            }

            Debug.Log($"Grid data loaded for Level {levelIndex + 1} from: {loadPath}");
        }
        else
        {
            Debug.LogWarning($"Save data not found for Level {levelIndex + 1} at path: {loadPath}");
        }
    }

    private Sprite LoadSpriteByName(string spriteName)
    {
        string spritePath = "Sprites/" + spriteName;
        return Resources.Load<Sprite>(spritePath);
    }



    private string GetSavePathForLevel(int levelIndex)
    {
        string levelFolderPath = "Assets/LevelData";
        if (!Directory.Exists(levelFolderPath))
        {
            Directory.CreateDirectory(levelFolderPath);
        }

        return Path.Combine(levelFolderPath, $"Level_{levelIndex + 1}.json");
    }

    private void OnSelectionChange()
    {
        Repaint();
    }
}