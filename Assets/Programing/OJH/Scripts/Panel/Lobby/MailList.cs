using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MailList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _mailText;

    public TextMeshProUGUI MailText {  get { return _mailText; } set { _mailText = value; } }

    [SerializeField] private string _mailTime;

    public string MailTime { get { return _mailTime; } set { _mailTime = value; } }

    [SerializeField] private int _itemType;

    public int ItemType { get { return _itemType; } set { _itemType = value; } }

    [SerializeField] private int _itemNum;

    public int ItemNum {  get { return _itemNum; } set { _itemNum = value; } }


    public void CheckMail()
    {
        UpdateItem();
        DeleteMail();

        gameObject.SetActive(false);
    }

    // ���� ������Ʈ
    private void UpdateItem()
    {
        //PlayerData Coin �� Update.
        int sum = PlayerDataManager.Instance.PlayerData.Items[_itemType] + _itemNum;
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Coin, sum);

        //�鿣�忡�� ����
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId).Child("_items");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic[$"/{_itemType}"] = PlayerDataManager.Instance.PlayerData.Items[_itemType];
        root.UpdateChildrenAsync(dic);
    }

    //���ɹ��� ���� ����
    private void DeleteMail()
    {
        DatabaseReference root = BackendManager.Database.RootReference.Child("MailData").Child(BackendManager.Auth.CurrentUser.UserId);
        root.Child(_mailTime).RemoveValueAsync();

    }
}
