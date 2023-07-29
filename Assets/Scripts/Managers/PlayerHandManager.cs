using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class PlayerHandManager : MonoBehaviour
    {
        public List<GridStone> playerHandStones;
        public List<RectTransform> playerHandSlots;
       [SerializeField] private int previousSlotsId;

        [SerializeField] private float slideDuration, moveDuration, destroyDuration;


        public void MoveToPlayerHand(GridStone gridStone)
        {
            for (var i = 0; i < playerHandSlots.Count; i++)
            {
                var slot = playerHandSlots[i].GetComponent<Slot>();
                if (!slot.isOccupied)
                {
                    Move(gridStone, i, slot);
                    previousSlotsId = 0;
                    Debug.Log("Section 1 Passed!");
                    return;
                }

                if (previousSlotsId == gridStone.stoneID)
                {

                    MoveRest(i,true,gridStone,slot);
                    
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

        private void MoveRest(int i,bool moveRight,GridStone gridStone, Slot slot)
        {
            if (moveRight)
            {
                for (var j = playerHandSlots.Count-1 ; j >= i ; j--)
                {
                    if (playerHandSlots[j].transform.childCount == 0) continue;
                  
                    var stoneObject = (RectTransform)playerHandSlots[j].transform.GetChild(0);
                    var nextSlot = playerHandSlots[j + 1].GetComponent<Slot>();
                    if (stoneObject == null) continue;
                    stoneObject.SetParent(playerHandSlots[j + 1]);
                    stoneObject.DOMove(playerHandSlots[j + 1].position, slideDuration);
                    nextSlot.occupyingId = stoneObject.GetComponent<GridStone>().stoneID;
                    nextSlot.isOccupied = true;

                    
                }
                Move(gridStone, i, slot);
            }
            else
            {
                
            }
          
        }


        private void Move(GridStone gridStone, int i, Slot slot)
        {
            gridStone.transform.SetParent(playerHandSlots[i]);
            var rectTransform = (RectTransform)gridStone.transform;
            rectTransform.DOMove(slot.transform.position, moveDuration);
            slot.occupyingId = gridStone.stoneID;
            slot.isOccupied = true;
        }
    }
}
