using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleReward : MonoBehaviour
{
    private Dictionary<int, Dictionary<string, string>> housingData;

    private void Start()
    {
        StartCoroutine(WaitForPlayerData());
    }

    private IEnumerator WaitForPlayerData()
    {
        // PlayerDataManager�� �ʱ�ȭ�ǰ� PlayerData�� �ε�� ������ ���
        yield return new WaitUntil(() =>
            PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.PlayerData != null &&
            PlayerDataManager.Instance.PlayerData.UnitDatas != null &&
            PlayerDataManager.Instance.PlayerData.UnitDatas.Count > 0);

        housingData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Housing];
        Debug.Log($"{housingData.Count}");
        foreach (var key in housingData.Keys)
        {
            Debug.Log($"Key: {key}, Value: {housingData[key]["PerHour"]}, {housingData[key]["0MaxStorage"]}");
        }
    }
    /*
    private void Awake()
    {
        housingData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Housing];

        Debug.Log($"{housingData.Count}");
        foreach (var key in housingData.Keys)
        {
            Debug.Log($"Key: {key}, Value: {housingData[key]["PerHour"]}, {housingData[key]["0MaxStorage"]}");
        }
    }
    */

    // �ð� ����ϱ�
    public void CalculateIdleReward()
    {
        string exitTimeStr = PlayerDataManager.Instance.PlayerData.RoomExitTime;
        DateTime exitTime = DateTime.Parse(exitTimeStr);
        TimeSpan idleTime = DateTime.Now - exitTime;

        int idleSeconds = (int)idleTime.TotalSeconds;
        Debug.Log(idleSeconds);

        int goldReward = CalculateReward(1, idleSeconds);
        int dinoBloodReward = CalculateReward(2, idleSeconds);
        int boneCrystalReward = CalculateReward(3, idleSeconds);

        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.Coin, goldReward);
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.DinoBlood, dinoBloodReward);
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.BoneCrystal, boneCrystalReward);

        Debug.Log($"Gold: {goldReward} DinoBlood: {dinoBloodReward} BoneCrystal: {boneCrystalReward}");

        UpdateStoredItemsInDatabase();
    }

    // ������ ���
    private int CalculateReward(int housingId, int seconds)
    {
        if (housingData.TryGetValue(housingId, out Dictionary<string, string> data))
        {
            float rewardPerHour = float.Parse(data["PerHour"]);
            int reward = Mathf.FloorToInt(rewardPerHour * seconds / 1f);
            return reward;
        }
        return 0;
    }

    // �����ͺ��̽��� ��ġ�� ������ ����
    private void UpdateStoredItemsInDatabase()
    {
        // string userId = BackendManager.Auth.CurrentUser.UserId;
        string userId = "sinEKs9IWRPuWNbboKov1fKgmab2";
        DatabaseReference userRef = BackendManager.Database.RootReference.Child("UserData").Child(userId).Child("_storedItems");

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            ["0"] = PlayerDataManager.Instance.PlayerData.StoredItems[(int)E_Item.Coin],
            ["1"] = PlayerDataManager.Instance.PlayerData.StoredItems[(int)E_Item.DinoBlood],
            ["2"] = PlayerDataManager.Instance.PlayerData.StoredItems[(int)E_Item.BoneCrystal],
        };

        userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log($"��ġ�� ������ ���� ���� {task.Exception}");
            }
            if (task.IsCanceled)
            {
                Debug.LogError($"��ġ�� ������ ���� �ߴܵ� {task.Exception}");
            }
            Debug.Log("��ġ�� ���� �����");
        });
    }

    // ���� �ð� ����
    public void SaveExitTime()
    {
        string curTime = DateTime.Now.ToString();

        PlayerDataManager.Instance.PlayerData.RoomExitTime = curTime;

        // string userId = BackendManager.Auth.CurrentUser.UserId;
        string userId = "sinEKs9IWRPuWNbboKov1fKgmab2";
        DatabaseReference userRef = BackendManager.Database.RootReference.Child("UserData").Child(userId);

        userRef.Child("_roomExitTime").SetValueAsync(curTime).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"exittime ���� ���� {task.Exception}");
            }
            if (task.IsCanceled)
            {
                Debug.LogError($"exittime ���� �ߴܵ� {task.Exception}");
            }

            Debug.Log($"exittime ����� {curTime}");
        });
    }

    public bool HasIdleReward()
    {
        string exitTimeStr = PlayerDataManager.Instance.PlayerData.RoomExitTime;
        DateTime exitTime = DateTime.Parse(exitTimeStr);
        TimeSpan idleTime = DateTime.Now - exitTime;

        int idleSeconds = (int)idleTime.TotalSeconds;
        return idleSeconds >= 3600;
    }
}
