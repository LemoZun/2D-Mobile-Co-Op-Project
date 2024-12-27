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
    }

    public void CalculateIdleReward()
    {
        string exitTimeStr = PlayerDataManager.Instance.PlayerData.ExitTime;

        DateTime exitTime = DateTime.Parse(exitTimeStr);
        TimeSpan idleTime = DateTime.Now - exitTime;

        int idleSeconds = (int)idleTime.TotalSeconds; // ��ġ�� �ð� ��
    }
}
