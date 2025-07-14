using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountPopupHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentAccountText;
    [SerializeField] private TMP_InputField _logInIDInputField;
    [SerializeField] private TMP_InputField _logInPasswordInputField;
    [SerializeField] private TMP_InputField _createAccountInputField;
    [SerializeField] private TMP_InputField _createPasswordInputField;
    [SerializeField] private TMP_InputField _createPasswordConfirmInputField;
    [SerializeField] private TMP_InputField _createNickNameInputField;

    [SerializeField] private Animator _popupAnimator;

    private OutGameController _controller;

    public void ManualStart(OutGameController controller)
    {
        _controller = controller;
    }

    public void OnOpenAccountPopup()
    {
        _currentAccountText.text = $"현재 {AccountManager.Instance.AccountName()} 계정으로 로그인되어 있습니다.";
    }

    public void OnClickCloseAccountPopup()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        _controller.ClosePopups();
    }

    public void OnClickConvertToCreateAccount()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        _popupAnimator.SetTrigger("CreateAccountOpen");
    }

    public void OnClickConvertToLogInAccount()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        _popupAnimator.SetTrigger("LogInOpen");
    }

    public void OnClickLogIn()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        if (!IsValidLoginSituation())
        {
            _controller.OpenPopupError("로그인 실패", "아이디와 비밀번호의 길이는 0자 초과 10자 이하이어야 합니다.");
        }
        else
        {
            // TODO: Send Packet to Server
        }
    }

    public void OnClickCreateAccount()
    {
        SoundManager.Instance.PlaySfxButtonClick(0f);
        if (!IsValidCreateAccountSituation())
        {
            _controller.OpenPopupError("계정 생성 실패", "올바르지 않은 요청입니다.");
        }
        else
        {
            // TODO: Send Packet to Server
        }
    }






    private bool IsValidLoginSituation()
    {
        if (_logInIDInputField.text.Length <= 0) return false;

        if (_logInPasswordInputField.text.Length <= 0) return false;

        if (_logInIDInputField.text.Length > 10) return false;

        if (_logInPasswordInputField.text.Length > 10) return false;

        return true;
    }

    private bool IsValidCreateAccountSituation()
    {
        if (_createAccountInputField.text.Length <= 0) return false;

        if (_createPasswordInputField.text.Length <= 0) return false;

        if (_createPasswordConfirmInputField.text.Length <= 0) return false;

        if (_createAccountInputField.text.Length > 10) return false;

        if (_createPasswordInputField.text.Length > 10) return false;

        if (_createPasswordConfirmInputField.text.Length > 10) return false;

        if (_createPasswordInputField.text != _createPasswordConfirmInputField.text) return false;

        if (_createNickNameInputField.text.Length <= 0) return false;

        if (_createNickNameInputField.text.Length > 10) return false;

        return true;
    }




}
