
using System.Collections.Generic;
using Data;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundImages;
    [SerializeField] private Image image;
    private SaveLoadManager _saveLoadManager;

    private void Start()
    {
        _saveLoadManager = FindObjectOfType<SaveLoadManager>();
        image.sprite = backgroundImages[_saveLoadManager.levelIndex];
    }
}
