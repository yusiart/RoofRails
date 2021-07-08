using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Image _startGame;

    public event UnityAction GameStarted;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startGame.gameObject.SetActive(false);
            GameStarted?.Invoke();
            enabled = false;
        }
    }
}