using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPanel : UIBInder
{
    private Character curCharacter;

    private void Awake()
    {
        BindAll();
        AddEvent("LevelUpButton", EventType.Click, OnLevelUpButtonClick);
        
    }

    private void Start()
    {
        UpdateCharacterInfo(curCharacter);
    }

    public void UpdateCharacterInfo(Character character)
    {
        curCharacter = character;
        GetUI<TextMeshProUGUI>("NameText").text = character.Name;
        GetUI<TextMeshProUGUI>("LevelText").text = character.level.ToString();
    }

    // ������ ��ư Ŭ��
    private void OnLevelUpButtonClick(PointerEventData eventData)
    {
        if (curCharacter != null)
        {
            if (LevelUp(curCharacter))
            {
                // UI ����
                UpdateCharacterInfo(curCharacter);
                FindObjectOfType<CharacterInventoryUI>().UpdateCharacterUI(curCharacter);
                ItemUI.instance.UpdateCurrencyUI();
            }
        }
    }

    private bool LevelUp(Character character)
    {
        int coin = 10;
        int dinoBlood = 100;
        int boneCrystal = 50;

        int requiredCoin = coin + (character.level * 20);
        int requiredDinoBlood = dinoBlood + (character.level * 10);
        int requiredBoneCrystal = 0;

        if(character.level % 5 == 0)
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
                if(requiredBoneCrystal > 0)
                {
                    Debug.Log($"�� ũ����Ż {requiredBoneCrystal} �Ҹ�");
                }
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
}
