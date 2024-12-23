using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpPanel : UIBInder
{
    private Character targetCharacter;
    private int maxLevelUp;
    private int curLevelUp;
    private const int MAXLEVEL = 30;

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
    }

    public void Initialize(Character character)
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

        if (targetCharacter.level + curLevelUp >= MAXLEVEL)
        {
            GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{targetCharacter.level} -> Lv.{MAXLEVEL} (MAX)";
        }
        else
        {
            GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{targetCharacter.level} -> Lv.{targetCharacter.level + curLevelUp}";
        }
    }

    // ������ �� �ִ�ġ
    private void CalculateMaxLevelUp()
    {
        maxLevelUp = 0;
        while (CanLevelUp(maxLevelUp + 1)
            && (targetCharacter.level + maxLevelUp + 1) <= MAXLEVEL)
        {
            maxLevelUp++;
        }
    }

    // ������ ���� ����
    private bool CanLevelUp(int level)
    {
        if (targetCharacter.level + level > MAXLEVEL)
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

        items.coin = CalculateRequiredCoin(level);
        items.dinoBlood = CalculateRequiredDinoBlood(level);
        items.boneCrystal = CalculateRequiredBoneCrystal(level);

        return items;
    }

    // ������ ���� ��ȭ �䱸�� ���
    private int CalculateRequiredCoin(int levels)
    {
        int total = 0;
        for (int i = 0; i < levels; i++)
        {
            total += 30 + (targetCharacter.level + i - 1) * 60;
        }
        return total;
    }

    // ���̳���� �䱸�� ���
    private int CalculateRequiredDinoBlood(int levels)
    {
        int total = 0;
        for (int i = 0; i < levels; i++)
        {
            total += 90 + (targetCharacter.level + i - 1) * 70;
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
            if ((targetCharacter.level + i) % 5 == 4)
            {
                total += 10 + (((targetCharacter.level + i + 1) / 5) * 40);
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
    private bool LevelUp(Character character)
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
                character.level++;
                Debug.Log($"{character.Name} ������ {character.level}");
                if (items.boneCrystal > 0)
                {
                    Debug.Log($"�� ũ����Ż {items.boneCrystal} �Ҹ�");
                }

                UpdateCharacters(character);
                ItemUI.instance.UpdateCurrencyUI();

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

    // UI ����
    private void UpdateCharacters(Character character)
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
        if (curLevelUp < maxLevelUp && (targetCharacter.level + curLevelUp) + 1 <= MAXLEVEL)
        {
            curLevelUp++;
            UpdateUI();
            GetUI<Slider>("LevelUpSlider").value = curLevelUp;
        }
    }
}
