using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerHandManager _playerHandManager;
        private void Awake()
        {
            _playerHandManager = FindObjectOfType<PlayerHandManager>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                if (!EventSystem.current.IsPointerOverGameObject()) return;
                
                var eventData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                // Create a list to store the raycast results
                var results = new List<RaycastResult>();

                // Raycast from the event data and store the results in the list
                EventSystem.current.RaycastAll(eventData, results);

                // Loop through the results to find the parent UI object with the GridStone script
                foreach (RaycastResult result in results)
                {
                    var gridStone = result.gameObject.GetComponentInParent<GridStone>();
                    if (gridStone == null) continue;
                    if (!gridStone.isClickable) return;
                    
                    Debug.Log("Hit successful: GridStone found!");
                    
                    _playerHandManager.MoveToPlayerHand(gridStone);
                    break;
                    
                    
                }
            }
        }

        
    }
}