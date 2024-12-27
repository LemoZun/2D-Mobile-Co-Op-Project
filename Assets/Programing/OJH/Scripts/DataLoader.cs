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

public class DataLoader : MonoBehaviour
{

    [SerializeField] private string _uID;

    //�ȷο� ���� �ð�
    [SerializeField] private int _resetFollowTime;

    //�ȷο� origin ��
    [SerializeField] private int _originFollowTime;

    [ContextMenu("Test")]
    public void Test()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;

        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(_uID);

        Debug.Log(root);

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

            PlayerDataManager.Instance.PlayerData.LastResetFollowTime = snapShot.Child("_lastResetFollowTime").Value.ToString();

            PlayerDataManager.Instance.PlayerData.RoomExitTime = snapShot.Child("_roomExitTime").Value.ToString();

            PlayerDataManager.Instance.PlayerData.GiftCoin = TypeCastManager.Instance.TryParseInt(snapShot.Child("_giftCoin").Value.ToString());

            PlayerDataManager.Instance.PlayerData.CanFollow = TypeCastManager.Instance.TryParseInt(snapShot.Child("_canFollow").Value.ToString());

            DateTime resetTime = DateTime.Parse(PlayerDataManager.Instance.PlayerData.LastResetFollowTime);

            //��¥�� �ٸ���, 8�� �̻��϶� _canFollow �ʱ�ȭ���ֱ�
            if (resetTime.Date != DateTime.Now.Date && DateTime.Now.Hour >= _resetFollowTime)
            {
                PlayerDataManager.Instance.PlayerData.CanFollow = _originFollowTime;
                root.Child("_canFollow").SetValueAsync(_originFollowTime);
            }


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


            //string followid List�� ��������
            var followingIdChildren = snapShot.Child("_followingIds").Children.ToList();
            CheckSnapSHot(followingIdChildren);

            followingIdChildren = followingIdChildren.OrderBy(followingId => TypeCastManager.Instance.TryParseInt(followingId.Key)).ToList();
            for (int i = 0; i < followingIdChildren.Count; i++)
            {
                PlayerDataManager.Instance.PlayerData.IsStageClear[i] = TypeCastManager.Instance.TryParseBool(followingIdChildren[i].Value.ToString());
            }


            //UniData��������
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

        });

    }

    //Snapshot�� ����� �ҷ��������� üũ�ϴ� �Լ� -> snapshot�� �ҷ������µ� �����ð��� �ణ �ִ°����� ������ ��.
    private void CheckSnapSHot(List<DataSnapshot> snapshotChildren)
    {
        while (snapshotChildren == null || snapshotChildren.Count == 0)
        {
            Debug.Log("snapshot null����!");
        }

        for(int i = 0; i < snapshotChildren.Count; i++)
        {
            Debug.Log(snapshotChildren[i].Key.ToString());
        }
    }
}
