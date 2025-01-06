using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserList : MonoBehaviour
{
    [SerializeField] private string _otherId;

    public string OtherId { get { return _otherId; } set { _otherId = value; } }

    [SerializeField] private int _giveCoin;

    [SerializeField] private Button _button;

    private AutoFalseSetter _getCoinImage;

    public AutoFalseSetter GetCoinImage { get { return _getCoinImage; } set { _getCoinImage = value; } }


    private AutoFalseSetter _cantAddImage;

    public AutoFalseSetter CantAddImage { get { return _cantAddImage; } set { _cantAddImage = value; } }

    public void AddFriend()
    {
        if(PlayerDataManager.Instance.PlayerData.CanAddFriend <= 0)
        {
            _cantAddImage.ResetCurrentTime();
            _cantAddImage.gameObject.SetActive(true);
            Debug.Log("�Ϸ� ģ���߰� Ƚ�� 0");
            return;
        }

        //�̹� ģ���� ��� return
        foreach(string id in PlayerDataManager.Instance.PlayerData.FriendIds)
        {
            if(id == _otherId)
            {
                Debug.Log("�̹� ģ�� �߰� ��û");
                return;
            }
        }

        //PlayerData List�� �߰�
        PlayerDataManager.Instance.PlayerData.FriendIds.Add(_otherId);

        //Firebase�� �߰�
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId).Child("_friendIds");
        root.SetValueAsync(PlayerDataManager.Instance.PlayerData.FriendIds);


        Debug.Log("ģ���߰�!");
        DecreaseCanFollow();
        GetCoin();
        GiveCoin();

        //ģ�� �߰� ���������� �����ٸ� ��ȣ�ۿ� false.
        _button.interactable = false;
        _getCoinImage.gameObject.SetActive(true);
        _getCoinImage.ResetCurrentTime();
    }

    //canFollow ���� ���� �� Firebase�� Update
    private void DecreaseCanFollow()
    {
        PlayerDataManager.Instance.PlayerData.CanAddFriend--;
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId).Child("_canAddFriend");
        root.SetValueAsync(PlayerDataManager.Instance.PlayerData.CanAddFriend);
    }

    // ģ�� �߰��� ���ε� Coin �ޱ�
    private void GetCoin()
    {
        //Coin �� ����
        int coin = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] + _giveCoin;
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Coin, coin);

        //Backend������ ����
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId).Child("_items");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic[$"/{(int)E_Item.Coin}"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin];
        root.UpdateChildrenAsync(dic);      
    }

    // ģ�� �߰��ϸ鼭 Coin ����
    private void GiveCoin()
    {
        string sendMailTime = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + DateTime.Now.Ticks.ToString("D19");

        DatabaseReference root = BackendManager.Database.RootReference.Child("MailData").Child(_otherId).Child(sendMailTime);

        MailData mailData = new MailData();

        mailData.Name = $"{PlayerDataManager.Instance.PlayerData.PlayerName}#{BackendManager.Auth.CurrentUser.UserId.Substring(0, 4)}";

        mailData.ItemType = (int)E_Item.Coin;

        mailData.ItemNum = _giveCoin;

        string json = JsonUtility.ToJson(mailData);
        root.SetRawJsonValueAsync(json);

    }

}
