using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class OutGameController : MonoBehaviour
{

    [SerializeField] private GameObject _waitGamePopup;
    [SerializeField] private GameObject _waitForServer;


    [SerializeField] private GameObject _touchBlocker;
    [SerializeField] private GameObject _popupError;
    [SerializeField] private GameObject _popUpLogin;

    [SerializeField] private TextMeshProUGUI _popUpErrorTitle;
    [SerializeField] private TextMeshProUGUI _popUpErrorDescription;

    private UnityAction<Scene, LoadSceneMode> _cachedStartGameHandler;

    protected void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //OnResponseStartGame();
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.Instance.SendMessageToServer(new RequestPacketData.Ping());
        }
        #endif
    }

    public void OnClickEnterRoom()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.EnterRoom());
        _waitForServer.SetActive(true);
    }

    public void OnResponseEnterRoom(bool isSuccess, ResponsePacketData.EnterRoom data)
    {
        if (isSuccess)
        {
            _waitGamePopup.SetActive(true);
        }
        else
        {
            OpenPopupError("문제 발생!", "게임이 이미 시작되었거나 방이 가득 찼습니다.");
        }

        _waitForServer.SetActive(false);
        
    }

    public void OnClickLeaveRoom()
    {
        // TODO: Send Packet to Server
        SoundManager.Instance.PlaySfxButtonClick(0f);
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.LeaveRoom());
        _waitForServer.SetActive(true);
    }

    public void OnResponseLeaveRoom(bool isSuccess, ResponsePacketData.LeaveRoom data)
    {
        if (isSuccess)
        {
            _waitGamePopup.SetActive(false);
            _waitForServer.SetActive(false);
        }
        else
        {
            OpenPopupError("문제 발생!", "방을 나갈 수 없습니다. 다시 시도해주세요");
        }
    }

    public void OnResponseStartGame(bool isSuccess, ResponsePacketData.StartGame data)
    {
        _cachedStartGameHandler = (scene, mode) =>
        {
            SceneManager.sceneLoaded -= _cachedStartGameHandler;
            StartGame(scene, mode, data);
        };

        SceneManager.sceneLoaded += _cachedStartGameHandler;
        SceneManager.LoadScene("InGameScene");
    }

    public void StartGame(Scene scene, LoadSceneMode mode, ResponsePacketData.StartGame data)
    {
        FindObjectOfType<GameController>()?.ManualStart(data);
    }

    public void OnClickClosePopup()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        ClosePopups();
    }

    private void ClosePopups()
    {
        _popupError.SetActive(false);
        _touchBlocker.SetActive(false);
    }

    private void OpenPopupError(string title, string description)
    {
        ClosePopups();
        _popupError.SetActive(true);
        _touchBlocker.SetActive(true);
        _popUpErrorTitle.text = title;
        _popUpErrorDescription.text = description;
    }

    private void OpenPopupLogin()
    {
        ClosePopups();
        _popUpLogin.SetActive(true);
        _touchBlocker.SetActive(true);
    }

    public void OnClickLoginButton()
    {
        GoogleLoginHandler.Instance.TryLogin();
    }





}
