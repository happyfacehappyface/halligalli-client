using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TextMeshProUGUI _roomPlayerCountText;
    [SerializeField] private TextMeshProUGUI _roomSettingFruitVariationText;
    [SerializeField] private TextMeshProUGUI _roomSettingFruitCountText;
    [SerializeField] private TextMeshProUGUI _roomSettingSpeedText;
    [SerializeField] private Button _roomJoinButton;
    [SerializeField] private Image _roomJoinButtonImage;

    private OutGameController _controller;
    private int _roomID;

    public void UpdateRoomItem(OutGameController controller,RoomInfo roomInfo)
    {
        _controller = controller;
        _roomID = roomInfo.roomID;

        _roomNameText.text = roomInfo.roomName;
        _roomPlayerCountText.text = $"{roomInfo.playerCount} / {roomInfo.maxPlayerCount}";
        _roomSettingFruitVariationText.text = $"{roomInfo.fruitVariation}";
        _roomSettingFruitCountText.text = $"{roomInfo.fruitCount}";
        string speedText;

        if (roomInfo.speed == 0)
        {
            speedText = "느림";
        }
        else if (roomInfo.speed == 1)
        {
            speedText = "표준";
        }
        else if (roomInfo.speed == 2)
        {
            speedText = "빠름";
        }
        else
        {
            speedText = "광속";
        }
        _roomSettingSpeedText.text = $"{speedText}";

        _roomJoinButton.onClick.AddListener(() => OnClickJoinRoom());

        if (roomInfo.playerCount >= roomInfo.maxPlayerCount)
        {
            _roomJoinButtonImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else
        {
            _roomJoinButtonImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void OnClickJoinRoom()
    {
        _controller.OnClickJoinRoom(_roomID);
    }
}
