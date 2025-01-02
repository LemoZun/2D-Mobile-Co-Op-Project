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

public class NamePanel : UIBInder
{
    [SerializeField] private int _baseUnitNum; //���� ������ �����ִ� ĳ���� ����.

    [SerializeField] private int[] _baseUnitIds;// ĳ���� ID

    private StringBuilder _sb = new StringBuilder();

    [SerializeField] private SceneChanger _sceneChanger;

    [SerializeField] private int _nameLen; // �̸� ���� ����

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        GetUI<Button>("SetNameButton").onClick.AddListener(SetName);
        GetUI<Button>("NameExitButton").onClick.AddListener(ResetInputField);
    }

    private void SetName()
    {
        string nickName = GetUI<TMP_InputField>("NameInputField").text;
        
        // �̸��� ���� ��� �˾�â ����
        if(nickName == "" || nickName.Length > _nameLen)
        {
            SetTrueWarningPanel("No Name or Too Long Name");
            ResetInputField();
            return;
        }

        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
        {
            ResetInputField();
            return;
        }

        UserProfile profile = new UserProfile();
        profile.DisplayName = nickName;


        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                ResetInputField();
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                ResetInputField();
                return;
            }


            ResetInputField();
            CreateDataBase();
        });
    }

    private void CreateDataBase()
    {
        // ������ �񵿱�� ����.
        _sceneChanger.ChangeScene("LobbyOJH");

        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId);
        PlayerDataManager.Instance.PlayerData.PlayerName = BackendManager.Auth.CurrentUser.DisplayName;
        PlayerDataManager.Instance.PlayerData.RoomExitTime = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        PlayerDataManager.Instance.PlayerData.LastResetFollowTime = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
        

        for (int i = 0; i < _baseUnitNum; i++)
        {
            PlayerUnitData unitData = new PlayerUnitData();

            unitData.UnitId = _baseUnitIds[i];
            unitData.UnitLevel = 1;

            PlayerDataManager.Instance.PlayerData.UnitDatas.Add(unitData);
        }

        //Json���� ��ȯ�� ����.
        string json = JsonUtility.ToJson(PlayerDataManager.Instance.PlayerData);
        root.SetRawJsonValueAsync(json);


        // ���������� �Լ� ���۽� �� ��ȯ.
        _sceneChanger.CanChangeSceen = true;
    }


    private void SetTrueWarningPanel(string textName)
    {
        GetUI("NameWarningPanel").SetActive(true);
        _sb.Clear();
        _sb.Append(textName);
        GetUI<TextMeshProUGUI>("NameWarningText").SetText(_sb);
    }

    private void ResetInputField()
    {
        GetUI<TMP_InputField>("NameInputField").text = "";
    }


}
