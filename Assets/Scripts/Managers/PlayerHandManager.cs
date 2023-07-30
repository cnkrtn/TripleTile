
using System.Collections.Generic;
using DG.Tweening;
using InGame;
using UnityEngine;


namespace Managers
{
    public class PlayerHandManager : MonoBehaviour
    {
        public List<GridStone> playerHandStones;
        public List<RectTransform> playerHandSlots;
        [SerializeField] private RectTransform lastPieceGridObject,lastMovedPiece;
        [SerializeField] private float moveDuration;
        private GameManager _gameManager;
        private ShuffleLogic _shuffleLogic;
        
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _shuffleLogic = FindObjectOfType<ShuffleLogic>();
        }
        
        public void MoveToPlayerHand(GridStone gridStone)
        {
            if(playerHandStones.Count>=playerHandSlots.Count) return;
            
            for (var i = 0; i < playerHandSlots.Count; i++)
            {
                var slot = playerHandSlots[i].GetComponent<Slot>();
                
                
                if (i  < playerHandStones.Count && playerHandStones[i].stoneID == gridStone.stoneID)
                {
                    if (i + 1 < playerHandStones.Count && playerHandStones[i + 1].stoneID == gridStone.stoneID)
                    {
                        AddToList(gridStone,i+2);
                        return;
                    }

                    AddToList(gridStone,i+1);
                    return;
                }

                if (slot.isOccupied) continue;
                AddToList(gridStone, i);
                Debug.Log("Section 1 Passed!");
                return;
            }
        }
        private void AddToList(GridStone gridStone, int i)
        {
            playerHandStones.Insert(i,gridStone);
            gridStone.isClickable = false;
            lastMovedPiece = (RectTransform)gridStone.transform;
            lastPieceGridObject = (RectTransform)lastMovedPiece.parent.transform;
            
            MoveStones();
            EventManager.OnStoneAddedToPlayerHand?.Invoke();
            
            
        }
        
        private void MoveStones()
        {
            for (var j = 0; j < playerHandStones.Count; j++)
            {
                if (playerHandStones[j] == null) continue;
                    
                var stoneObject = (RectTransform)playerHandStones[j].transform;
                var slotToMove = playerHandSlots[j].GetComponent<Slot>();

                var position = (RectTransform)playerHandSlots[j].transform;
                // if (stoneObject.parent.GetComponent<Slot>())
                // {
                //     stoneObject.DOMove(position.position, slideDuration).SetEase(Ease.InBack,0.5f,0.5f); 
                // }
                // else
                // {
                //     stoneObject.DOMove(position.position, moveDuration).SetEase(Ease.InBack,0.5f,0.5f); 
                // }
                stoneObject.DOMove(position.position, moveDuration).SetEase(Ease.InBack,0.5f,0.5f); 
                stoneObject.SetParent(playerHandSlots[j]);
                slotToMove.occupyingId = playerHandStones[j].stoneID;
                slotToMove.isOccupied = true;
            }

            ClearSlotOccupation();
            MergeAndDestroy();
        }
        
         private void ClearSlotOccupation()
         {
             foreach (var slot in playerHandSlots)
             {
                 if (slot.childCount > 0) continue;
                 slot.GetComponent<Slot>().isOccupied = false;
                 slot.GetComponent<Slot>().occupyingId = 0;
             }
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


                if (stone1.stoneID != stone2.stoneID || stone1.stoneID != stone3.stoneID) continue;
                for (var i = 0; i < 3; i++)
                {
                    Destroy(playerHandSlots[a + i].GetChild(0).gameObject,1f);
                    playerHandSlots[a + i].GetChild(0).transform.DOScale(Vector3.zero, 0.5f).SetDelay(0.5f).SetEase(Ease.InBack,2.5f,0.5f);
                    _gameManager.CheckTotalStoneCount();
                    playerHandStones.Remove(playerHandSlots[a + i].GetComponentInChildren<GridStone>());
                    playerHandSlots[a + i].GetComponent<Slot>().isOccupied = false;
                    playerHandSlots[a + i].GetComponent<Slot>().occupyingId = 0;
                    
                }
                Invoke(nameof(MoveStones),0.7f);
            }
        }
     
       

        
    }
}
