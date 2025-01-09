using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpPanel : UIBInder
{
    private PlayerUnitData _targetCharacter;
    private int _maxLevelUp;
    private int _curLevelUp;
    private const int MAXLEVEL = 30;

    private Dictionary<int, Dictionary<string, string>> _levelUpData;
    private Dictionary<int, Dictionary<string, string>> _characterData;

    private CharacterPanel _characterPanel;
    private InventoryPanel _inventoryPanel;

    private struct RequiredItems
    {
        public int Coin;
        public int DinoBlood;
        public int BoneCrystal;
    }

    private void Awake()
    {
        BindAll();
        AddEvent("LevelUpConfirm", EventType.Click, OnConfirmButtonClick);
        GetUI<Slider>("LevelUpSlider").onValueChanged.AddListener(OnSliderValueChanged);
        AddEvent("DecreaseButton", EventType.Click, OnDecreaseButtonClick);
        AddEvent("IncreaseButton", EventType.Click, OnIncreaseButtonClick);

        _levelUpData = CsvDataManager.Instance.DataLists[(int)E_CsvData.CharacterLevelUp];
        _characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];

        _characterPanel = FindObjectOfType<CharacterPanel>();
        _inventoryPanel = FindObjectOfType<InventoryPanel>();

        Debug.Log($"levelUpData count: {_levelUpData.Count}");
        foreach (var key in _levelUpData.Keys)
        {
            Debug.Log($"Key: {key}, Value: {_levelUpData[key]["500"]}, {_levelUpData[key]["501"]}, {_levelUpData[key]["502"]}");
        }
    }

    private void OnEnable()
    {
        if (PlayerDataManager.Instance.PlayerData.OnItemChanged == null)
        {
            PlayerDataManager.Instance.PlayerData.OnItemChanged = new UnityAction<int>[Enum.GetValues(typeof(E_Item)).Length];
        }

        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Coin] += (value) => UpdateItemText(value, "Coin");
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoBlood] += (value) => UpdateItemText(value, "DinoBlood");
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.BoneCrystal] += (value) => UpdateItemText(value, "BoneCrystal");
    }

    private void OnDisable()
    {
        if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.PlayerData != null)
        {
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Coin] -= (value) => UpdateItemText(value, "Coin");
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoBlood] -= (value) => UpdateItemText(value, "DinoBlood");
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.BoneCrystal] -= (value) => UpdateItemText(value, "BoneCrystal");
        }
    }

    // ������ �г��� ������ �ʱ�ȭ
    public void Init(PlayerUnitData character)
    {
        _targetCharacter = character;
        CalculateMaxLevelUp();

        Slider slider = GetUI<Slider>("LevelUpSlider");
        slider.minValue = 1;
        slider.maxValue = _maxLevelUp;
        slider.value = 1;
        _curLevelUp = 1;

        UpdateUI();
    }

    // �����̴� �� ����
    private void OnSliderValueChanged(float value)
    {
        _curLevelUp = Mathf.RoundToInt(value);
        UpdateUI();
    }

    // ��ȭ�Ҹ� ����
    private void UpdateUI()
    {
        RequiredItems items = CalculateRequiredItems(_curLevelUp);

        int notEnoughCoin = Mathf.Max(0, items.Coin - PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin]);
        int notEnoughDinoBlood = Mathf.Max(0, items.DinoBlood - PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood]);
        int notEnoughBoneCrystal = Mathf.Max(0, items.BoneCrystal - PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal]);

        bool canLevelUp = (notEnoughCoin == 0 && notEnoughDinoBlood == 0 && notEnoughBoneCrystal == 0);

        GetUI<Button>("DecreaseButton").interactable = (_curLevelUp > 1);

        LoadItemImage("CoinImage", E_Item.Coin);
        LoadItemImage("DinoBloodImage", E_Item.DinoBlood);
        LoadItemImage("BoneCrystalImage", E_Item.BoneCrystal);

        if (canLevelUp)
        {
            GetUI<Button>("DecreaseButton").interactable = true;
            GetUI<Button>("IncreaseButton").interactable = true;
            GetUI<Slider>("LevelUpSlider").interactable = true;
            GetUI<RectTransform>("Handle Slide Area").gameObject.SetActive(true);

            GetUI<TextMeshProUGUI>("CoinText").text = $"Coin : {items.Coin}";
            GetUI<TextMeshProUGUI>("DinoBloodText").text = $"DinoBlood : {items.DinoBlood}";
            GetUI<TextMeshProUGUI>("BoneCrystalText").text = $"BoneCrystal : {items.BoneCrystal}";
        }
        else
        {
            GetUI<Button>("DecreaseButton").interactable = false;
            GetUI<Button>("IncreaseButton").interactable = false;
            GetUI<Slider>("LevelUpSlider").interactable = false;
            GetUI<RectTransform>("Handle Slide Area").gameObject.SetActive(false);

            if (notEnoughCoin > 0)
            {
                GetUI<TextMeshProUGUI>("CoinText").text = $"Coin {notEnoughCoin} ����";
            }
            else
            {
                GetUI<TextMeshProUGUI>("CoinText").text = $"Coin �����";
            }
            if (notEnoughDinoBlood > 0)
            {
                GetUI<TextMeshProUGUI>("DinoBloodText").text = $"DinoBlood {notEnoughDinoBlood} ����";
            }
            else
            {
                GetUI<TextMeshProUGUI>("DinoBloodText").text = $"DinoBlood �����";
            }
            if (notEnoughBoneCrystal > 0)
            {
                GetUI<TextMeshProUGUI>("BoneCrystalText").text = $"BoneCrystal {notEnoughBoneCrystal} ����";
            }
            else
            {
                GetUI<TextMeshProUGUI>("BoneCrystalText").text = $"BoneCrystal �����";
            }
        }

        if (_targetCharacter.UnitLevel + _curLevelUp > MAXLEVEL)
        {
            GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{_targetCharacter.UnitLevel} -> Lv.{MAXLEVEL} (MAX)";
        }
        else
        {
            GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{_targetCharacter.UnitLevel} -> Lv.{_targetCharacter.UnitLevel + _curLevelUp}";
        }

        GetUI<Button>("LevelUpConfirm").interactable = canLevelUp;
    }

    // ������ �� �ִ�ġ
    private void CalculateMaxLevelUp()
    {
        _maxLevelUp = 0;
        while (CanLevelUp(_maxLevelUp + 1) && (_targetCharacter.UnitLevel + _maxLevelUp + 1) <= MAXLEVEL)
        {
            _maxLevelUp++;
        }
    }

    // ������ ���� ����
    private bool CanLevelUp(int level)
    {
        if (_targetCharacter.UnitLevel + level > MAXLEVEL)
        {
            return false;
        }

        RequiredItems items = CalculateRequiredItems(level);

        // �������� ����� ������ �������϶� true
        return PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] >= items.Coin &&
               PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] >= items.DinoBlood &&
               PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] >= items.BoneCrystal;
    }

    // �䱸 ��ȭ�� ���
    private RequiredItems CalculateRequiredItems(int level)
    {
        RequiredItems items = new RequiredItems();
        int rarity = GetRarity();
        int endLevel = Math.Min(_targetCharacter.UnitLevel + level, MAXLEVEL);

        for (int curLevel = _targetCharacter.UnitLevel + 1; curLevel <= endLevel; curLevel++)
        {
            int levelUpId = FindLevelUpId(rarity, curLevel);

            if (_levelUpData.TryGetValue(levelUpId, out Dictionary<string, string> data))
            {
                if (int.TryParse(data["500"], out int coin))
                {
                    items.Coin += coin;
                }
                if (int.TryParse(data["501"], out int dinoBlood))
                {
                    items.DinoBlood += dinoBlood;
                }
                if (data.ContainsKey("502") && int.TryParse(data["502"], out int boneCrystal))
                {
                    items.BoneCrystal += boneCrystal;
                }
            }
            else
            {
                Debug.LogError($"������ �����͸� ã�� �� �����ϴ�. LevelUpID: {levelUpId}");
                return new RequiredItems();
            }
        }
        return items;
    }

    // ��� ��������
    private int GetRarity()
    {
        if (_characterData.TryGetValue(_targetCharacter.UnitId, out var data))
        {
            if (int.TryParse(data["Rarity"], out int rarity))
            {
                return rarity;
            }
        }
        else
        {
            Debug.LogError($"ID ã�� �� ���� {_targetCharacter.UnitId}");
        }

        return -1;
    }

    // LevelUpID ��������
    private int FindLevelUpId(int rarity, int level)
    {
        if (level > MAXLEVEL)
        {
            return -1;
        }

        foreach (var entry in _levelUpData)
        {
            // ����� ������ ���� ���� Ű = LevelUpID �޾ƿ���
            if (int.Parse(entry.Value["Rarity"]) == rarity &&
                int.Parse(entry.Value["Level"]) == level)
            {
                return entry.Key;
            }
        }

        Debug.Log($"LevelUpId ã�� ���� Rarity {rarity}, Level {level}");

        return -1;
    }

    // ������ Ȯ�� ��ư
    private void OnConfirmButtonClick(PointerEventData eventData)
    {
        if (_targetCharacter.UnitLevel + _curLevelUp > MAXLEVEL)
        {
            return;
        }

        RequiredItems items = CalculateRequiredItems(_curLevelUp);

        // �������� ������� Ȯ���ϰ� ������ ����
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] >= items.Coin &&
            PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] >= items.DinoBlood &&
            PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] >= items.BoneCrystal)
        {
            for (int i = 0; i < _curLevelUp; i++)
            {
                LevelUp(_targetCharacter);
            }

            // UI �� �����ͺ��̽� ������Ʈ
            UpdateItemsData(items);
            UpdateCharacters(_targetCharacter);
            UpdateLevelData(_targetCharacter);

            gameObject.SetActive(false);
        }
    }

    // ������
    private bool LevelUp(PlayerUnitData character)
    {
        RequiredItems items = CalculateRequiredItems(1); // ���� �������� ���� ������ ���

        if (items.Coin == 0 && items.DinoBlood == 0 && items.BoneCrystal == 0)
        {
            Debug.Log("�䱸 ������ ������ ã�� �� �����ϴ�.");
            return false;
        }

        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Coin, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] - items.Coin);
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.DinoBlood, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] - items.DinoBlood);
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.BoneCrystal, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] - items.BoneCrystal);

        character.UnitLevel++;

        // ������ �� PlayerDataManager ������Ʈ
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; i++)
        {
            if (PlayerDataManager.Instance.PlayerData.UnitDatas[i].UnitId == character.UnitId)
            {
                PlayerDataManager.Instance.PlayerData.UnitDatas[i] = character;
                break;
            }
        }

        Debug.Log($"{character.UnitId} ������ {character.UnitLevel}");

        return true;
    }

    // db�� �Ҹ��� ������ ����
    private void UpdateItemsData(RequiredItems items)
    {
        string userId = BackendManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = BackendManager.Database.RootReference.Child("UserData").Child(userId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            ["_items/0"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin],
            ["_items/1"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood],
            ["_items/2"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal]
        };

        userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"������ ������Ʈ ���� {task.Exception}");
            }
            if (task.IsCanceled)
            {
                Debug.Log($"������ ������Ʈ �ߴܵ� {task.Exception}");
            }

            Debug.Log("�Ҹ��� ������ ����");
        });
    }

    // db�� ������ ������ ����
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

    // �ܺ� UI ����
    private void UpdateCharacters(PlayerUnitData character)
    {
        // ĳ���� ������ �������� ���� ����
        _characterPanel?.UpdateCharacterInfo(character);

        // �κ��丮 ���Կ� �������� ���� ����
        _inventoryPanel?.UpdateCharacterUI(character);
    }

    // ���� ��ư
    private void OnDecreaseButtonClick(PointerEventData eventData)
    {
        if (GetUI<Button>("DecreaseButton").interactable == false)
        {
            return;

        }
        if (_curLevelUp > 0)
        {
            _curLevelUp--;
            UpdateUI();
            GetUI<Slider>("LevelUpSlider").value = _curLevelUp;
        }

    }

    // ���� ��ư
    private void OnIncreaseButtonClick(PointerEventData eventData)
    {
        if (GetUI<Button>("IncreaseButton").interactable == false)
        {
            return;

        }
        if (_curLevelUp < _maxLevelUp && (_targetCharacter.UnitLevel + _curLevelUp) + 1 <= MAXLEVEL)
        {
            _curLevelUp++;
            UpdateUI();
            GetUI<Slider>("LevelUpSlider").value = _curLevelUp;
        }
    }

    // UI ������ ���� ����
    private void UpdateItemText(int newValue, string itemName)
    {
        GetUI<TextMeshProUGUI>($"{itemName}Text").text = $"{itemName} : {newValue}";
        CalculateMaxLevelUp();
        UpdateUI();
    }

    private void LoadItemImage(string imageName, E_Item itemType)
    {
        string itemPath = $"UI/item_{(int)itemType}";
        Sprite itemSprite = Resources.Load<Sprite>(itemPath);
        if (itemSprite != null)
        {
            GetUI<Image>(imageName).sprite = itemSprite;
        }
        else
        {
            Debug.LogWarning($"�̹��� ã�� �� ���� {itemPath}");
        }
    }
}
