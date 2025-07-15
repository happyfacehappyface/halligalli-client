using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class OutGameController : MonoBehaviour
{

    [SerializeField] private Animator _canvasAnimator;

    [SerializeField] private GameObject _waitForServer;


    [SerializeField] private GameObject _touchBlocker;
    [SerializeField] private GameObject _popupError;
    [SerializeField] private GameObject _popUpAccount;
    [SerializeField] private GameObject _popUpCreateRoom;
    [SerializeField] private AccountPopupHandler _accountPopupHandler;
    [SerializeField] private RoomListHandler _roomListHandler;
    [SerializeField] private CreateRoomPanelHandler _createRoomPanelHandler;
    [SerializeField] private InsideRoomHandler _insideRoomHandler;

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
        OnAccountNameChanged();
        _accountPopupHandler.ManualStart(this);
        _roomListHandler.ManualStart(this);
        _createRoomPanelHandler.ManualStart(this);
        _insideRoomHandler.ManualStart(this);

        _canvasAnimator.SetTrigger("ToTitle");
    }

    public void OnClickStartGame()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        _canvasAnimator.SetTrigger("ToRoomList");
        RequestGetRoomList();
    }

    public void OnClickEnterRoom(int roomID)
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.EnterRoom(roomID));
        _waitForServer.SetActive(true);
    }

    public void OnResponseEnterRoom(bool isSuccess, ResponsePacketData.EnterRoom data)
    {
        if (isSuccess)
        {
            _insideRoomHandler.OnEnterRoom(data.roomName, data.maxPlayers);
            _canvasAnimator.SetTrigger("ToInRoom");
        }
        else
        {
            OpenPopupError("문제 발생!", "게임이 이미 시작되었거나 방이 가득 찼습니다.");
        }

        _waitForServer.SetActive(false);
        
    }

    public void OnClickLeaveRoom()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.LeaveRoom());
        _waitForServer.SetActive(true);
    }

    public void OnResponseLeaveRoom(bool isSuccess, ResponsePacketData.LeaveRoom data)
    {
        if (isSuccess)
        {
            _canvasAnimator.SetTrigger("ToRoomList");
            RequestGetRoomList();
        }
        else
        {
            OpenPopupError("문제 발생!", "방을 나갈 수 없습니다. 다시 시도해주세요");
        }

        _waitForServer.SetActive(false);
    }

    public void RequestGetRoomList()
    {
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.GetRoomList());
        _waitForServer.SetActive(true);
    }

    public void RequestCreateRoom(string roomName, int maxPlayerCount, int fruitVariation, int fruitCount, int speed)
    {
        NetworkManager.Instance.SendMessageToServer(new RequestPacketData.CreateRoom(roomName, maxPlayerCount, fruitVariation, fruitCount, speed));
        ClosePopups();
        _waitForServer.SetActive(true);
    }

    

    public void OnResponseGetRoomList(bool isSuccess, ResponsePacketData.GetRoomList data)
    {
        if (isSuccess)
        {
            UpdateRoomList(data.rooms);
        }
        _waitForServer.SetActive(false);
    }

    public void OnResponseCreateRoom(bool isSuccess, ResponsePacketData.CreateRoom data)
    {
        if (isSuccess)
        {
            return;
        }
        else
        {
            OpenPopupError("문제 발생!", "방을 생성할 수 없습니다. 다시 시도해주세요");
        }
        _waitForServer.SetActive(false);
    }

    public void OnResponsePlayerCountChanged(bool isSuccess, ResponsePacketData.PlayerCountChanged data)
    {
        if (isSuccess)
        {
            _insideRoomHandler.OnRoomPlayerCountChanged(data.playerCount);
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
        _popUpCreateRoom.SetActive(false);
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

    private void OpenPopupCreateRoom()
    {
        ClosePopups();
        _popUpCreateRoom.SetActive(true);
        _touchBlocker.SetActive(true);
        _createRoomPanelHandler.OnOpen();
    }

    public void OnClickOpenAccountPopup()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OpenPopupAccount();
    }

    public void OnClickOpenCreateRoomPopup()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        OpenPopupCreateRoom();
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
            OnAccountNameChanged();
        }
    }

    public void UpdateRoomList(RoomInfo[] roomInfos)
    {
        _roomListHandler.UpdateRoomList(roomInfos);
    }




}
