using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        yield return new WaitUntil(() => PlayerDataManager.Instance.PlayerData.UnitDatas.Count > 0);

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

    // ��ġ�ð� ����ϱ�
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

        // �����ͺ��̽��� ��ġ�� ������ ����
        UpdateStoredItemsInDatabase();
    }

    // ������ ���
    public int CalculateReward(int housingId, int seconds)
    {
        if (housingData.TryGetValue(housingId, out Dictionary<string, string> data))
        {
            // ���������� ���� �ð��� ����
            int rewardPerHour = GetRewardPerHour(housingId);
                       
            int reward = Mathf.FloorToInt(rewardPerHour * seconds / 1f);
            // int hours = seconds / 3600;
            // int reward = Mathf.FloorToInt(rewardPerHour * hours);
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

    // Ÿ�̸� 1�ð� �̻���� ���ɹ�ư Ȱ��ȭ
    public bool HasIdleReward()
    {
        string exitTimeStr = PlayerDataManager.Instance.PlayerData.RoomExitTime;
        DateTime exitTime = DateTime.Parse(exitTimeStr);
        TimeSpan idleTime = DateTime.Now - exitTime;

        int idleSeconds = (int)idleTime.TotalSeconds;
        return idleSeconds >= 3600;
    }

    // ��ġ�� �ð�
    public TimeSpan GetIdleTime()
    {
        string exitTimeStr = PlayerDataManager.Instance.PlayerData.RoomExitTime;
        DateTime exitTime = DateTime.Parse(exitTimeStr);
        return DateTime.Now - exitTime;
    }

    // �������� ���࿡ ���� ����
    private int GetRewardPerHour(int housingId)
    {
        if(housingData.TryGetValue(housingId, out Dictionary<string, string> data))
        {
            int baseReward = int.Parse(data["PerHour"]);

            // Ŭ������ ���������� ��
            int clearedStages = PlayerDataManager.Instance.PlayerData.IsStageClear.Count(x => x);

            if (clearedStages >= 7 && data.TryGetValue("2MaxStorage", out string storage2))
            {
                Debug.Log("storage2");
                return int.Parse(storage2);
            }
            else if (clearedStages >= 5 && data.TryGetValue("1MaxStorage", out string storage1))
            {
                Debug.Log("storage1");
                return int.Parse(storage1);
            }
            else if(clearedStages >= 2 && data.TryGetValue("0MaxStorage", out string storage0))
            {
                Debug.Log("storage0");
                return int.Parse(storage0);
            }
            else
            {
                Debug.Log("basereward");
                return baseReward;
            }
        }
        return 0;
    }
}
