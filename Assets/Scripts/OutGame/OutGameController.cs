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
            OnResponseStartGame();
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.Instance.SendMessageToServer(new RequestPacketData.Ping());
        }
        #endif
    }

    public void OnClickWaitGame()
    {
        // TODO: Send Packet to Server

        SoundManager.Instance.PlaySfxButtonClick(0f);
        OnResponseWaitGame();
    }

    public void OnResponseWaitGame()
    {
        _waitGamePopup.SetActive(true);
    }

    public void OnClickCancelWaitGame()
    {
        // TODO: Send Packet to Server
        SoundManager.Instance.PlaySfxButtonClick(0f);
        _waitGamePopup.SetActive(false);
    }

    public void OnResponseStartGame()
    {
        _cachedStartGameHandler = (scene, mode) =>
        {
            SceneManager.sceneLoaded -= _cachedStartGameHandler;
            StartGame(scene, mode);
        };

        SceneManager.sceneLoaded += _cachedStartGameHandler;
        SceneManager.LoadScene("InGameScene");
    }

    public void StartGame(Scene scene, LoadSceneMode mode)
    {
        FindObjectOfType<GameController>()?.ManualStart();
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



}
