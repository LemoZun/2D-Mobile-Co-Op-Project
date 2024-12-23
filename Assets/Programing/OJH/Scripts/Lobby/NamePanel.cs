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

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        GetUI<Button>("SetNameButton").onClick.AddListener(SetName);
    }

    private void SetName()
    {
        string nickName = GetUI<TMP_InputField>("NameInputField").text;
        
        // �̸��� ���� ��� �˾�â ����
        if(nickName == "")
        {
            SetTrueWarningPanel("Please Input Name");
            return;
        }

        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;

        UserProfile profile = new UserProfile();
        profile.DisplayName = nickName;


        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                return;
            }

            CreateDataBase();
        });
    }

    private void CreateDataBase()
    {
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId);
        PlayerDataManager.Instance.PlayerData.PlayerName = BackendManager.Auth.CurrentUser.DisplayName;
        PlayerDataManager.Instance.PlayerData.PlayerId = BackendManager.Auth.CurrentUser.UserId;

        for (int i = 0; i < (int)E_Money.Length; i++)
        {
            PlayerDataManager.Instance.PlayerData.Money[i] = 0;
        }

        List<Dictionary<string, string>> stageDic = DataManager.Instance.DataLists[(int)E_CsvData.Character];

        for (int i = 0; i < _baseUnitNum; i++)
        {
            PlayerUnitData unitData = new PlayerUnitData();

            Debug.Log($"{i}����");

            foreach (Dictionary<string, string> field in stageDic)
            {
                if (int.Parse(field["CharID"]) == _baseUnitIds[i])
                {
                    unitData.Name = field["Name"];
                    unitData.UnitLevel = 1;
                    unitData.Type = field["Class"];
                    unitData.ElementName = field["ElementName"];
                    unitData.Hp = int.Parse(field["BaseHp"]);
                    unitData.Atk = int.Parse(field["BaseATK"]);
                    unitData.Def = int.Parse(field["BaseDef"]);
                    unitData.Grid = field["Grid"];
                    unitData.StatId = int.Parse(field["StatID"]);
                    unitData.PercentIncrease = int.Parse(field["PercentIncrease"]);

                    PlayerDataManager.Instance.PlayerData.UnitDatas.Add(unitData);
                    Debug.Log("hhhii");
                }
            }
        }

        string json = JsonUtility.ToJson(PlayerDataManager.Instance.PlayerData);
        root.SetRawJsonValueAsync(json);


        // Data�����ϰ����� �ʱ�ȭ���ֱ�
        PlayerDataManager.Instance.PlayerData.UnitDatas.Clear();

        // Database�����ϰ� ���� �̸� Panel ���� �α���ȭ������ ���ư���
        gameObject.SetActive(false);
    }


    private void SetTrueWarningPanel(string textName)
    {
        GetUI("NameWarningPanel").SetActive(true);
        _sb.Clear();
        _sb.Append(textName);
        GetUI<TextMeshProUGUI>("WarningText").SetText(_sb);
    }


}
