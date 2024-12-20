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
        int requiredCoin = 100;

        if (Inventory.instance.SpendItem(ItemID.Coin, requiredCoin))
        {
            character.level++;
            Debug.Log($"{character.Name}�� ������ {character.level}�� �ö����ϴ�.");
            return true;
        }
        else
        {
            Debug.Log("������ �����մϴ�");
            return false;
        }
    }
}
