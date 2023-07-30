using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;

public class HintLogic:MonoBehaviour
{
    private PlayerHandManager _playerHandManager;
    private GameManager _gameManager;
    public bool canHint;
    public float hintTimer;
    
    private void Start()
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
    }

    private void Hint()
    {
        if (_playerHandManager.playerHandStones.Count <= 0) return;
        _gameManager.slotsToShuffle.Clear();
        _gameManager.AddToListToShuffle();
        List<RectTransform> stones = new();
         
        foreach (var slot in _gameManager.slotsToShuffle)
        {
            var stone = slot.transform.GetComponentInChildren<GridStone>();
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
                    hintTimer = 0;
                    canHint = true;
                });
                        
            _playerHandManager.playerHandStones[i].transform.DOScale(Vector3.one * 1.2f, 1.5f).SetLoops(2,LoopType.Yoyo)
                .OnComplete(() =>
                {
                    hintTimer = 0;
                    canHint = true;
                });
        }
    }
    
    private void HintTimer()
    {
        hintTimer += Time.deltaTime;
        if (!(hintTimer >= 10)) return;
        canHint = false;
        Hint();
    }
}