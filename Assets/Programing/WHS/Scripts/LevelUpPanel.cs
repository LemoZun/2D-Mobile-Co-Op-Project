using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        return Inventory.instance.GetItemAmount(ItemID.Coin) >= items.coin &&
               Inventory.instance.GetItemAmount(ItemID.DinoBlood) >= items.dinoBlood &&
               Inventory.instance.GetItemAmount(ItemID.BoneCrystal) >= items.boneCrystal;
    }

    // �䱸 ��ȭ�� ���
    private RequiredItems CalculateRequiredItems(int level)
    {
        RequiredItems items = new RequiredItems();  

        for (int i = 0; i < level; i++)
        {
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

    // ������ ���� ��ȭ �䱸�� ���
    private int CalculateRequiredCoin(int levels)
    {
        int total = 0;
        for (int i = 0; i < levels; i++)
        {
            total += 30 + (targetCharacter.UnitLevel + i - 1) * 60;
        }
        return total;
    }

    // ���̳���� �䱸�� ���
    private int CalculateRequiredDinoBlood(int levels)
    {
        int total = 0;
        for (int i = 0; i < levels; i++)
        {
            total += 90 + (targetCharacter.UnitLevel + i - 1) * 70;
        }
        return total;
    }

    // ������ ���� ��ũ����Ż �䱸�� ���
    private int CalculateRequiredBoneCrystal(int levels)
    {
        int total = 0;

        for (int i = 0; i < levels; i++)
        {
            // 5�������� ��ũ����Ż �䱸��
            if ((targetCharacter.UnitLevel + i) % 5 == 4)
            {
                total += 10 + (((targetCharacter.UnitLevel + i + 1) / 5) * 40);
            }
        }
        return total;
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

        if (Inventory.instance.GetItemAmount(ItemID.Coin) >= items.coin &&
            Inventory.instance.GetItemAmount(ItemID.DinoBlood) >= items.dinoBlood &&
            Inventory.instance.GetItemAmount(ItemID.BoneCrystal) >= items.boneCrystal)
        {
            if (Inventory.instance.SpendItem(ItemID.Coin, items.coin) &&
                Inventory.instance.SpendItem(ItemID.DinoBlood, items.dinoBlood) &&
                (items.boneCrystal == 0 || Inventory.instance.SpendItem(ItemID.BoneCrystal, items.boneCrystal)))
            {
                character.UnitLevel++;
                Debug.Log($"{character.UnitId} ������ {character.UnitLevel}");
                if (items.boneCrystal > 0)
                {
                    Debug.Log($"�� ũ����Ż {items.boneCrystal} �Ҹ�");
                }

                UpdateCharacters(character);
                ItemUI.instance.UpdateCurrencyUI();
                UpdateLevelData(character);

                return true;
            }
        }

        if (Inventory.instance.GetItemAmount(ItemID.Coin) < items.coin)
        {
            Debug.Log($"������ {Inventory.instance.GetItemAmount(ItemID.Coin) - items.coin} �����մϴ�.");
        }
        if (Inventory.instance.GetItemAmount(ItemID.DinoBlood) < items.dinoBlood)
        {
            Debug.Log($"���̳���尡 {Inventory.instance.GetItemAmount(ItemID.DinoBlood) - items.dinoBlood} �����մϴ�");
        }
        if (Inventory.instance.GetItemAmount(ItemID.BoneCrystal) < items.boneCrystal)
        {
            Debug.Log($"��ũ����Ż�� {Inventory.instance.GetItemAmount(ItemID.BoneCrystal) - items.boneCrystal} �����մϴ�");
        }

        return false;
    }

    // ������ ����
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

        CharacterInventoryUI characterInventoryUI = FindObjectOfType<CharacterInventoryUI>();
        if (characterInventoryUI != null)
        {
            characterInventoryUI.UpdateCharacterUI(character);
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
}
