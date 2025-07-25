using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameDrawer _gameDrawer;

    [SerializeField] private RankHandler _rankHandler;

    [SerializeField] private GameObject _ruleOn;
    [SerializeField] private GameObject _ruleOff;
    

    private InGameState _gameState;

    private TimeSpan _currentTime;
    public TimeSpan CurrentTime => _currentTime;
    public TimeSpan TimeLeft => _updatedTimeLeft - (_currentTime - _updateTime);

    private TimeSpan _updatedTimeLeft;
    private TimeSpan _updateTime;

    private GamePlayer[] _players;
    public GamePlayer[] Players => _players;

    


    
    private int _myPlayerIndex;
    private int _totalPlayerCount;
    private bool _initialized = false;
    private ResponsePacketData.StartGame _startGameData;

    public void ManualStart(ResponsePacketData.StartGame data)
    {
        _currentTime = TimeSpan.Zero;
        _gameState = InGameState.BeforeStart;
        _gameDrawer.ManualStart(this);
        
        _startGameData = data;
        _initialized = true;

        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.ReadyGame());
    }

    private void OnStartGame(ResponsePacketData.StartGame data)
    {
        _myPlayerIndex = data.myIndex;
        _totalPlayerCount = data.playerCount;

        _updatedTimeLeft = TimeSpan.FromSeconds(data.gameTimeLimit);
        _updateTime = _currentTime;

        _gameDrawer.UpdateGameRuleData(data);

        _gameState = InGameState.Playing;

        _players = new GamePlayer[_totalPlayerCount];

        for (var i = 0; i < _totalPlayerCount; i++)
        {
            _players[i] = new GamePlayer(data.playerNames[i], data.startingCards, i);
        }

        _gameDrawer.OnStartGame(_players, _myPlayerIndex);

        SoundManager.Instance.PlaySfxStartGame(0f);

        //StartCoroutine(CO_ShowRandomCard());
    }

    public int GetOpenCardCount()
    {
        int count = 0;
        foreach (var player in _players)
        {
            count += player.ShowCardCount;
        }
        return count;
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
        SoundManager.Instance.PlaySfxCardOpen(0f);
    }

    public void OnResponseRingBellCorrect(bool isSuccess, ResponsePacketData.RingBellCorrect data)
    {
        int[] openCount = new int[_players.Length];
        for (var i = 0; i < _players.Length; i++)
        {
            openCount[i] = _players[i].ShowCardCount;
        }

        _gameDrawer.OnStartNewAnimation(new CorrectAnimationItem(data.playerIndex, openCount));

        OnBellRing(data.playerIndex);
        SoundManager.Instance.PlaySfxCorrect(0.5f);

        ResetAllShowCards();
        SetAllDeckCards(data.playerCards);
        _gameDrawer.OnPlayersUpdated();
    }

    public void OnResponseRingBellWrong(bool isSuccess, ResponsePacketData.RingBellWrong data)
    {
        _gameDrawer.OnStartNewAnimation(new WrongAnimationItem(data.playerIndex, data.cardGivenTo));

        OnBellRing(data.playerIndex);
        SoundManager.Instance.PlaySfxWrong(0.5f);

        SetAllDeckCards(data.playerCards);
        _gameDrawer.OnPlayersUpdated();

        
    }

    public void OnResponseEndGame(bool isSuccess, ResponsePacketData.EndGame data)
    {
        _gameState = InGameState.Ended;
        _rankHandler.UpdateRank(_players, data, _myPlayerIndex);
        SoundManager.Instance.PlaySfxEndGame(0f);

        if (data.playerRanks[_myPlayerIndex] == 1)
        {
            SoundManager.Instance.PlaySfxWinning(0f);
        }

        Utils.Log("OnResponseEndGame");
    }

    public void OnClickExit()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        SceneManager.LoadScene("OutGameScene");
    }

    public void OnClickRuleOn()
    {
        _ruleOn.SetActive(true);
        _ruleOff.SetActive(false);
    }

    public void OnClickRuleOff()
    {
        _ruleOn.SetActive(false);
        _ruleOff.SetActive(true);
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
        _currentTime = _currentTime + TimeSpan.FromSeconds(Time.deltaTime);

    
        HandleInput();

        _gameDrawer.ManualUpdate();
    }

    private void HandleInput()
    {
        if (_gameState != InGameState.Playing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickBell();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnClickEmotion(0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            OnClickEmotion(1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnClickEmotion(2);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnClickEmotion(3);
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

    public void OnClickEmotion(int emotionID)
    {
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.Emotion(emotionID));
    }

    public void OnResponseEmotion(bool isSuccess, ResponsePacketData.Emotion data)
    {
        _gameDrawer.OnEmotion(data.playerIndex, data.emotionType);
    }

    public void OnResponseHowSlow(bool isSuccess, ResponsePacketData.HowSlow data)
    {
        _gameDrawer.OnHowSlow(data.delayMs);
    }


}

public enum InGameState
{
    BeforeStart,
    Playing,
    Ended,
}
