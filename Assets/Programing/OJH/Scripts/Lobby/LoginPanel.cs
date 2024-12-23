using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBInder
{
    private string _email;

    private string _password;

    private StringBuilder _sb = new StringBuilder();

    [SerializeField] private SceneChanger _sceneChanger;

    private void Awake()
    {
        BindAll();
    }
    void Start()
    {
        _sceneChanger.CanChangeSceen = false;
        GetUI<Button>("LoginButton").onClick.AddListener(Login);
        GetUI<Button>("LoginExitButton").onClick.AddListener(_sceneChanger.QuitGame);
    }


    private void Login()
    {
        Debug.Log("�α��� ����");

        _email = GetUI<TMP_InputField>("LoginEmailInputField").text;

        _password = GetUI<TMP_InputField>("LoginPwInputField").text;

        BackendManager.Auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Exception exception = task.Exception.InnerException;

                FirebaseException firebaseException = exception as FirebaseException;

                if (firebaseException != null)
                {
                    AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                    Debug.Log(errorCode);

                    switch (errorCode)
                    {
                        case AuthError.InvalidEmail:
                            SetTrueWarningPanel("InvalidEmail");
                            break;
                        case AuthError.UserNotFound:
                            SetTrueWarningPanel("UserNotFound");
                            break;
                        case AuthError.MissingEmail:
                            SetTrueWarningPanel("MissingEmail");
                            break;
                        case AuthError.MissingPassword:
                            SetTrueWarningPanel("MissingPassword");
                            break;
                        case AuthError.WeakPassword:
                            SetTrueWarningPanel("WeakPassword");
                            break;
                        case AuthError.WrongPassword:
                            SetTrueWarningPanel("WrongPassword");
                            break;
                        default:
                            SetTrueWarningPanel("Unknown Error. Try Again.");
                            break;
                    }
                }
                else
                {
                    Debug.LogError("FirebaseException�� �ƴ� �ٸ� ���ܰ� �߻��߽��ϴ�: " + exception?.ToString());
                }

                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
            CheckUserInfo();
        });
    }

    public void CheckUserInfo()
    {
        Debug.Log("ù �α���!!!!");
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
        {
            return;
        }

        // ù �α��� �� �̸��� ���� �� �г��� ���� ����
        if (user.IsEmailVerified == false)
        {
            //TODO : �̸��� ���� ����
            GetUI<VerifyPanel>("VerifyPanel").namePanel = GetUI("NamePanel").gameObject;
            GetUI<VerifyPanel>("VerifyPanel").gameObject.SetActive(true);

        }
        else if (user.DisplayName == "")  // �������ϰ� �̸� �������� ��쵵 ���.
        {
            //TODO : �г��� ���� ����
            GetUI("NamePanel").SetActive(true);
        }
        else
        {
            //TODO :  �κ������ �񵿱� �� ��ȯ 
            _sceneChanger.ChangeScene("LobbyOJH");

            Debug.Log("User profile updated successfully.");
            Debug.Log($"Display name: {user.DisplayName}");
            Debug.Log($"Email: {user.Email}");
            Debug.Log($"EmailVerified: {user.IsEmailVerified}");
            Debug.Log($"UserId: {user.UserId}");

            // �ʱ�ȭ ������ ���� ��change����.
            _sceneChanger.CanChangeSceen = true;
        }
    }

    private void SetTrueWarningPanel(string textName)
    {
        GetUI<Image>("LoginWarningPanel").gameObject.SetActive(true);
        _sb.Clear();
        _sb.Append(textName);
        GetUI<TextMeshProUGUI>("WarningText").SetText(_sb);
    }
}
