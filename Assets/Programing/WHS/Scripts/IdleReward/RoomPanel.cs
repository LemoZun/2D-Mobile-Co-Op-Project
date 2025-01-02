using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomPanel : UIBInder
{
    private IdleReward idleReward;
    [SerializeField] private GameObject idleRewardPanel;
    [SerializeField] private ItemPanel itemPanel;

    private Coroutine updateIdleTimeCoroutine;

    private void Awake()
    {
        BindAll();

        AddEvent("ClaimButton", EventType.Click, ClaimIdleRewards);
        AddEvent("CheckRewardButton", EventType.Click, ShowIdleRewardPanel);
    }

    private void Start()
    {
        idleReward = GetComponent<IdleReward>();

        idleReward.CalculateIdleReward();
    }

    private void OnEnable()
    {
        GetUI<UnityEngine.UI.Button>("ClaimButton").interactable = idleReward.HasIdleReward();

      
        if (updateIdleTimeCoroutine == null)
        {
            updateIdleTimeCoroutine = StartCoroutine(UpdateIdleTimeCoroutine());
        }
    }
    private void OnDisable()
    {
        if (updateIdleTimeCoroutine != null)
        {
            StopCoroutine(updateIdleTimeCoroutine);
            updateIdleTimeCoroutine = null;
        }
    }

    /*
    public void TESTESTS()
    {
        if (updateIdleTimeCoroutine == null)
        {
            updateIdleTimeCoroutine = StartCoroutine(UpdateIdleTimeCoroutine());
        }
    }
    */

    // ���� ����
    public void ClaimIdleRewards(PointerEventData eventData)
    {
        if (GetUI<Button>("ClaimButton").interactable == false)
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

        ShowPopup(storedGold, storedDinoBlood, storedBoneCrystal);

        // Firebase�� ������Ʈ�� ������ ���� ����
        UpdateItemsInDatabase();

        // ��ġ������ ���� �� ����ð� ����
        idleReward.SaveExitTime();

        idleRewardPanel.SetActive(false);

        itemPanel.UpdateItems();
    }

    // �����ͺ��̽��� ������ ����
    private void UpdateItemsInDatabase()
    {
        string userId = BackendManager.Auth.CurrentUser.UserId;
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

    // ��ġ�ð� Ÿ�̸� �ؽ�Ʈ
    private IEnumerator UpdateIdleTimeCoroutine()
    {
        DateTime lastTime = DateTime.Now;

        while (true)
        {
            TimeSpan idleTime = idleReward.GetIdleTime();

            GetUI<TextMeshProUGUI>("IdleTimeText").text = $"{idleTime.Hours} : {idleTime.Minutes} : {idleTime.Seconds}";
            GetUI<Button>("ClaimButton").interactable = idleReward.HasIdleReward();

            TimeSpan elapsedTime = DateTime.Now - lastTime;

            if (elapsedTime.TotalSeconds >= 10)
            {
                idleReward.CalculateIdleReward();
                lastTime = DateTime.Now;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // ��ġ������ UI �г�
    private void ShowIdleRewardPanel(PointerEventData eventData)
    {
        idleReward.CalculateIdleReward();

        idleRewardPanel.SetActive(true);

        int goldReward = idleReward.CalculateReward(1, (int)idleReward.GetIdleTime().TotalSeconds);
        int dinoBloodReward = idleReward.CalculateReward(2, (int)idleReward.GetIdleTime().TotalSeconds);
        int boneCrystalReward = idleReward.CalculateReward(3, (int)idleReward.GetIdleTime().TotalSeconds);

        GetUI<TextMeshProUGUI>("GoldRewardText").text = $"Gold: {goldReward}";
        GetUI<TextMeshProUGUI>("DinoBloodRewardText").text = $"Dino Blood: {dinoBloodReward}";
        GetUI<TextMeshProUGUI>("BoneCrystalRewardText").text = $"Bone Crystal: {boneCrystalReward}";

        GetUI<Button>("ClaimButton").interactable = idleReward.HasIdleReward();
    }

    private void ShowPopup(int gold, int dinoBlood, int boneCrystal)
    {
        GetUI<TextMeshProUGUI>("GoldClaimText").text = $"Gold : {gold}";
        GetUI<TextMeshProUGUI>("DinoBloodClaimText").text = $"Gold : {dinoBlood}";
        GetUI<TextMeshProUGUI>("BoneCrystalClaimText").text = $"Gold : {boneCrystal}";
    }
}