using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    [SerializeField] private GameObject _cardBackPrefab;
    [SerializeField] private Transform _cardBackParent;

    private GameDrawer _gameDrawer;

    private TimeSpan _currentAnimationTime;

    public TimeSpan CurrentAnimationTime => _currentAnimationTime;

    private AnimationItem _currentAnimationItem;


    public void ManualStart(GameDrawer gameDrawer)
    {
        _gameDrawer = gameDrawer;
        _currentAnimationTime = TimeSpan.Zero;
        _currentAnimationItem = new EmptyAnimationItem();
    }

    public void ManualUpdate()
    {
        _currentAnimationTime += TimeSpan.FromSeconds(Time.deltaTime);


        if (_currentAnimationItem.IsAnimationFinished())
        {
            _currentAnimationItem.OnEndAnimation();
            _currentAnimationItem = new EmptyAnimationItem();
        }
        else
        {
            _currentAnimationItem.OnUpdateAnimation();
        }
    }

    public void OnStartNewAnimation(AnimationItem newAnimationItem)
    {
        _currentAnimationItem.OnEndAnimation();
        _currentAnimationItem = newAnimationItem;
        _currentAnimationItem.OnStartAnimation(_gameDrawer, this);
    }

    public void ClearAllCardBack()
    {
        foreach (Transform child in _cardBackParent)
        {
            Destroy(child.gameObject);
        }
    }

    public GameObject CreateCardBack()
    {
        var cardBack = Instantiate(_cardBackPrefab, _cardBackParent);
        return cardBack;
    }



}
