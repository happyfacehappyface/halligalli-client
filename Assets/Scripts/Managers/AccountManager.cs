using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance;
    private bool _isReady = false;

    public bool IsReady() => _isReady;

    private bool _isLoggedIn = false;
    public bool IsLoggedIn() => _isLoggedIn;

    private string _accountID = "";
    public string AccountID() => _accountID;

    private string _accountName = "Guest";
    public string AccountName() => _accountName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _isReady = true;

    }

    public void OnLogIn(string accountID, string accountName)
    {
        _isLoggedIn = true;
        _accountID = accountID;
        _accountName = accountName;
    }

    public void OnLogOut()
    {
        _isLoggedIn = false;
        _accountID = "";
        _accountName = "Guest";
    }


}
