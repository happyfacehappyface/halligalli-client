using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrawer : MonoBehaviour
{
    private GameController _controller;
    private int _myPlayerIndex;
    private int _playerCount;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private Transform _playerItemParent;
    private PlayerComponent[] _playerComponents;

    public void ManualStart(GameController controller)
    {
        _controller = controller;
    }

    public void OnStartGame(GamePlayer[] players, int myPlayerIndex)
    {
        ClearPlayerItems();

        _playerComponents = new PlayerComponent[players.Length];

        for (var i = 0; i < players.Length; i++)
        {
            _playerComponents[i] = Instantiate(_playerItemPrefab, _playerItemParent).GetComponent<PlayerComponent>();
            _playerComponents[i].ManualStart(players[i]);
        }

        

        _myPlayerIndex = myPlayerIndex;
        _playerCount = players.Length;

        AdjustPlayerAngle();
    }

    public void OnPlayerUpdated(int playerIndex)
    {
        _playerComponents[playerIndex].UpdatePlayer();
    }

    private void AdjustPlayerAngle()
    {
        for (var i = 0; i < _playerComponents.Length; i++)
        {
            _playerComponents[i].transform.localRotation = Quaternion.Euler(0, 0, GetPlayerAngle(i));
        }
    }


    private void ClearPlayerItems()
    {
        foreach (Transform child in _playerItemParent)
        {
            Destroy(child.gameObject);
        }

        _playerComponents = new PlayerComponent[0];
    }

    public float GetPlayerAngle(int playerIndex)
    {
        int opponentCount = _playerCount - 1;
        float angleStart, angleStep;

        if (opponentCount == 1)
        {
            angleStart = 180;
            angleStep = 0;
        }
        else if (opponentCount == 2)
        {
            angleStart = 120;
            angleStep = 120;
        }
        else
        {
            angleStart = 90;
            angleStep = 180 * (1f / (float)(opponentCount - 1));
        }

        if (playerIndex == _myPlayerIndex)
        {
            return 0f;
        }
        else
        {
            Utils.Log($"GetPlayerAngle: {playerIndex}, {_myPlayerIndex}, {opponentCount}");
            int indexDelta = Utils.RealModulo((playerIndex - _myPlayerIndex - 1), opponentCount);
            return angleStart + (indexDelta * angleStep);
        }
    }

    
}
