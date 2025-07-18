using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameDrawer : MonoBehaviour
{
    private GameController _controller;
    private int _myPlayerIndex;
    private int _playerCount;
    [SerializeField] private AnimationHandler _animationHandler;
    [SerializeField] private EffectHandler _effectHandler;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private Transform _playerItemParent;

    [SerializeField] private GameObject _portraitPrefab;
    [SerializeField] private Transform _portraitParent;

    
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private Image[] _handImages;


    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private TextMeshProUGUI _openCardText;

    [SerializeField] private TextMeshProUGUI _fruitVariationText;
    [SerializeField] private TextMeshProUGUI _fruitCountText;
    [SerializeField] private TextMeshProUGUI _gameTempoText;


    [SerializeField] private TextMeshProUGUI _slowText;
    [SerializeField] private Animator _slowAnimator;

    private TimeSpan _lastSlowInfoTime;

    private PlayerComponent[] _playerComponents;
    private PortraitComponent[] _portraitComponents;

    public void ManualStart(GameController controller)
    {
        _controller = controller;
        _animationHandler.ManualStart(this);
        _effectHandler.ManualStart(this);
        _lastSlowInfoTime = TimeSpan.FromSeconds(-10);
    }

    public void ManualUpdate()
    {
        _animationHandler.ManualUpdate();
        _effectHandler.ManualUpdate();
        _timeText.text = Math.Max(0, (int) _controller.TimeLeft.TotalSeconds).ToString();
        _timeText.color = _controller.TimeLeft.TotalSeconds > 10f ? Color.black : Color.red;
    }

    public void OnStartGame(GamePlayer[] players, int myPlayerIndex)
    {
        ClearPlayerItems();

        _playerComponents = new PlayerComponent[players.Length];
        _portraitComponents = new PortraitComponent[players.Length];

        for (var i = 0; i < players.Length; i++)
        {
            _playerComponents[i] = Instantiate(_playerItemPrefab, _playerItemParent).GetComponent<PlayerComponent>();
            _playerComponents[i].ManualStart(players[i]);

            _portraitComponents[i] = Instantiate(_portraitPrefab, _portraitParent).GetComponent<PortraitComponent>();
            _portraitComponents[i].ManualStart(players[i], myPlayerIndex == i);
        }

        

        _myPlayerIndex = myPlayerIndex;
        _playerCount = players.Length;

        AdjustPlayerAngle();
    }

    public void OnHowSlow(int delayMs)
    {
        _slowText.text = $"{(delayMs * 0.001f):F3}초 느렸습니다!";
        _slowAnimator.SetTrigger("Open");
    }

    public void OnPlayerUpdated(int playerIndex)
    {
        _playerComponents[playerIndex].UpdatePlayer();
        _portraitComponents[playerIndex].UpdateCardLeft();

        int openCardCount = _controller.GetOpenCardCount();

        _openCardText.text = openCardCount.ToString();
    }

    public void OnPlayersUpdated()
    {
        for (var i = 0; i < _playerComponents.Length; i++)
        {
            _playerComponents[i].UpdatePlayer();
            _portraitComponents[i].UpdateCardLeft();
        }
        int openCardCount = _controller.GetOpenCardCount();
        _openCardText.text = openCardCount.ToString();
    }

    public void UpdateGameRuleData(ResponsePacketData.StartGame data)
    {
        _fruitVariationText.text = Utils.GetFruitVariationDescription(data.fruitVariation);
        _fruitCountText.text = Utils.GetFruitCountDescription(data.fruitBellCount);
        _gameTempoText.text = Utils.GetGameTempoDescription(data.gameTempo);
    }

    public void OnBellRing(int playerIndex, int colorCode)
    {
        _handTransform.localRotation = GetPlayerRotation(playerIndex);

        foreach (var handImage in _handImages)
        {
            handImage.color = AssetHolder.Instance.GetCharacterColor(colorCode);
        }

        _handAnimator.SetTrigger("HandUp");
        _effectHandler.AddBellHitEffect(Vector3.zero, Quaternion.identity);
    }

    public void OnPlayerUpdatedWithFlipCard(int playerIndex)
    {
        _playerComponents[playerIndex].UpdateWithFlipCard();
        _portraitComponents[playerIndex].UpdateCardLeft();

        int openCardCount = _controller.GetOpenCardCount();
        _openCardText.text = openCardCount.ToString();
    }

    private void AdjustPlayerAngle()
    {
        for (var i = 0; i < _playerComponents.Length; i++)
        {
            _playerComponents[i].transform.localRotation = Quaternion.Euler(0, 0, GetPlayerAngle(i));
        }

        for (var i = 0; i < _portraitComponents.Length; i++)
        {
            float radius = 570f;

            Vector2 playerVector = GetPlayerVector(i);
            float playerAngle = GetPlayerAngle(i);
            
            _portraitComponents[i].transform.localPosition = playerVector * radius;
            _portraitComponents[i].AdjustAngle(playerAngle);
        }
    }


    private void ClearPlayerItems()
    {
        foreach (Transform child in _playerItemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _portraitParent)
        {
            Destroy(child.gameObject);
        }

        _playerComponents = new PlayerComponent[0];
    }

    public float GetPlayerAngle(int playerIndex)
    {
        // 내 플레이어를 기준으로 상대적 위치 계산
        int relativeIndex = Utils.RealModulo((playerIndex - _myPlayerIndex), _playerCount);
        
        // 전체 원(360도)을 플레이어 수로 나누어 균등 배치
        float angleStep = 360f / _playerCount;
        return angleStep * relativeIndex;
    }

    public Vector2 GetPlayerVector(int playerIndex)
    {
        float angle = GetPlayerAngle(playerIndex);
        
        float angleInRadians = angle * Mathf.Deg2Rad;
        float x = Mathf.Sin(angleInRadians);
        float y = -Mathf.Cos(angleInRadians);
        
        return new Vector2(x, y);
    }

    public Quaternion GetPlayerRotation(int playerIndex)
    {
        return Quaternion.Euler(0, 0, GetPlayerAngle(playerIndex));
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }

    public void OnStartNewAnimation(AnimationItem newAnimationItem)
    {
        _animationHandler.OnStartNewAnimation(newAnimationItem);
    }

    public void OnEmotion(int playerIndex, int emotionID)
    {
        _portraitComponents[playerIndex].UpdateEmotion(emotionID);
    }

    
}
