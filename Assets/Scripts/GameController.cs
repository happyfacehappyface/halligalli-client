using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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

    private void ManualStart()
    {
        _currentTime = TimeSpan.Zero;
        _gameDrawer.ManualStart(this);
        OnStartGame();
    }

    private void OnStartGame()
    {
        
        _players = new GamePlayer[]
        {
            new GamePlayer("Player", 10),
            new GamePlayer("Player2", 10),
            new GamePlayer("Player3", 10),
            new GamePlayer("Player4", 10),
        };

        _gameDrawer.OnStartGame(_players, _myPlayerIndex);

        _myPlayerIndex = 0;
        _totalPlayerCount = 4;

        StartCoroutine(CO_ShowRandomCard());
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

    private void OnResponseBellRing()
    {
        SoundManager.Instance.PlaySfxBell();
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
            OnResponseBellRing();
        }
    }

    protected void Start()
    {
        ManualStart();
    }

    protected void Update()
    {
        ManualUpdate();
    }


}
