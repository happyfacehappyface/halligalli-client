using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDrawer : MonoBehaviour
{
    private GameController _controller;
    private int _myPlayerIndex;
    private int _playerCount;
    [SerializeField] private GameObject _playerItemPrefab;
    [SerializeField] private Transform _playerItemParent;

    [SerializeField] private GameObject _portraitPrefab;
    [SerializeField] private Transform _portraitParent;

    
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private Image[] _handImages;
    private PlayerComponent[] _playerComponents;
    private PortraitComponent[] _portraitComponents;

    public void ManualStart(GameController controller)
    {
        _controller = controller;
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
            _portraitComponents[i].ManualStart(players[i]);
        }

        

        _myPlayerIndex = myPlayerIndex;
        _playerCount = players.Length;

        AdjustPlayerAngle();
    }

    public void OnPlayerUpdated(int playerIndex)
    {
        _playerComponents[playerIndex].UpdatePlayer();
        _portraitComponents[playerIndex].UpdateCardLeft();
    }

    public void OnBellRing(int playerIndex, int colorCode)
    {
        _handTransform.localRotation = Quaternion.Euler(0, 0, GetPlayerAngle(playerIndex));

        foreach (var handImage in _handImages)
        {
            handImage.color = AssetHolder.Instance.GetCharacterColor(colorCode);
        }

        _handAnimator.SetTrigger("HandUp");
    }

    public void OnPlayerUpdatedWithFlipCard(int playerIndex)
    {
        _playerComponents[playerIndex].UpdateWithFlipCard();
    }

    private void AdjustPlayerAngle()
    {
        for (var i = 0; i < _playerComponents.Length; i++)
        {
            _playerComponents[i].transform.localRotation = Quaternion.Euler(0, 0, GetPlayerAngle(i));
        }

        for (var i = 0; i < _portraitComponents.Length; i++)
        {
            float angle = GetPlayerAngle(i);
            float radius = 570f;
            
            float angleInRadians = angle * Mathf.Deg2Rad;
            float x = Mathf.Sin(angleInRadians) * radius;
            float y = -Mathf.Cos(angleInRadians) * radius;
            
            _portraitComponents[i].transform.localPosition = new Vector3(x, y, 1f);
            _portraitComponents[i].AdjustAngle(angle);
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
            int indexDelta = Utils.RealModulo((playerIndex - _myPlayerIndex - 1), opponentCount);
            return angleStart + (indexDelta * angleStep);
        }
    }

    
}
