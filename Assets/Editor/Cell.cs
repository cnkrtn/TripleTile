using UnityEngine;

[System.Serializable]
public class Cell
{
    public int ID { get; set; } = 0; // Default ID is 0
    public Color Color { get; set; }
    public int RowIndex { get; set; } // Cell's row index in the grid
    public int ColIndex { get; set; } // Cell's column index in the grid

    public Cell(Color color)
    {
        Color = color;
    }
}