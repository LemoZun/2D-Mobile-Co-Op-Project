using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailPanel : UIBInder
{
    [SerializeField] private GameObject _mailList;

    [SerializeField] private Transform _content; // content �ڽ����� �ֱ� ����.

    [SerializeField] private AutoFalseSetter _getCoinImage;

    [SerializeField] private ListObjectPull _pull;

    private List<MailList> _mailLists;

    private void Awake()
    {
        BindAll();
    }
    private void Start()
    {
        _mailLists = new List<MailList>();
    }

    private void OnEnable()
    {
        GetUI<Button>("MailAllCheckButton").onClick.AddListener(CheckAllMail);
        GetMailData();
    }

    private void OnDisable()
    {
        GetUI<Button>("MailAllCheckButton").onClick.RemoveListener(CheckAllMail);
        ResetMailLists();
    }

    private void GetMailData()
    {

        DatabaseReference root = BackendManager.Database.RootReference.Child("MailData").Child(BackendManager.Auth.CurrentUser.UserId);
        root.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            DataSnapshot snapShot = task.Result;

            // ���ϵ� �������� 
            var mails = snapShot.Children.ToList();

            if(mails == null || mails.Count == 0)
            {
                Debug.Log("mail�� �����ϴ�");
                return;
            }
            CheckSnapSHot(mails);

            mails = mails.OrderByDescending(mail => mail.Key.ToString()).ToList();

            for(int i = 0; i < mails.Count; i++)
            {
                GameObject mailListObject = _pull.Get((int)E_List.Mail, _content);
                MailList mailList = mailListObject.GetComponent<MailList>();
                _mailLists.Add(mailList);
                mailList.MailTime = mails[i].Key.ToString();
                mailList.ItemType = TypeCastManager.Instance.TryParseInt(mails[i].Child("_itemType").Value.ToString());
                mailList.ItemNum = TypeCastManager.Instance.TryParseInt(mails[i].Child("_itemNum").Value.ToString());


                // MailList Text ��¥, �̸�, ����������, ���� ����.
                StringBuilder sb = new StringBuilder();
                FindTime(mailList.MailTime, sb);
                sb.Append("  ");
                sb.Append(mails[i].Child("_name").Value.ToString());
                sb.Append("  ");
                sb.Append(((E_Item)mailList.ItemType).ToString());
                sb.Append("  ");
                sb.Append(mailList.ItemNum.ToString());

                mailList.MailText.SetText(sb);
            }
            
        });

    }

    private void FindTime(string time,StringBuilder sb)
    {
        // Time 
        string year = time.Substring(0, 4);
        string month = time.Substring(4, 2);
        string day = time.Substring(6, 2);
        string hour = time.Substring(9, 2);
        string minute = time.Substring(11, 2);

        string formattedTime = $"{year}/{month}/{day}/{hour}:{minute}";
        sb.Append(formattedTime);
        sb.Append("\n");
    }

    private void CheckSnapSHot(List<DataSnapshot> snapshotChildren)
    {
        while (snapshotChildren == null || snapshotChildren.Count == 0)
        {
            Debug.Log("snapshot null����! �Ǵ� List�� 0����");
        }
    }

    // �ϰ�����
    private void CheckAllMail()
    {
        UpdateAllItem();
        DeleteAllMail();
        ResetMailLists();
    }

    private void UpdateAllItem()
    {

        int[] items = new int[(int)E_Item.Length];

        //��� ���� Ȯ���ϰ� �����ϱ�
        foreach(MailList mailList in _mailLists)
        {
            items[mailList.ItemType] += mailList.ItemNum;
        }

        //PlayerData�� ���� Update ���ֱ�
        for(int i = 0; i < (int)E_Item.Length; i++)
        {
            int sum = PlayerDataManager.Instance.PlayerData.Items[i] + items[i];
            PlayerDataManager.Instance.PlayerData.SetItem(i, sum);
        }

        //�鿣�忡�� Update���ֱ�
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId).Child("_items");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        for (int i = 0; i < (int)E_Item.Length; i++)
        {
            dic[$"/{i}"] = PlayerDataManager.Instance.PlayerData.Items[i];
        }
        root.UpdateChildrenAsync(dic);
        Debug.Log("���������� �鿣�忡�� items ������");
    }

    // ���� �� �������ֱ�
    private void DeleteAllMail()
    {
        DatabaseReference root = BackendManager.Database.RootReference.Child("MailData").Child(BackendManager.Auth.CurrentUser.UserId);
        root.RemoveValueAsync();
    }

    private void ResetMailLists()
    {
        foreach (MailList mailList in _mailLists)
        {
            mailList.gameObject.SetActive(false);
        }
        _mailLists.Clear();
    }


}
