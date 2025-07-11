using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Coroutine _currentCoroutine;

    [SerializeField] private GameObject _cardBackPrefab;
    [SerializeField] private Transform _cardBackParent;

    private GameDrawer _gameDrawer;

    private TimeSpan _currentAnimationTime;


    public void ManualStart(GameDrawer gameDrawer)
    {
        _gameDrawer = gameDrawer;
    }

    public void ManualUpdate()
    {
        _currentAnimationTime += TimeSpan.FromSeconds(Time.deltaTime);
    }



}
