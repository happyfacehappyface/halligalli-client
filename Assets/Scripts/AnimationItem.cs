using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AnimationItem
{
    public void OnStartAnimation(GameDrawer gameDrawer, AnimationHandler animationHandler);
    public void OnUpdateAnimation();
    public void OnEndAnimation();
    public bool IsAnimationFinished();
}

public class EmptyAnimationItem : AnimationItem
{
    public void OnStartAnimation(GameDrawer gameDrawer, AnimationHandler animationHandler)
    {

    }

    public void OnUpdateAnimation()
    {

    }

    public void OnEndAnimation()
    {

    }

    public bool IsAnimationFinished()
    {
        return true;
    }
}

public class WrongAnimationItem : AnimationItem
{
    private GameDrawer _gameDrawer;
    private AnimationHandler _animationHandler;
    private bool[] _cardGivenTo;
    private int _wrongPlayerIndex;
    private TimeSpan _duration;
    private TimeSpan _startAnimationTime;

    private Transform[] _cardBacks;

    public WrongAnimationItem(int wrongPlayerIndex, bool[] cardGivenTo)
    {
        _wrongPlayerIndex = wrongPlayerIndex;
        _cardGivenTo = cardGivenTo;
        _duration = TimeSpan.FromSeconds(1f);
    }

    public void OnStartAnimation(GameDrawer gameDrawer, AnimationHandler animationHandler)
    {
        _gameDrawer = gameDrawer;
        _animationHandler = animationHandler;
        _startAnimationTime = _animationHandler.CurrentAnimationTime;

        _cardBacks = new Transform[_gameDrawer.GetPlayerCount()];
        bool isCardGiven = false;

        for (var i = 0; i < _cardBacks.Length; i++)
        {
            if (_cardGivenTo[i])
            { 
                var cardBack = _animationHandler.CreateCardBack();
                _cardBacks[i] = cardBack.transform;
                cardBack.transform.localRotation = _gameDrawer.GetPlayerRotation(i);
                cardBack.transform.localPosition = _gameDrawer.GetPlayerVector(i) * 210f;
                isCardGiven = true;
            }

        }

        if (isCardGiven)
        {
            SoundManager.Instance.PlaySfxCardMove(0f);
        }
    }

    public void OnUpdateAnimation()
    {
        Vector2 originPosition = _gameDrawer.GetPlayerVector(_wrongPlayerIndex) * 360f;
        float originAngle = _gameDrawer.GetPlayerAngle(_wrongPlayerIndex);
        float progress = (float)(_animationHandler.CurrentAnimationTime - _startAnimationTime).TotalSeconds / (float)_duration.TotalSeconds;

        for (var i = 0; i < _cardBacks.Length; i++)
        {
            if (_cardGivenTo[i])
            {
                Vector2 destinationPosition = _gameDrawer.GetPlayerVector(i) * 360f;
                float destinationAngle = _gameDrawer.GetPlayerAngle(i);

                float subProgress = Utils.GetEaseInOutProgress(0f, 1f, progress);

                _cardBacks[i].localPosition = Vector2.Lerp(originPosition, destinationPosition, subProgress);
                _cardBacks[i].localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(originAngle, destinationAngle, subProgress));
            }
            

        }

    }

    public void OnEndAnimation()
    {
        _animationHandler.ClearAllCardBack();
    }

    public bool IsAnimationFinished()
    {
        return _animationHandler.CurrentAnimationTime - _startAnimationTime > _duration;
    }
}

public class CorrectAnimationItem : AnimationItem
{
    private GameDrawer _gameDrawer;
    private AnimationHandler _animationHandler;
    private TimeSpan _startAnimationTime;
    private int[] _cardOpenCount;
    private int _correctPlayerIndex;
    private TimeSpan _duration;

    private List<Transform>[] _cardBacks;
    private int _cardMaxCount = 0;

    public CorrectAnimationItem(int correctPlayerIndex, int[] cardOpenCount)
    {
        _correctPlayerIndex = correctPlayerIndex;
        _cardOpenCount = cardOpenCount;
        _duration = TimeSpan.FromSeconds(1f);
    }

    public void OnStartAnimation(GameDrawer gameDrawer, AnimationHandler animationHandler)
    {
        _gameDrawer = gameDrawer;
        _animationHandler = animationHandler;
        _startAnimationTime = _animationHandler.CurrentAnimationTime;

        _cardBacks = new List<Transform>[_gameDrawer.GetPlayerCount()];

        for (var i = 0; i < _cardBacks.Length; i++)
        {
            _cardMaxCount = Mathf.Max(_cardMaxCount, _cardOpenCount[i]);
            _cardBacks[i] = new List<Transform>();
            for (var j = 0; j < _cardOpenCount[i]; j++)
            {
                var cardBack = _animationHandler.CreateCardBack();
                _cardBacks[i].Add(cardBack.transform);
                cardBack.transform.localRotation = _gameDrawer.GetPlayerRotation(i);
                cardBack.transform.localPosition = _gameDrawer.GetPlayerVector(i) * 210f;

                
            }
        }

        for (var i = 0; i < _cardMaxCount; i++)
        {
            SoundManager.Instance.PlaySfxCardMove((float)_duration.TotalSeconds * 0.5f * ((float)i / (float)_cardMaxCount));
        }

    }

    public void OnUpdateAnimation()
    {
        Vector2 destinationPosition = _gameDrawer.GetPlayerVector(_correctPlayerIndex) * 360f;
        float destinationAngle = _gameDrawer.GetPlayerAngle(_correctPlayerIndex);
        float progress = (float)(_animationHandler.CurrentAnimationTime - _startAnimationTime).TotalSeconds / (float)_duration.TotalSeconds;

        for (var i = 0; i < _cardBacks.Length; i++)
        {
            Vector2 originPosition = _gameDrawer.GetPlayerVector(i) * 210f;
            float originAngle = _gameDrawer.GetPlayerAngle(i);
            for (var j = 0; j < _cardBacks[i].Count; j++)
            {
                float progressStart;
                if (_cardMaxCount == 0)
                {
                    progressStart = 0f;
                }
                else
                {
                    progressStart = 0f + 0.5f * ((float)j / (float)_cardMaxCount);
                }

                float subProgress = Utils.GetEaseInOutProgress(progressStart, progressStart + 0.5f, progress);

                _cardBacks[i][j].localPosition = Vector2.Lerp(originPosition, destinationPosition, subProgress);
                _cardBacks[i][j].localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(originAngle, destinationAngle, subProgress));
            }
        }


    }

    public void OnEndAnimation()
    {
        _animationHandler.ClearAllCardBack();
    }

    public bool IsAnimationFinished()
    {
        return _animationHandler.CurrentAnimationTime - _startAnimationTime > _duration;
    }


}




