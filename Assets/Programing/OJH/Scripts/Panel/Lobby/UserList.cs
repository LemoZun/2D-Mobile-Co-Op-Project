using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserList : MonoBehaviour
{
    [SerializeField] private string _otherId;

    public string OtherId { get { return _otherId; } set { _otherId = value; } }

    [SerializeField] private int _giveCoin;

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
        GiveCoin();
    }

    //canFollow ���� ���� �� Firebase�� Update
    private void DecreaseCanFollow()
    {
        PlayerDataManager.Instance.PlayerData.CanAddFriend--;
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(BackendManager.Auth.CurrentUser.UserId).Child("_canAddFriend");
        root.SetValueAsync(PlayerDataManager.Instance.PlayerData.CanAddFriend);
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
