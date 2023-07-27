using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class StoneEditorTool : EditorWindow
{
    private int cellSize = 40; // Size of each cell in the grid
    private Stone[] stones = new Stone[5]; // Array to store the stone buttons
    private Stone selectedStone; // Currently selected stone
    private List<List<Cell[,]>> gridsByLevel = new List<List<Cell[,]>>(); // List to store grids for each level
    private int selectedGrid = 0; // Default selected grid index is 0 (first grid)
    private int selectedLevel = 0; // Default selected level index is 0

    [MenuItem("Window/Stone Editor Tool")]
    private static void ShowWindow()
    {
        StoneEditorTool window = GetWindow<StoneEditorTool>();
        window.titleContent = new GUIContent("Stone Editor Tool");
        window.Show();
    }

    private void OnEnable()
    {
        // Initialize the stone buttons with different colors
        stones[0] = new Stone(1, Color.red);
        stones[1] = new Stone(2, Color.green);
        stones[2] = new Stone(3, Color.blue);
        stones[3] = new Stone(4, Color.yellow);
        stones[4] = new Stone(5, Color.cyan);

        // Add grids for each level
        for (int i = 0; i < 10; i++)
        {
            gridsByLevel.Add(CreateEmptyLevel());
        }
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawButtons();
        DrawDropdowns();
        DrawSaveButton();
    }

    private void DrawGrid()
    {
        List<Cell[,]> currentLevel = gridsByLevel[selectedLevel];
        Cell[,] currentGrid = currentLevel[selectedGrid];

        // Draw the 8x8 grid
        GUILayout.BeginVertical(EditorStyles.helpBox);
        for (int row = 7; row >= 0; row--)
        {
            GUILayout.BeginHorizontal();
            for (int col = 0; col < 8; col++)
            {
                if (currentGrid[col, row] == null)
                {
                    currentGrid[col, row] = new Cell(Color.white); // Initialize cell with default color (white) and ID 0
                }

                // Set the cell size and use the color from the Cell as background
                GUIStyle cellStyle = new GUIStyle(GUI.skin.button);
                cellStyle.fixedWidth = cellSize;
                cellStyle.fixedHeight = cellSize;
                cellStyle.normal.background = TextureFromColor(currentGrid[col, row].Color);

                if (GUILayout.Button("", cellStyle))
                {
                    // Handle cell click
                    OnCellClick(row, col, currentGrid);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void DrawButtons()
    {
        // Draw buttons in a row
        GUILayout.BeginHorizontal();
        foreach (Stone stone in stones)
        {
            if (GUILayout.Button($"Stone {stone.ID}"))
            {
                // Handle stone button click
                OnStoneButtonClick(stone);
            }
        }
        GUILayout.EndHorizontal();
    }

    private void DrawDropdowns()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Select Grid:");
        selectedGrid = EditorGUILayout.IntPopup(selectedGrid, new string[] { "Grid 1", "Grid 2", "Grid 3", "Grid 4", "Grid 5" }, new int[] { 0, 1, 2, 3, 4 });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Select Level:");
        selectedLevel = EditorGUILayout.IntPopup(selectedLevel, GetLevelNames(), GetLevelIndices());
        GUILayout.EndHorizontal();
    }

    private string[] GetLevelNames()
    {
        List<string> levelNames = new List<string>();
        for (int i = 0; i < gridsByLevel.Count; i++)
        {
            levelNames.Add($"Level {i + 1}");
        }
        return levelNames.ToArray();
    }

    private int[] GetLevelIndices()
    {
        int[] levelIndices = new int[gridsByLevel.Count];
        for (int i = 0; i < gridsByLevel.Count; i++)
        {
            levelIndices[i] = i;
        }
        return levelIndices;
    }

    private void SaveGrid(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= gridsByLevel.Count)
        {
            Debug.LogWarning("Invalid level index. Save failed.");
            return;
        }

        List<Cell[,]> currentLevel = gridsByLevel[levelIndex];
        Cell[,] currentGrid = currentLevel[selectedGrid];

        List<SerializableCell> serializableCells = new List<SerializableCell>();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Cell cell = currentGrid[col, row];
                serializableCells.Add(new SerializableCell(cell.RowIndex, cell.ColIndex, cell.ID));
            }
        }

        GridData gridData = new GridData
        {
            Cells = serializableCells.ToArray(),
            SelectedGrid = selectedGrid,
            SelectedLevel = selectedLevel
        };

        string savePath = GetSavePathForLevel(levelIndex);
        string jsonData = JsonUtility.ToJson(gridData, true);
        File.WriteAllText(savePath, jsonData);

        Debug.Log($"Grid data saved for Level {levelIndex + 1} to: {savePath}");
    }

    private string GetSavePathForLevel(int levelIndex)
    {
        string saveFolder = "Assets/LevelData";
        string fileName = $"Level{levelIndex + 1}GridData.json";
        return Path.Combine(saveFolder, fileName);
    }

    private void DrawSaveButton()
    {
        if (GUILayout.Button("Save All Levels"))
        {
            for (int levelIndex = 0; levelIndex < gridsByLevel.Count; levelIndex++)
            {
                SaveGrid(levelIndex);
            }
        }
    }

    private void OnCellClick(int row, int col, Cell[,] currentGrid)
    {
        // Handle cell click event
        Cell clickedCell = currentGrid[col, row];
        Debug.Log($"Cell ID: {clickedCell.ID} at ({clickedCell.RowIndex}, {clickedCell.ColIndex})");
        if (selectedStone != null)
        {
            clickedCell.Color = selectedStone.Color;
            clickedCell.ID = selectedStone.ID;
            Repaint(); // Repaint the editor window to reflect the color and ID change
        }
    }

    private void OnStoneButtonClick(Stone stone)
    {
        // Handle stone button click event
        selectedStone = stone;
        Debug.Log($"Stone {stone.ID} clicked with color {stone.Color}");
    }

    private void SaveGrid()
    {
        string savePath = EditorUtility.SaveFilePanel("Save Grid", "", "grid_data", "json");
        if (string.IsNullOrEmpty(savePath))
            return;

        List<Cell[,]> currentLevel = gridsByLevel[selectedLevel];
        Cell[,] currentGrid = currentLevel[selectedGrid];

        List<SerializableCell> serializableCells = new List<SerializableCell>();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Cell cell = currentGrid[col, row];
                serializableCells.Add(new SerializableCell(cell.RowIndex, cell.ColIndex, cell.ID));
            }
        }

        GridData gridData = new GridData
        {
            Cells = serializableCells.ToArray(),
            SelectedGrid = selectedGrid,
            SelectedLevel = selectedLevel
        };

        string jsonData = JsonUtility.ToJson(gridData, true);
        File.WriteAllText(savePath, jsonData);

        Debug.Log("Grid data saved successfully!");
    }

    private List<Cell[,]> CreateEmptyLevel()
    {
        List<Cell[,]> levelGrids = new List<Cell[,]>();
        for (int i = 0; i < 5; i++)
        {
            levelGrids.Add(CreateEmptyGrid());
        }
        return levelGrids;
    }

    private Cell[,] CreateEmptyGrid()
    {
        Cell[,] grid = new Cell[8, 8];
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                grid[col, row] = new Cell(Color.white)
                {
                    RowIndex = row,
                    ColIndex = col
                };
            }
        }
        return grid;
    }

    private Texture2D TextureFromColor(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}


