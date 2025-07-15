using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CreateRoomPanelHandler : MonoBehaviour
{
    private OutGameController _controller;

    private int _roomMaxPlayerCount;
    private int _roomFruitVariation;
    private int _roomFruitCount;
    private int _roomSpeed;

    public void ManualStart(OutGameController controller)
    {
        _controller = controller;
        _roomMaxPlayerCount = 4;
        _roomFruitVariation = 4;
        _roomFruitCount = 5;
        _roomSpeed = 1;

        UpdateRoomSettingText();
    }

    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private TextMeshProUGUI _roomMaxPlayerCountText;
    [SerializeField] private TextMeshProUGUI _roomFruitVariationText;
    [SerializeField] private TextMeshProUGUI _roomFruitCountText;
    [SerializeField] private TextMeshProUGUI _roomSpeedText;

    public void UpdateRoomSettingText()
    {
        _roomMaxPlayerCountText.text = $"{_roomMaxPlayerCount}";
        _roomFruitVariationText.text = $"{_roomFruitVariation}";
        _roomFruitCountText.text = $"{_roomFruitCount}";

        string speedText;
        if (_roomSpeed == 0)
        {
            speedText = "느림";
        }
        else if (_roomSpeed == 1)
        {
            speedText = "표준";
        }
        else if (_roomSpeed == 2)
        {
            speedText = "빠름";
        }
        else
        {
            speedText = "광속";
        }
        _roomSpeedText.text = $"{speedText}";
    }

    public void OnOpen()
    {
        UpdateRoomSettingText();
        _roomNameInputField.text = String.Empty;
    }

    public void OnClickCreateRoom()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);

        if ((_roomNameInputField.text.Length <= 0) || (_roomNameInputField.text.Length > 10))
        {
            return;
        }

        // TODO
        //_controller.OnClickCreateRoom(_roomNameInputField.text, _roomMaxPlayerCount, _roomFruitVariation, _roomFruitCount, _roomSpeed);
    }

    public void OnClickClose()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        _controller.ClosePopups();
    }

    private void OnChangeRoomMaxPlayerCount(int delta)
    {
        _roomMaxPlayerCount = Math.Clamp(_roomMaxPlayerCount + delta, 3, 8);
        UpdateRoomSettingText();
    }

    private void OnChangeRoomFruitVariation(int delta)
    {
        _roomFruitVariation = Math.Clamp(_roomFruitVariation + delta, 2, 6);
        UpdateRoomSettingText();
    }

    private void OnChangeRoomFruitCount(int delta)
    {
        _roomFruitCount = Math.Clamp(_roomFruitCount + delta, 3, 8);
        UpdateRoomSettingText();
    }

    private void OnChangeRoomSpeed(int delta)
    {
        _roomSpeed = Math.Clamp(_roomSpeed + delta, 0, 3);
        UpdateRoomSettingText();
    }

    public void OnClickPlayerCountUp()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomMaxPlayerCount(1);
    }
    public void OnClickPlayerCountDown()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomMaxPlayerCount(-1);
    }

    public void OnClickFruitVariationUp()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomFruitVariation(1);
    }
    public void OnClickFruitVariationDown()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomFruitVariation(-1);
    }

    public void OnClickFruitCountUp()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomFruitCount(1);
    }
    public void OnClickFruitCountDown()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomFruitCount(-1);
    }

    public void OnClickSpeedUp()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomSpeed(1);
    }
    public void OnClickSpeedDown()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnChangeRoomSpeed(-1);
    }
}
