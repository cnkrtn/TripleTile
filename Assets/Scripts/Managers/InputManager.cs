using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerHandManager _playerHandManager;
        private GameManager _gameManager;
        public float hintTimer,clickTimer;
        public bool canHint,canClick;
        private void Awake()
        {
            _playerHandManager = FindObjectOfType<PlayerHandManager>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            if (canHint)
            {
                HintTimer();
            }

           
            ClickTimer();
            

            if (Input.GetMouseButtonDown(0) && canClick)
            {
                   
                canClick = false;
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
                    hintTimer = 0;
                    canHint = true;
                    break;
                    
                    
                }
            }
        }

        private void ClickTimer()
        { 
            clickTimer += Time.deltaTime;
            if (!(hintTimer >= 1)) return;
            canClick = true;
            clickTimer = 0;
        }
        private void HintTimer()
        {
            hintTimer += Time.deltaTime;
            if (!(hintTimer >= 10)) return;
            canHint = false;
            Hint();
        }

        private void Hint()
        {
            if (_playerHandManager.playerHandStones.Count <= 0) return;
            _gameManager.slotsToShuffle.Clear();
            _gameManager.AddToListToShuffle();
            List<RectTransform> stones = new();
            bool canBreakLoop;
            canBreakLoop = false;
            foreach (var slot in _gameManager.slotsToShuffle)
            {
                var stone = slot.transform.GetChild(0).GetComponent<GridStone>();
                if (stone.isClickable)
                {
                    stones.Add((RectTransform)stone.transform);
                }
            }

            var i = UnityEngine.Random.Range(0, _playerHandManager.playerHandStones.Count);
            foreach (var stone in stones)
            {
                if (_playerHandManager.playerHandStones[i].stoneID !=
                    stone.GetComponent<GridStone>().stoneID) continue;
                stone.transform.DOScale(Vector3.one * 1.2f, 1f).SetLoops(2,LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        // stone.transform.DOScale(Vector3.one , 1.5f);
                        hintTimer = 0;
                        canHint = true;
                    });
                        
                _playerHandManager.playerHandStones[i].transform.DOScale(Vector3.one * 1.2f, 1.5f).SetLoops(2,LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        //_playerHandManager.playerHandStones[i].transform.transform.DOScale(Vector3.one , 1.5f);
                        hintTimer = 0;
                        canHint = true;
                    });

                   

            }

               
        }
        }
    }
