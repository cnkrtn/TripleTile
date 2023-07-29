using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class PlayerHandManager : MonoBehaviour
    {
        public List<GridStone> playerHandStones;
        public List<RectTransform> playerHandSlots;
        [SerializeField] private RectTransform lastPieceGridObject,lastMovedPiece;
       [SerializeField] private int previousSlotsId;
       [SerializeField] private float slideDuration, moveDuration, destroyDuration;


        public void MoveToPlayerHand(GridStone gridStone)
        {
            if(playerHandStones.Count>=playerHandSlots.Count) return;
            
            for (var i = 0; i < playerHandSlots.Count; i++)
            {
                var slot = playerHandSlots[i].GetComponent<Slot>();
                if (!slot.isOccupied)
                {
                    AddToList(gridStone, i);
                    previousSlotsId = 0;
                    Debug.Log("Section 1 Passed!");
                    return;
                }

                if (previousSlotsId == gridStone.stoneID)
                {
                   
                    AddToList(gridStone, i);
                    
                    
                    Debug.Log("Section 2 Passed!");
                    return;
                }

                if (gridStone.stoneID == slot.occupyingId)
                {
                    previousSlotsId = slot.occupyingId;
                    Debug.Log("Section 3 Passed!");
                }

            }

              
        }

         private void MoveStones()
            {
                for (int j = 0; j < playerHandStones.Count; j++)
                {
                    if (playerHandStones[j] == null) continue;
                    
                    var stoneObject = (RectTransform)playerHandStones[j].transform;
                    var slotToMove = playerHandSlots[j].GetComponent<Slot>();
                    stoneObject.SetParent(playerHandSlots[j]);
                    stoneObject.DOLocalMove(Vector3.zero, slideDuration); 
                    slotToMove.occupyingId = playerHandStones[j].stoneID;
                    slotToMove.isOccupied = true;
                }  
            }
            
        


        private void AddToList(GridStone gridStone, int i)
        {
            playerHandStones.Insert(i,gridStone);
            gridStone.isClickable = false;
            lastMovedPiece = (RectTransform)gridStone.transform;
            lastPieceGridObject = (RectTransform)lastMovedPiece.parent.transform;
            
            MoveStones();
            MergeAndDestroy();
            EventManager.OnStoneAddedToPlayerHand?.Invoke();
            
            
        }

        private void MergeAndDestroy()
        {
            if (playerHandStones.Count < 2) return;
            for (var a = 0; a < playerHandStones.Count - 2; a++)
            {
                Debug.Log("!!!!");
                var stone1 = playerHandStones[a].GetComponent<GridStone>();
                var stone2 = playerHandStones[a + 1].GetComponent<GridStone>();
                var stone3 = playerHandStones[a + 2].GetComponent<GridStone>();


               
                if (stone1.stoneID == stone2.stoneID && stone1.stoneID == stone3.stoneID)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(playerHandSlots[a + i].GetChild(0).gameObject);
                        playerHandStones.Remove(playerHandSlots[a + i].GetComponentInChildren<GridStone>());
                        playerHandSlots[a + i].GetComponent<Slot>().isOccupied = false;
                        playerHandSlots[a + i].GetComponent<Slot>().occupyingId = 0;
                    }
                   
                    // playerHandStones.Remove(stone1);
                    // playerHandStones.Remove(stone2);
                    // playerHandStones.Remove(stone3);
               
                    MoveStones();
                  
                }
            }
        }

        public void ReturnTheLastPieceToOrigin()
        {
            if(lastMovedPiece == null) return;
            var slotToMoveFrom = lastMovedPiece.parent.transform.GetComponent<Slot>();
            lastMovedPiece.SetParent(lastPieceGridObject);
            lastMovedPiece.DOLocalMove(Vector3.zero, slideDuration); 
            slotToMoveFrom.occupyingId = 0;
            slotToMoveFrom.isOccupied = false;
            lastMovedPiece.GetComponent<GridStone>().isClickable = true;
            playerHandStones.Remove(lastMovedPiece.GetComponent<GridStone>());
            lastMovedPiece = null;
            lastPieceGridObject = null;
        }
    }
}