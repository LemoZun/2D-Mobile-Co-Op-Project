using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        GetUI<Button>("SignUpButton").onClick.AddListener(ResetInputField);
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
                ResetInputField();
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

                ResetInputField();
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
            CheckUserInfo();
            ResetInputField();
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

            //ToDo: DB���� PlayerData �ҷ����� 
            GetPlayerData();

        }
    }

    private void GetPlayerData()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;

        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(user.UserId);


        root.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            DataSnapshot snapShot = task.Result;

            while (snapShot == null)
            {
                Debug.Log("snapshot null����!");
            }

            PlayerDataManager.Instance.PlayerData.PlayerName = snapShot.Child("_playerName").Value.ToString();

            PlayerDataManager.Instance.PlayerData.ExitTime = snapShot.Child("_exitTime").Value.ToString();

            // int�� �迭 items ��������
            var itemChildren = snapShot.Child("_items").Children.ToList();
            CheckSnapSHot(itemChildren);

            itemChildren = itemChildren.OrderBy(item => TypeCastManager.Instance.TryParseInt(item.Key)).ToList();
            for (int i = 0; i < itemChildren.Count; i++)
            {
                PlayerDataManager.Instance.PlayerData.Items[i] = TypeCastManager.Instance.TryParseInt(itemChildren[i].Value.ToString());
            }


            // int�� �迭 storedItem��������
            var storedItemChildren = snapShot.Child("_storedItems").Children.ToList();
            CheckSnapSHot(storedItemChildren);

            storedItemChildren = storedItemChildren.OrderBy(storedItem => TypeCastManager.Instance.TryParseInt(storedItem.Key)).ToList();
            for (int i = 0; i < storedItemChildren.Count; i++)
            {
                PlayerDataManager.Instance.PlayerData.StoredItems[i] = TypeCastManager.Instance.TryParseInt(storedItemChildren[i].Value.ToString());
            }


            // int�� �迭 unitPos ��������
            var unitPosChildren = snapShot.Child("_unitPos").Children.ToList();
            CheckSnapSHot(unitPosChildren);

            unitPosChildren = unitPosChildren.OrderBy(unitPos => TypeCastManager.Instance.TryParseInt(unitPos.Key)).ToList();
            for (int i = 0; i < unitPosChildren.Count; i++)
            {
                PlayerDataManager.Instance.PlayerData.UnitPos[i] = TypeCastManager.Instance.TryParseInt(unitPosChildren[i].Value.ToString());
            }


            //bool�� �迭 isStageClear��������
            var isStageClearChildren = snapShot.Child("_isStageClear").Children.ToList();
            CheckSnapSHot(isStageClearChildren);

            isStageClearChildren = isStageClearChildren.OrderBy(isStageClear => TypeCastManager.Instance.TryParseInt(isStageClear.Key)).ToList();
            for (int i = 0; i < isStageClearChildren.Count; i++)
            {
                PlayerDataManager.Instance.PlayerData.IsStageClear[i] = TypeCastManager.Instance.TryParseBool(isStageClearChildren[i].Value.ToString());
            }

            var unitDataChildren = snapShot.Child("_unitDatas").Children.ToList();
            CheckSnapSHot(unitDataChildren);

            unitDataChildren = unitDataChildren.OrderBy(unitData => TypeCastManager.Instance.TryParseInt(unitData.Key)).ToList();

            foreach (var unitChild in unitDataChildren)
            {
                PlayerUnitData unitData = new PlayerUnitData
                {
                    UnitId = TypeCastManager.Instance.TryParseInt(unitChild.Child("_unitId").Value.ToString()),
                    UnitLevel = int.Parse(unitChild.Child("_unitLevel").Value.ToString()),
                };
                PlayerDataManager.Instance.PlayerData.UnitDatas.Add(unitData);
            }

            // �ʱ�ȭ ������ ���� ��change����.
            _sceneChanger.CanChangeSceen = true;
        });
    }

    //Snapshot�� ����� �ҷ��������� üũ�ϴ� �Լ� -> snapshot�� �ҷ������µ� �����ð��� �ణ �ִ°����� ������ ��.
    private void CheckSnapSHot(List<DataSnapshot> snapshotChildren)
    {
        while (snapshotChildren == null || snapshotChildren.Count == 0)
        {
            Debug.Log("snapshot null����!");
        }
    }
    private void SetTrueWarningPanel(string textName)
    {
        GetUI<Image>("LoginWarningPanel").gameObject.SetActive(true);
        _sb.Clear();
        _sb.Append(textName);
        GetUI<TextMeshProUGUI>("WarningText").SetText(_sb);
    }

    private void ResetInputField()
    {
        GetUI<TMP_InputField>("LoginEmailInputField").text = "";
        GetUI<TMP_InputField>("LoginPwInputField").text = "";
    }
}