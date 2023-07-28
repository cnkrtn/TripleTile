using System.Collections.Generic;

[System.Serializable]
public class SerializableGrid
{
    public int Index; // Add an index to identify the grid
    public List<SerializableCell> Cells;

    public SerializableGrid(int index, List<SerializableCell> cells)
    {
        Index = index;
        Cells = cells;
    }
}