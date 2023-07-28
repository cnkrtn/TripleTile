using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] public int rowIndex;
    [SerializeField] public  int colIndex;
    public int stoneId;
    
    

    private void Awake()
    {
        
        var gridLayer = transform.parent;
        
        var cellIndex = transform.GetSiblingIndex();
        
        
        var gridSize = Mathf.RoundToInt(Mathf.Sqrt(gridLayer.childCount));
        rowIndex = cellIndex / gridSize;
        colIndex = cellIndex % gridSize;

        
    }
}