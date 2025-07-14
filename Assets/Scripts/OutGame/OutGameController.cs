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
    [SerializeField] private GameObject _popUpAccount;
    [SerializeField] private AccountPopupHandler _accountPopupHandler;

    [SerializeField] private TextMeshProUGUI _popUpErrorTitle;
    [SerializeField] private TextMeshProUGUI _popUpErrorDescription;

    [SerializeField] private TextMeshProUGUI _accountNameText;

    private UnityAction<Scene, LoadSceneMode> _cachedStartGameHandler;

    protected void Start()
    {
        ManualStart();
    }

    private void ManualStart()
    {
        _accountNameText.text = AccountManager.Instance.AccountName();
        _accountPopupHandler.ManualStart(this);
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

    public void ClosePopups()
    {
        _popupError.SetActive(false);
        _touchBlocker.SetActive(false);
        _popUpAccount.SetActive(false);
    }

    public void OpenPopupError(string title, string description)
    {
        ClosePopups();
        _popupError.SetActive(true);
        _touchBlocker.SetActive(true);
        _popUpErrorTitle.text = title;
        _popUpErrorDescription.text = description;
    }

    private void OpenPopupAccount()
    {
        ClosePopups();
        _popUpAccount.SetActive(true);
        _touchBlocker.SetActive(true);
        _accountPopupHandler.OnOpenAccountPopup();
    }

    public void OnClickOpenAccountPopup()
    {
        OpenPopupAccount();
        SoundManager.Instance.PlaySfxButtonClick(0f);
    }

    public void OnAccountNameChanged()
    {
        _accountNameText.text = AccountManager.Instance.AccountName();
    }

    public void RequestCreateAccount(string id, string password, string nickname)
    {
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.CreateAccount(id, password, nickname));
        _waitForServer.SetActive(true);
    }

    public void RequestLogin(string id, string password)
    {
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.Login(id, password));
        _waitForServer.SetActive(true);
    }

    public void OnResponseCreateAccount(bool isSuccess, ResponsePacketData.CreateAccount data)
    {
        _waitForServer.SetActive(false);
        ClosePopups();
        if (!isSuccess)
        {
            OpenPopupError("계정 생성 실패", "다시 시도해주세요");
        }
    }

    public void OnResponseLogin(bool isSuccess, ResponsePacketData.Login data)
    {
        _waitForServer.SetActive(false);
        ClosePopups();
        if (!isSuccess)
        {
            OpenPopupError("로그인 실패", "아이디와 비밀번호를 확인해주세요");
        }
        else
        {
            AccountManager.Instance.OnLogIn(data.id, data.nickname);
        }
    }




}
