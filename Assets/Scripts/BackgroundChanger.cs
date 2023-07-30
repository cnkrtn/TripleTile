
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundImages;
    [SerializeField] private Image image;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        image.sprite = backgroundImages[_gameManager.levelIndex];
    }
}
