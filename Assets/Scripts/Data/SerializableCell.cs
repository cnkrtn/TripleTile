[System.Serializable]
public class SerializableCell
{
    public int ID;
    public int RowIndex;
    public int ColIndex;
    public string SpriteName;

    public SerializableCell(Cell cell)
    {
        ID = cell.ID;
        RowIndex = cell.RowIndex;
        ColIndex = cell.ColIndex;
        SpriteName = cell.Sprite != null ? cell.Sprite.name : "";
    }
}