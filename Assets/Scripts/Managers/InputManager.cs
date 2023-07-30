using System;
using System.Collections.Generic;
using DG.Tweening;
using InGame;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerHandManager _playerHandManager;
        private HintLogic _hintLogic;
        public float clickTimer;
        public bool canClick;
        private Touch _touch;

        private void Awake()
        {
            _playerHandManager = FindObjectOfType<PlayerHandManager>();
            _hintLogic = FindObjectOfType<HintLogic>();
        }

        private void Update()
        {
            ClickTimer();
            SelectStone();
        }

        private void SelectStone()
        {
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && canClick)
            {
                canClick = false;


                Vector2 inputPosition = Input.mousePosition;
                if (Input.touchCount > 0)
                {
                    _touch = Input.GetTouch(0);
                    inputPosition = _touch.position;
                }


                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = inputPosition },
                    results);

                foreach (RaycastResult result in results)
                {
                    var gridStone = result.gameObject.GetComponentInParent<GridStone>();
                    if (gridStone == null) continue;
                    if (!gridStone.isClickable) return;

                    Debug.Log("Hit successful: GridStone found!");

                    _playerHandManager.MoveToPlayerHand(gridStone);
                    _hintLogic.hintTimer = 0;
                    _hintLogic.canHint = true;
                    break;
                }
            }
        }

        private void ClickTimer()
        {
            clickTimer += Time.deltaTime;
            if (!(clickTimer >= 0.8f)) return;
            canClick = true;
            clickTimer = 0;
        }
    }
}