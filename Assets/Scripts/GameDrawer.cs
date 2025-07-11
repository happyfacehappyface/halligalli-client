using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrawer : MonoBehaviour
{
    private GameController _controller;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private Transform _playerItemParent;
    private PlayerComponent _myPlayerComponent;
    private PlayerComponent[] _opponentPlayerComponent;

    public void ManualStart(GameController controller)
    {
        _controller = controller;
    }

    public void OnStartGame(int opponentCount)
    {
        ClearPlayerItems();

        _myPlayerComponent = Instantiate(_playerItemPrefab, _playerItemParent).GetComponent<PlayerComponent>();

        List<PlayerComponent> opponents = new List<PlayerComponent>();
        for (var i = 0; i < opponentCount; i++)
        {
            opponents.Add(Instantiate(_playerItemPrefab, _playerItemParent).GetComponent<PlayerComponent>());
        }

        _opponentPlayerComponent = opponents.ToArray();

        AdjustPlayerAngle();
    }

    private void AdjustPlayerAngle()
    {
        _myPlayerComponent.transform.localRotation = Quaternion.identity;

        int opponentCount = _opponentPlayerComponent.Length;
        float angleStart;
        float angleStep;

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

        for (var i = 0; i < opponentCount; i++)
        { 
            _opponentPlayerComponent[i].transform.localRotation = Quaternion.Euler(0, 0, angleStart + (i * angleStep));

        }
    }


    private void ClearPlayerItems()
    {
        foreach (Transform child in _playerItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    
}
