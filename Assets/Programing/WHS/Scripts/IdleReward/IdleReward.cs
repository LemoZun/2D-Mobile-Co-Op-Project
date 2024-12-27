using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleReward : MonoBehaviour
{
    // TimeSpan ��ġ�� �ð� = ���� �ð� - ���� �ð�
    // TotalSeconds�� �ʴ����� ��ȯ
    // CSV Housing ��Ʈ���� �ð��� HousingID 1��� 2���̳���� 3��ũ����Ż�� ���� ����

    private Dictionary<int, Dictionary<string, string>> housingData;

    private void Awake()
    {
        housingData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Housing];
        if (housingData != null)
        {
            Debug.Log("housingData LOADED");
        }
    }

    // �ð� ����ϱ�
    public void CalculateIdleReward()
    {
        string exitTimeStr = PlayerDataManager.Instance.PlayerData.ExitTime;
        DateTime exitTime = DateTime.Parse(exitTimeStr);
        TimeSpan idleTime = DateTime.Now - exitTime;

        int idleSeconds = (int)idleTime.TotalSeconds;

        int goldReward = CalculateReward(1, idleSeconds);
        int dinoBloodReward = CalculateReward(2, idleSeconds);
        int boneCrystalReward = CalculateReward(3, idleSeconds);

        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.Coin, goldReward);
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.DinoBlood, dinoBloodReward);
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.BoneCrystal, boneCrystalReward);

        Debug.Log($"Gold: {goldReward} DinoBlood: {dinoBloodReward} BoneCrystal: {boneCrystalReward}");
    }

    // ������ ���
    private int CalculateReward(int housingId, int seconds)
    {
        if (housingData.TryGetValue(housingId, out Dictionary<string, string> data))
        {
            float rewardPerHour = float.Parse(data["PerHour"]);
            return Mathf.FloorToInt(rewardPerHour * seconds / 3600f);
        }

        return 0;
    }

    // ���� �ð� ����
    public void SaveExitTime()
    {
        string curTime = DateTime.Now.ToString();

        PlayerDataManager.Instance.PlayerData.ExitTime = curTime;

        // string userId = BackendManager.Auth.CurrentUser.UserId;
        string userId = "poZb90DRTiczkoC5TpHOpaJ5AXR2";
        DatabaseReference userRef = BackendManager.Database.RootReference.Child("UserData").Child(userId);

        userRef.Child("_exitTime").SetValueAsync(curTime).ContinueWithOnMainThread(task =>
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
}
