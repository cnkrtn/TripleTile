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

    private string[] levelOptions = new string[10] { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9", "Level 10" };
    private string[] gridOptions = new string[5] { "Grid 1", "Grid 2", "Grid 3", "Grid 4", "Grid 5" };

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
                grid[col, row] = new Cell(0, Color.white, col, row);
            }
        }
        return grid;
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
                GUI.backgroundColor = cell.Color;
                if (GUILayout.Button("", GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                {
                    OnCellClick(cell);
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUI.backgroundColor = Color.white;
    }

    private void OnCellClick(Cell cell)
    {
        if (selectedStone != null)
        {
            cell.ID = selectedStone.ID;
            cell.Color = selectedStone.Color;

            int row = 7 - cell.RowIndex; // Calculate the row index with respect to the bottom row being 0
            int column = cell.ColIndex;

            Debug.Log($"Cell Clicked: ID={cell.ID}, Color={cell.Color}, Row={row}, Column={column}");
        }
        else
        {
            Debug.Log("No stone selected. Click a stone button first.");
        }
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

    private void SaveGridsForLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= gridsByLevel.Count)
        {
            Debug.LogWarning("Invalid level index. Save failed.");
            return;
        }

        List<Cell[,]> levelGrids = gridsByLevel[levelIndex];

        for (int gridIndex = 0; gridIndex < levelGrids.Count; gridIndex++)
        {
            Cell[,] currentGrid = levelGrids[gridIndex];

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
                SelectedGrid = gridIndex,
                SelectedLevel = levelIndex
            };

            string savePath = GetSavePathForGrid(levelIndex, gridIndex);
            string jsonData = JsonUtility.ToJson(gridData, true);
            File.WriteAllText(savePath, jsonData);

            Debug.Log($"Grid data saved for Level {levelIndex + 1}, Grid {gridIndex + 1} to: {savePath}");
        }
    }

    private string GetSavePathForGrid(int levelIndex, int gridIndex)
    {
        string saveFolder = "Assets/LevelData";
        string fileName = $"Level{levelIndex + 1}_Grid{gridIndex + 1}_Data.json";
        return Path.Combine(saveFolder, fileName);
    }

    private void OnSelectionChange()
    {
        Repaint();
    }
}
