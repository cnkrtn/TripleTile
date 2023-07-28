using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {

        public int levelIndex;

        private LevelLoader _levelLoader;
        public List<GameObject> gridLayers;
        public List<GameObject> stones;
        
        // Start is called before the first frame update
        void Awake()
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
            gridLayers = _levelLoader.gridLayers;
        }

        private void Start()
        {
            _levelLoader.LoadLevel(levelIndex);
            SetTheStones();

        }

        private void SetTheStones()
        {
            foreach (var grid in gridLayers)
            {
                for (int i = 0; i < grid.transform.childCount; i++)
                {
                    var gridCell = grid.transform.GetChild(i).GetComponent<GridCell>();
                    if (gridCell.stoneId <= 0 || gridCell.stoneId >= stones.Count) continue;
                    var stone = Instantiate(stones[gridCell.stoneId], grid.transform.position, quaternion.identity);
                    stone.SetActive(true);
                    stone.transform.SetParent(gridCell.transform);
                    Debug.Log("Done");

                }
            }
            
           
        }
       
    }
}
