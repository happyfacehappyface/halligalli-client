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

    public void ManualStart(ResponsePacketData.StartGame data)
    {
        _currentTime = TimeSpan.Zero;
        _gameDrawer.ManualStart(this);
        OnStartGame(data);
        _initialized = true;
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

    private IEnumerator CO_ShowRandomCard()
    {
        int playerIndex = 0;

        while (true)
        {
            yield return new WaitForSeconds(1f);
            OnResponseShowCard(playerIndex, Utils.GetRandomFruitCard());
            playerIndex = Utils.RealModulo((playerIndex + 1), _totalPlayerCount);
        }
    }

    private GamePlayer GetPlayer(int index)
    {
        return _players[index];
    }

    private void OnResponseCorrectBellRing(int playerIndex)
    {
        OnBellRing(playerIndex);

        int totalOpenedCardCount = 0;

        for (var i = 0; i < _players.Length; i++)
        {
            totalOpenedCardCount += _players[i].ShowCardCount;
            _players[i].ResetShowCard();
        }

        _players[playerIndex].DeckCardCount += totalOpenedCardCount;

        _gameDrawer.OnPlayersUpdated();

    }

    private void OnResponseInCorrectBellRing(int playerIndex)
    {
        OnBellRing(playerIndex);
    }

    private void OnBellRing(int playerIndex)
    {
        SoundManager.Instance.PlaySfxBell(0f);
        _gameDrawer.OnBellRing(playerIndex, _players[playerIndex].ColorCode);
    }

    private void OnResponseShowCard(int playerIndex, FruitCard showedCard)
    {
        _players[playerIndex].ChangeTopCard(showedCard);
        _gameDrawer.OnPlayerUpdatedWithFlipCard(playerIndex);
    }

    

    private void ManualUpdate()
    {
        _currentTime += TimeSpan.FromSeconds(Time.deltaTime);

    
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickBell();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            OnResponseInCorrectBellRing(_myPlayerIndex);
        }
    }

    public void OnClickBell()
    {
        OnResponseCorrectBellRing(_myPlayerIndex);
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
