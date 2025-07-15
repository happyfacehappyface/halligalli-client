using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsideRoomHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _roomPlayerCountText;

    [SerializeField] private Button _startGameButton;

    private OutGameController _controller;
    private int _maxPlayerCount;

    public void ManualStart(OutGameController controller)
    {
        _controller = controller;
    }

    public void OnEnterRoom(string roomName, int maxPlayerCount)
    {
        _roomNameText.text = roomName;
        _maxPlayerCount = maxPlayerCount;
    }


    public void OnRoomPlayerCountChanged(int playerCount)
    {
        _roomPlayerCountText.text = $"{playerCount} / {_maxPlayerCount}";
    }

    public void OnClickExitRoom()
    {
        _controller.OnClickLeaveRoom();
    }




}
