using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpPanel : UIBInder
{
    private PlayerUnitData targetCharacter;
    private int maxLevelUp;
    private int curLevelUp;
    private const int MAXLEVEL = 30;

    private Dictionary<int, Dictionary<string, string>> levelUpData;

    private struct RequiredItems
    {
        public int coin;
        public int dinoBlood;
        public int boneCrystal;
    }

    private void Awake()
    {
        BindAll();
        AddEvent("LevelUpConfirm", EventType.Click, OnConfirmButtonClick);
        GetUI<Slider>("LevelUpSlider").onValueChanged.AddListener(OnSliderValueChanged);
        AddEvent("DecreaseButton", EventType.Click, OnDecreaseButtonClick);
        AddEvent("IncreaseButton", EventType.Click, OnIncreaseButtonClick);

        levelUpData = CsvDataManager.Instance.DataLists[(int)E_CsvData.CharacterLevelUp];
        Debug.Log($"levelUpData count: {levelUpData.Count}");
        foreach (var key in levelUpData.Keys)
        {
            Debug.Log($"Key: {key}, Value: {levelUpData[key]["500"]}, {levelUpData[key]["501"]}, {levelUpData[key]["502"]}");
        }
    }

    private void OnEnable()
    {
        if (PlayerDataManager.Instance.PlayerData.OnItemChanged == null)
        {
            PlayerDataManager.Instance.PlayerData.OnItemChanged = new UnityAction<int>[System.Enum.GetValues(typeof(E_Item)).Length];
        }

        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Coin] += UpdateCoinText;
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoBlood] += UpdateDinoBloodText;
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.BoneCrystal] += UpdateBoneCrystalText;
    }

    private void OnDisable()
    {
        if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.PlayerData != null)
        {
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Coin] -= UpdateCoinText;
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoBlood] -= UpdateDinoBloodText;
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.BoneCrystal] -= UpdateBoneCrystalText;
        }
    }


    public void Initialize(PlayerUnitData character)
    {
        targetCharacter = character;
        CalculateMaxLevelUp();
        UpdateUI();

        Slider slider = GetUI<Slider>("LevelUpSlider");
        slider.minValue = 0;
        slider.maxValue = maxLevelUp;
        slider.value = 0;
    }

    // �����̴� �� ����
    private void OnSliderValueChanged(float value)
    {
        curLevelUp = Mathf.RoundToInt(value);
        UpdateUI();
    }

    // ��ȭ�Ҹ� ����
    private void UpdateUI()
    {
        RequiredItems items = CalculateRequiredItems(curLevelUp);

        GetUI<TextMeshProUGUI>("CoinText").text = $"Coin : {items.coin}";
        GetUI<TextMeshProUGUI>("DinoBloodText").text = $"DinoBlood : {items.dinoBlood}";
        GetUI<TextMeshProUGUI>("BoneCrystalText").text = $"BoneCrystal : {items.boneCrystal}";

        if (targetCharacter.UnitLevel + curLevelUp >= MAXLEVEL)
        {
            GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{targetCharacter.UnitLevel} -> Lv.{MAXLEVEL} (MAX)";
        }
        else
        {
            GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{targetCharacter.UnitLevel} -> Lv.{targetCharacter.UnitLevel + curLevelUp}";
        }
    }

    // ������ �� �ִ�ġ
    private void CalculateMaxLevelUp()
    {
        maxLevelUp = 0;
        while (CanLevelUp(maxLevelUp + 1)
            && (targetCharacter.UnitLevel + maxLevelUp + 1) <= MAXLEVEL)
        {
            maxLevelUp++;
        }
    }

    // ������ ���� ����
    private bool CanLevelUp(int level)
    {
        if (targetCharacter.UnitLevel + level > MAXLEVEL)
        {
            return false;
        }

        RequiredItems items = CalculateRequiredItems(level);

        // �������� ����� ���� ������ ������ true
        return PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] >= items.coin &&
               PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] >= items.dinoBlood &&
               PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] >= items.boneCrystal;
    }

    // �䱸 ��ȭ�� ���
    private RequiredItems CalculateRequiredItems(int level)
    {
        RequiredItems items = new RequiredItems();  

        for (int i = 0; i < level; i++)
        {
            // TODO : LevelUpID�� ����� ���� ���� �ʿ�
            int levelUpId = 4000 + ((targetCharacter.UnitLevel + i) * 10);

            if (levelUpData.TryGetValue(levelUpId, out Dictionary<string, string> data))
            {
                items.coin += int.Parse(data["500"].Replace(",", ""));
                items.dinoBlood += int.Parse(data["501"].Replace(",", ""));
                if (data.ContainsKey("502") && !string.IsNullOrEmpty(data["502"]))
                {
                    items.boneCrystal += int.Parse(data["502"].Replace(",", ""));
                }
            }
            else
            {
                Debug.LogError($"������ �����͸� ã�� �� �����ϴ�. LevelUpID: {levelUpId}");
            }
        }
        return items;
    }

    // ������ ��ư
    private void OnConfirmButtonClick(PointerEventData eventData)
    {
        for (int i = 0; i < curLevelUp; i++)
        {
            if (LevelUp(targetCharacter) == false)
            {
                break;
            }
        }
        gameObject.SetActive(false);
    }

    // ������
    private bool LevelUp(PlayerUnitData character)
    {
        RequiredItems items = CalculateRequiredItems(1);

        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] >= items.coin &&
            PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] >= items.dinoBlood &&
            PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] >= items.boneCrystal)
        {
            PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Coin, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] - items.coin);
            PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.DinoBlood, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] - items.dinoBlood);
            PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.BoneCrystal, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] - items.boneCrystal);

            // TODO : �����ͺ��̽��� �Ҹ��� ������ ������Ʈ

            character.UnitLevel++;
            Debug.Log($"{character.UnitId} ������ {character.UnitLevel}");
            if (items.boneCrystal > 0)
            {
                Debug.Log($"�� ũ����Ż {items.boneCrystal} �Ҹ�");
            }

            UpdateCharacters(character);
            UpdateLevelData(character);

            return true;
        }

        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] < items.coin)
        {
            Debug.Log($"������ {items.coin - PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin]} �����մϴ�.");
        }
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] < items.dinoBlood)
        {
            Debug.Log($"���̳���尡 {items.dinoBlood - PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood]} �����մϴ�");
        }
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] < items.boneCrystal)
        {
            Debug.Log($"��ũ����Ż�� {items.boneCrystal - PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal]} �����մϴ�");
        }

        return false;
    }

    // db ������ ������ ����
    private void UpdateLevelData(PlayerUnitData character)
    {
        string userID = BackendManager.Auth.CurrentUser.UserId;
        DatabaseReference characterRef = BackendManager.Database.RootReference
            .Child("UserData").Child(userID).Child("_unitDatas");

        characterRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("ĳ���� ������ �ε� �� ���� �߻�: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            foreach (var childSnapshot in snapshot.Children)
            {
                if (int.Parse(childSnapshot.Child("_unitId").Value.ToString()) == character.UnitId)
                {
                    Dictionary<string, object> updates = new Dictionary<string, object>
                    {
                        ["_unitLevel"] = character.UnitLevel
                    };

                    childSnapshot.Reference.UpdateChildrenAsync(updates).ContinueWithOnMainThread(updateTask =>
                    {
                        if (updateTask.IsCompleted)
                        {
                            Debug.Log($"ĳ���� ID {character.UnitId}�� ������ {character.UnitLevel}�� ������Ʈ��");
                        }
                        else
                        {
                            Debug.LogError($"ĳ���� ID {character.UnitId} ������Ʈ ����: " + updateTask.Exception);
                        }
                    });
                    break;
                }
            }
        });
    }

    // UI ����
    private void UpdateCharacters(PlayerUnitData character)
    {
        CharacterPanel characterPanel = FindObjectOfType<CharacterPanel>();
        if (characterPanel != null)
        {
            characterPanel.UpdateCharacterInfo(character);
        }

        InventoryPanel inventoryPanel = FindObjectOfType<InventoryPanel>();
        if (inventoryPanel != null)
        {
            inventoryPanel.UpdateCharacterUI(character);
        }
    }

    private void OnDecreaseButtonClick(PointerEventData eventData)
    {
        if (curLevelUp > 0)
        {
            curLevelUp--;
            UpdateUI();
            GetUI<Slider>("LevelUpSlider").value = curLevelUp;
        }
    }

    private void OnIncreaseButtonClick(PointerEventData eventData)
    {
        if (curLevelUp < maxLevelUp && (targetCharacter.UnitLevel + curLevelUp) + 1 <= MAXLEVEL)
        {
            curLevelUp++;
            UpdateUI();
            GetUI<Slider>("LevelUpSlider").value = curLevelUp;
        }
    }

    private void UpdateCoinText(int newValue)
    {
        GetUI<TextMeshProUGUI>("CoinText").text = $"Coin : {newValue}";
        CalculateMaxLevelUp();
        UpdateUI();
    }

    private void UpdateDinoBloodText(int newValue)
    {
        GetUI<TextMeshProUGUI>("DinoBloodText").text = $"DinoBlood : {newValue}";
        CalculateMaxLevelUp();
        UpdateUI();
    }

    private void UpdateBoneCrystalText(int newValue)
    {
        GetUI<TextMeshProUGUI>("BoneCrystalText").text = $"BoneCrystal : {newValue}";
        CalculateMaxLevelUp();
        UpdateUI();
    }
}
