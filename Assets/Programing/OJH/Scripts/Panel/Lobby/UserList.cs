using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserList : MonoBehaviour
{
    [SerializeField] private string _otherId;

    public string OtherId { get { return _otherId; } set { _otherId = value; } }

    [SerializeField] private int _giveCoin;

    [SerializeField] private Button _button;

    public void AddFriend()
    {
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

        Debug.Log("bbbb");
        
    }

    // ģ�� �߰��ϸ鼭 Coin ����
    private void GiveCoin()
    {
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(_otherId).Child("_gift");

        root.RunTransaction(mutableData =>
        {
            //_gift ��ųʸ��� ���� ��� 
            if(mutableData.Value == null)
            {
                Dictionary<string, object> newGift = new Dictionary<string, object>();
                newGift.Add(BackendManager.Auth.CurrentUser.UserId, _giveCoin);
                Debug.Log(newGift[BackendManager.Auth.CurrentUser.UserId]);
                mutableData.Value = newGift;
                return TransactionResult.Success(mutableData);
            }

            Dictionary<string, object> gift = mutableData.Value as Dictionary<string, object>;
            // Id�������ϴٴ� ���� ������ �ȷο��� �� ������ �ȹ��� ����.
            if (gift.ContainsKey(BackendManager.Auth.CurrentUser.UserId))
            {
                gift[BackendManager.Auth.CurrentUser.UserId] = (int)gift[BackendManager.Auth.CurrentUser.UserId] + _giveCoin;
            }
            else
            {
                gift[BackendManager.Auth.CurrentUser.UserId] = _giveCoin;
            }

            mutableData.Value = gift;
            Debug.Log("bbb");
            return TransactionResult.Success(mutableData);

        });
    }
}
