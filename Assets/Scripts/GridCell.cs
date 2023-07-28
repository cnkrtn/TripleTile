using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private int rowIndex;
    [SerializeField] private  int colIndex;
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