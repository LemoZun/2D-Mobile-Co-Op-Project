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
        int requiredCoin = CalculateRequiredItem(10, 20, curLevelUp);
        int requiredDinoBlood = CalculateRequiredItem(100, 10, curLevelUp);
        int requiredBoneCrystal = CalculateRequiredBoneCrystal(curLevelUp);

        GetUI<TextMeshProUGUI>("CoinText").text = $"Coin : {requiredCoin}";
        GetUI<TextMeshProUGUI>("DinoBloodText").text = $"DinoBlood : {requiredDinoBlood}";
        GetUI<TextMeshProUGUI>("BoneCrystalText").text = $"BoneCrystal : {requiredBoneCrystal}";
        GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{targetCharacter.level} -> Lv.{targetCharacter.level + curLevelUp}";
    }

    // ������ �� �ִ�ġ
    private void CalculateMaxLevelUp()
    {
        maxLevelUp = 0;
        while (CanLevelUp(maxLevelUp + 1))
        {
            maxLevelUp++;
        }
    }

    // ������ ���� ����
    private bool CanLevelUp(int level)
    {
        // ������ ���� ��ȭ �䱸��
        int requiredCoin = CalculateRequiredItem(10, 20, level);
        int requiredDinoBlood = CalculateRequiredItem(100, 10, level);
        int requiredBoneCrystal = CalculateRequiredBoneCrystal(level);

        // �������� ����� ���� ������ ������ true
        return Inventory.instance.GetItemAmount(ItemID.Coin) >= requiredCoin &&
               Inventory.instance.GetItemAmount(ItemID.DinoBlood) >= requiredDinoBlood &&
               Inventory.instance.GetItemAmount(ItemID.BoneCrystal) >= requiredBoneCrystal;
    }

    // ������ ���� ��ȭ �䱸�� ���
    private int CalculateRequiredItem(int baseAmount, int increasePerLevel, int level)
    {
        int total = 0;
        
        // ������ ���� �䱸�� ���
        for (int i = 0; i < level; i++)
        {
            total += baseAmount + ((targetCharacter.level + i) * increasePerLevel);
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
            if ((targetCharacter.level + i) % 5 == 0)
            {
                total += 50 + (((targetCharacter.level + i) / 5) * 50);
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
        int coin = 10;
        int dinoBlood = 100;
        int boneCrystal = 50;

        int requiredCoin = coin + (character.level * 20);
        int requiredDinoBlood = dinoBlood + (character.level * 10);
        int requiredBoneCrystal = 0;

        if (character.level % 5 == 0)
        {
            requiredBoneCrystal = boneCrystal + ((character.level / 5) * 50);
        }

        if (Inventory.instance.GetItemAmount(ItemID.Coin) >= requiredCoin &&
        Inventory.instance.GetItemAmount(ItemID.DinoBlood) >= requiredDinoBlood &&
        Inventory.instance.GetItemAmount(ItemID.BoneCrystal) >= requiredBoneCrystal)
        {
            if (Inventory.instance.SpendItem(ItemID.Coin, requiredCoin) &&
                Inventory.instance.SpendItem(ItemID.DinoBlood, requiredDinoBlood) &&
                (requiredBoneCrystal == 0 || Inventory.instance.SpendItem(ItemID.BoneCrystal, requiredBoneCrystal)))
            {
                character.level++;
                Debug.Log($"{character.Name} ������ {character.level}");
                if (requiredBoneCrystal > 0)
                {
                    Debug.Log($"�� ũ����Ż {requiredBoneCrystal} �Ҹ�");
                }

                UpdateCharacters(character);
                ItemUI.instance.UpdateCurrencyUI();

                return true;
            }
        }

        if (Inventory.instance.GetItemAmount(ItemID.Coin) < requiredCoin)
        {
            Debug.Log($"������ {Inventory.instance.GetItemAmount(ItemID.Coin) - requiredCoin} �����մϴ�.");
        }
        if (Inventory.instance.GetItemAmount(ItemID.DinoBlood) < requiredDinoBlood)
        {
            Debug.Log($"���̳���尡 {Inventory.instance.GetItemAmount(ItemID.DinoBlood) - requiredDinoBlood} �����մϴ�");
        }
        if (Inventory.instance.GetItemAmount(ItemID.BoneCrystal) < requiredDinoBlood)
        {
            Debug.Log($"��ũ����Ż�� {Inventory.instance.GetItemAmount(ItemID.BoneCrystal) - requiredBoneCrystal} �����մϴ�");
        }

        return false;
    }

    // UI ����
    private void UpdateCharacters(Character character)
    {
        CharacterPanel characterPanel = FindObjectOfType<CharacterPanel>();
        if(characterPanel != null)
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
        if (curLevelUp < maxLevelUp)
        {
            curLevelUp++;
            UpdateUI();
            GetUI<Slider>("LevelUpSlider").value = curLevelUp;
        }
    }
}
