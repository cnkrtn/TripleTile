[System.Serializable]
public class SerializableCell
{
    public int rowIndex;
    public int colIndex;
    public int ID;
    

    public SerializableCell(int rowIndex, int colIndex, int ID)
    {
        this.rowIndex = rowIndex;
        this.colIndex = colIndex;
        this.ID = ID;
        
    }
}
