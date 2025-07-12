using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameDrawer _gameDrawer;


    private TimeSpan _currentTime;
    public TimeSpan CurrentTime => _currentTime;

    private GamePlayer[] _players;
    public GamePlayer[] Players => _players;

    


    
    private int _myPlayerIndex;
    private int _totalPlayerCount;
    private bool _initialized = false;
    private ResponsePacketData.StartGame _startGameData;

    public void ManualStart(ResponsePacketData.StartGame data)
    {
        _currentTime = TimeSpan.Zero;
        _gameDrawer.ManualStart(this);
        //OnStartGame(data);
        _startGameData = data;
        _initialized = true;

        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.ReadyGame());
    }

    private void OnStartGame(ResponsePacketData.StartGame data)
    {
        _myPlayerIndex = data.myIndex;
        _totalPlayerCount = data.playerCount;

        _players = new GamePlayer[_totalPlayerCount];

        for (var i = 0; i < _totalPlayerCount; i++)
        {
            _players[i] = new GamePlayer(data.playerNames[i], data.startingCards, 0);
        }

        _gameDrawer.OnStartGame(_players, _myPlayerIndex);

        

        //StartCoroutine(CO_ShowRandomCard());
    }

    public void OnResponseReadyGame(bool isSuccess, ResponsePacketData.ReadyGame data)
    {
        if (isSuccess)
        {
            OnStartGame(_startGameData);
        }
        else
        {
            // TODO: 오류 처리 (Outgame으로 돌아가기)
        }
    }

    public void OnResponseOpenCard(bool isSuccess, ResponsePacketData.OpenCard data)
    {
        _players[data.playerIndex].ChangeTopCard(new FruitCard(data.fruitIndex, data.fruitCount));
        _gameDrawer.OnPlayerUpdatedWithFlipCard(data.playerIndex);
    }

    public void OnResponseRingBellCorrect(bool isSuccess, ResponsePacketData.RingBellCorrect data)
    {
        int[] deckCount = new int[_players.Length];
        for (var i = 0; i < _players.Length; i++)
        {
            deckCount[i] = _players[i].DeckCardCount;
        }

        _gameDrawer.OnStartNewAnimation(new CorrectAnimationItem(data.playerIndex, deckCount));

        OnBellRing(data.playerIndex);

        ResetAllShowCards();
        SetAllDeckCards(data.playerCards);
        _gameDrawer.OnPlayersUpdated();
    }

    public void OnResponseRingBellWrong(bool isSuccess, ResponsePacketData.RingBellWrong data)
    {
        //_gameDrawer.OnStartNewAnimation();

        OnBellRing(data.playerIndex);

        SetAllDeckCards(data.playerCards);
        _gameDrawer.OnPlayersUpdated();

        
    }

    private GamePlayer GetPlayer(int index)
    {
        return _players[index];
    }

    private void ResetAllShowCards()
    {
        foreach (var player in _players)
        {
            player.ResetShowCard();
        }
    }

    private void SetAllDeckCards(int[] deckCount)
    {
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i].DeckCardCount = deckCount[i];
        }
    }


    private void OnBellRing(int playerIndex)
    {
        SoundManager.Instance.PlaySfxBell(0f);
        _gameDrawer.OnBellRing(playerIndex, _players[playerIndex].ColorCode);
    }
    

    private void ManualUpdate()
    {
        _currentTime += TimeSpan.FromSeconds(Time.deltaTime);

    
        HandleInput();

        _gameDrawer.ManualUpdate();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickBell();
        }
    }

    public void OnClickBell()
    {
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.RingBell());
    }

    protected void Start()
    {
        //ManualStart();
    }

    protected void Update()
    {
        if (_initialized)
        {
            ManualUpdate();
        }

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!_initialized)
            {
                //ManualStart();
            }
        }
        #endif


    }


}
