using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private AssetHolder _assetHolder;
    public AssetHolder AssetHolder => _assetHolder;

    private TimeSpan _currentTime;
    public TimeSpan CurrentTime => _currentTime;

    private GamePlayer _myPlayer;
    public GamePlayer MyPlayer => _myPlayer;
    private GamePlayer[] _otherPlayers;
    public GamePlayer[] OtherPlayers => _otherPlayers;


    protected void Start()
    {
        ManualStart();
    }

    protected void Update()
    {
        ManualUpdate();
    }


    private void ManualStart()
    {
        _currentTime = TimeSpan.Zero;
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
    }

    private void ManualUpdate()
    {
        _currentTime = _currentTime + TimeSpan.FromSeconds(Time.deltaTime);
    }


}
