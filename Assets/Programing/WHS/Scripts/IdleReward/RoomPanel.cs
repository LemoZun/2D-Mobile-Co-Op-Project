using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomPanel : UIBInder
{
    private IdleReward idleReward;

    private void Awake()
    {
        BindAll();

        AddEvent("ClaimButton", EventType.Click, ClaimIdleRewards);
    }

    private void Start()
    {
        idleReward = GetComponent<IdleReward>();

        UpdateClaimButtonState();
    }

    private void OnEnable()
    {
        // 1�ð��� ������ ���� ��ư Ȱ��ȭ
        // GetUI<UnityEngine.UI.Button>("ClaimButton").interactable = idleReward.HasIdleReward();
    }

    // Room ���� �� RoomExitTime ������ ����
    private void OnDisable()
    {
        idleReward.SaveExitTime();
    }

    public void UpdateClaimButtonState()
    {
        // ClaimButton�� ��ȣ�ۿ� ���� ���θ� idleReward�� ����� ���� ����
        GetUI<UnityEngine.UI.Button>("ClaimButton").interactable = idleReward.HasIdleReward();
    }

    private void OnApplicationQuit()
    {
        idleReward.SaveExitTime();
    }

    public void ClaimIdleRewards(PointerEventData eventData)
    {
        if (GetUI<UnityEngine.UI.Button>("ClaimButton").interactable == false)
            return;

        int storedGold = PlayerDataManager.Instance.PlayerData.StoredItems[(int)E_Item.Coin];
        int storedDinoBlood = PlayerDataManager.Instance.PlayerData.StoredItems[(int)E_Item.DinoBlood];
        int storedBoneCrystal = PlayerDataManager.Instance.PlayerData.StoredItems[(int)E_Item.BoneCrystal];

        // ��ġ�� ������ �÷��̾��� �����ۿ� �߰�
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Coin, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] + storedGold);
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.DinoBlood, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] + storedDinoBlood);
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.BoneCrystal, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] + storedBoneCrystal);

        // ��ġ�� ���� ����
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.Coin, 0);
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.DinoBlood, 0);
        PlayerDataManager.Instance.PlayerData.SetStoredItem((int)E_Item.BoneCrystal, 0);

        // Firebase�� ������Ʈ�� ������ ���� ����
        UpdateItemsInDatabase();

        // ��ġ������ ���� �� ����ð� ����
        idleReward.SaveExitTime();

        UpdateClaimButtonState();
    }

    // �����ͺ��̽��� ������ ����
    private void UpdateItemsInDatabase()
    {
        // string userId = BackendManager.Auth.CurrentUser.UserId;
        string userId = "sinEKs9IWRPuWNbboKov1fKgmab2";
        DatabaseReference userRef = BackendManager.Database.RootReference.Child("UserData").Child(userId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            ["_items/0"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin],
            ["_items/1"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood],
            ["_items/2"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal],
            ["_storedItems/0"] = 0,
            ["_storedItems/1"] = 0,
            ["_storedItems/2"] = 0
        };

        userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log($"��ġ�� ������ ���� ���� {task.Exception}");
            }
            if (task.IsCanceled)
            {
                Debug.LogError($"��ġ�� ������ ���� �ߴܵ� {task.Exception}");
            }
            Debug.Log("��ġ�� ���� ���� ����");
        });
    }
}