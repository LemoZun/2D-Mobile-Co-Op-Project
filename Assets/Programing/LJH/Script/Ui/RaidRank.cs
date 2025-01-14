using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class RaidRank : MonoBehaviour
{
    [SerializeField] List<RaidData> raidDatas;
    private void OnEnable()
    {   
        raidDatas = new List<RaidData>();
        getRankingData();
    }
    private void CheckSnapSHot(List<DataSnapshot> snapshotChildren)
    {
        while (snapshotChildren == null || snapshotChildren.Count == 0)
        {
            Debug.Log("snapshot null����! �Ǵ� List�� 0����");
        }
    }

    public void getRankingData() 
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;

        DatabaseReference root = BackendManager.Database.RootReference.Child("RaidData");

        root.KeepSynced(true);

        root.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            DataSnapshot snapShot = task.Result;

            // ID���� �������� SnapShot�� List �߰����Դ��� Ȯ��.
            var RaidChildren = snapShot.Children.ToList();
            CheckSnapSHot(RaidChildren);


            System.Random random = new System.Random();
            RaidChildren = RaidChildren.OrderBy(item => random.Next()).ToList();

            for (int i = 0; i < RaidChildren.Count; i++)
            {
 
                RaidData data = new RaidData();
                data.Name = RaidChildren[i].Child("_name").Value.ToString();
                data.TotalDamage = int.Parse(RaidChildren[i].Child("_totalDamage").Value.ToString());

                Debug.Log($"{data.Name} :  {data.TotalDamage}");
                raidDatas.Add(data);
                //RaidChildren[i].Child("_name");
                //RaidChildren[i].Child("_totalDamage");

            }




        });
    }
}
