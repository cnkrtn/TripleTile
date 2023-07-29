using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    private UIManager _uiManager;
    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void Count()
    {
        _uiManager.ScoreCounter();
    }
}
