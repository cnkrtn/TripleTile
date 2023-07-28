[System.Serializable]
public class SerializableCell
{
    public int ID;
    public int RowIndex;
    public int ColIndex;
    public string SpriteName; // Store the sprite name instead of the actual Sprite

   
    public SerializableCell(Cell cell)
    {
        ID = cell.ID;
        RowIndex = cell.RowIndex;
        ColIndex = cell.ColIndex;
        SpriteName = cell.Sprite != null ? cell.Sprite.name : ""; // Store the Sprite name
    }
}