using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataWriter : MonoBehaviour
{
    [SerializeField] private int _baseUnitNum; //���� ������ �����ִ� ĳ���� ����.

    [SerializeField] private int[] _baseUnitIds;// ĳ���� ID

    [SerializeField] private string _uID;

    [SerializeField] private string _name;

    [ContextMenu("WriteTest")]
    public void CreateDataBase()
    {

        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(_uID);
        PlayerDataManager.Instance.PlayerData.PlayerName = _name;
        PlayerDataManager.Instance.PlayerData.RoomExitTime = DateTime.Now.ToString("o");
        PlayerDataManager.Instance.PlayerData.LastResetFollowTime = DateTime.Now.ToString("o");
        //PlayerDataManager.Instance.PlayerData.Gift["ruru"] = 1000;

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

        //Dictionary�� JsonUtility.TOJson�� �Ұ����ؼ� �ʿ�� ���� �������.
        //root.Child("_gift").SetValueAsync(PlayerDataManager.Instance.PlayerData.Gift);
    }


}
