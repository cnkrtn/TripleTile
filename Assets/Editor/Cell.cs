using UnityEngine;

[System.Serializable]
public class Cell
{
    public int ID;
    public Color Color;
    public int RowIndex;
    public int ColIndex;

    public Cell(int id, Color color, int rowIndex, int colIndex)
    {
        ID = id;
        Color = color;
        RowIndex = rowIndex;
        ColIndex = colIndex;
    }
}