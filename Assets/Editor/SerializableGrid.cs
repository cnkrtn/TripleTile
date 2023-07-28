using System.Collections.Generic;

[System.Serializable]
public class SerializableGrid
{
    public List<SerializableCell> Cells;

    public SerializableGrid(List<SerializableCell> cells)
    {
        Cells = cells;
    }
}
