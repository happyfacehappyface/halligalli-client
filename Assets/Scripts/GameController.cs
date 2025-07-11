using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private AssetHolder _assetHolder;
    [SerializeField] private GameDrawer _gameDrawer;

    public AssetHolder AssetHolder => _assetHolder;

    private TimeSpan _currentTime;
    public TimeSpan CurrentTime => _currentTime;

    private GamePlayer _myPlayer;
    public GamePlayer MyPlayer => _myPlayer;
    private GamePlayer[] _otherPlayers;
    public GamePlayer[] OtherPlayers => _otherPlayers;


    


    private void ManualStart()
    {
        _currentTime = TimeSpan.Zero;
        _gameDrawer.ManualStart(this);
        OnStartGame();
    }

    private void OnStartGame()
    {
        _myPlayer = new GamePlayer("Player", 10);
        _otherPlayers = new GamePlayer[]
        {
            new GamePlayer("Player2", 10),
            new GamePlayer("Player3", 10),
            new GamePlayer("Player4", 10),
        };

        _gameDrawer.OnStartGame(3);
    }

    private void OnResponseBellRing()
    {
        SoundManager.Instance.PlaySfxBell();
    }

    private void ManualUpdate()
    {
        _currentTime = _currentTime + TimeSpan.FromSeconds(Time.deltaTime);

    
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
