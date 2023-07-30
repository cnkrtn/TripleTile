using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class Cell
    {
        public int ID;
        public int RowIndex;
        public int ColIndex;
        public Sprite Sprite;
    
        public Cell(int id, Sprite sprite, int rowIndex, int colIndex)
        {
            ID = id;
            Sprite = sprite;
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }
    }
}