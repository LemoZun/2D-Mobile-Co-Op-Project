using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;





public class DatabaseTest : MonoBehaviour
{
    [SerializeField] GameObject monster;
    [SerializeField] GameObject player;

    [SerializeField] Transform[] myPoss;
    [SerializeField] Transform[] enemyPoss;

    private void OnEnable()
    {
    }


    private StageData stageData;
    
   
    private void getDbData() 
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Stages").GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("�� �������� ��ҵ�");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogWarning($"�� �������� ������ : {task.Exception.Message}");
                    return;
                }

                DataSnapshot snapshot = task.Result;

                Debug.Log($"Found {snapshot.GetRawJsonValue()}");
                Debug.Log("�������� ����");
            });



    }
    public void TestgetStageData(int stageNum) 
    {   
        string num = (stageNum-1).ToString(); // �Է��Ҷ� 1���� ���� �Է��ϴ°� ���ƺ��̱� �� 

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Stages").Child(num).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("�� �������� ��ҵ�");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogWarning($"�� �������� ������ : {task.Exception.Message}");
                    return;
                }

                DataSnapshot snapshot = task.Result;
                Debug.Log($"Found {snapshot.GetRawJsonValue()}");
                if (snapshot.Exists)
                {
                    foreach (var childSnapshot in snapshot.Children)
                    {

                        string result = childSnapshot.GetRawJsonValue();
                        Debug.Log(result);
                        
                        //stageData = JsonUtility.FromJson<StageData>(snapshot.GetRawJsonValue());
                        
                    }
                }
               

                //Debug.Log($"Found {snapshot.GetRawJsonValue()}");
                //Debug.Log("�������� ����");
 
                //Debug.Log($"{stageData.stageName}");
                //Debug.Log($"{stageData.timeLimit}");
                //Debug.Log($"{stageData.isClear}");
            });
    }
    public void getSt(int num)  // ���� ����
    {
        if (num == 1)
        {
            int[] ints = {1,2,3};
            StartStage(3, ints, 3, ints);
        }
        else if (num == 2) 
        {
            int[] ints = { 4, 7, 8 };
            StartStage(3, ints, 3, ints);
        }
        else if (num == 3) 
        {
            int[] ints = { 1, 6, 3 };
            StartStage(3, ints, 3, ints);
        }

    }
    // ������������ �̸� ���õǾ�� �ϴ� ��
    // ��� , ������ ui , ĳ���Ϳ� ���Ͱ� ��ġ�� ��ġ, �����  
    public void StartStage(int monsterCount,int[] monsterpos,int playerCount, int[] myPos)
    {
        for (int i = 0; i < monsterCount; i++) 
        {
            Instantiate(monster, enemyPoss[monsterpos[i]].position, Quaternion.identity);
        }
        for (int i = 0; i < playerCount; i++) 
        {
            Instantiate(player, myPoss[myPos[i]].position, Quaternion.identity);
        }
    }

    public void StageClear() 
    {

    }
}
