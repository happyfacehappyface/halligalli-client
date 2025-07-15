using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsideRoomHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _roomPlayerCountText;

    [SerializeField] private TextMeshProUGUI _fruitVariationText;
    [SerializeField] private TextMeshProUGUI _fruitCountText;
    [SerializeField] private TextMeshProUGUI _gameTempoText;


    private OutGameController _controller;
    private int _maxPlayerCount;

    public void ManualStart(OutGameController controller)
    {
        _controller = controller;
    }

    public void OnEnterRoom(ResponsePacketData.EnterRoom data)
    {
        _roomNameText.text = data.roomName;
        _maxPlayerCount = data.maxPlayers;
        _fruitVariationText.text = Utils.GetFruitVariationDescription(data.fruitVariation);
        _fruitCountText.text = Utils.GetFruitCountDescription(data.fruitBellCount);
        _gameTempoText.text = Utils.GetGameTempoDescription(data.gameTempo);
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
