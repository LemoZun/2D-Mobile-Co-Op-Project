using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventoryUI : MonoBehaviour
{
    public GameObject characterPrefab;  // ĳ���� ĭ ������
    public Transform content;           // ��ũ�Ѻ��� content

    private List<Character> characterList = new List<Character>();

    private void Start()
    {
        // ĳ���ʹ� �����ͺ��̽����� �޾ƿͼ� ����,  ��ũ�Ѻ� Ȯ�ο�
        AddCharacter(new Character { ID = 10, Name = "tyrano", level = 10 });
        AddCharacter(new Character { ID = 11, Name = "yrano", level = 50 });
        AddCharacter(new Character { ID = 12, Name = "rano", level = 33 });
        AddCharacter(new Character { ID = 13, Name = "ano", level = 44 });
        AddCharacter(new Character { ID = 14, Name = "no", level = 12 });
        AddCharacter(new Character { ID = 15, Name = "o", level = 21 });
        AddCharacter(new Character { ID = 16, Name = "tyrano", level = 10 });
        AddCharacter(new Character { ID = 17, Name = "yrano", level = 50 });
        AddCharacter(new Character { ID = 18, Name = "rano", level = 33 });
        AddCharacter(new Character { ID = 19, Name = "ano", level = 44 });
        AddCharacter(new Character { ID = 20, Name = "no", level = 12 });
        AddCharacter(new Character { ID = 21, Name = "o", level = 21 });

        PopulateGrid();
    }

    // ĳ���� �߰��ϱ�
    private void AddCharacter(Character character)
    {
        characterList.Add(character);
    }

    // �׸��忡 ���� ĳ���� ����
    private void PopulateGrid()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // ��ũ�Ѻ信 ĳ���� ����
        foreach (Character character in characterList)
        {
            GameObject slot = Instantiate(characterPrefab, content);
            CharacterSlotUI slotUI = slot.GetComponent<CharacterSlotUI>();

            // ���� ĳ���� ���� ����
            slotUI.SetCharacter(character);
        }
    }

    // �ٲ� ĳ���� ����
    public void UpdateCharacterUI(Character updatedCharacter)
    {
        foreach (Transform child in content)
        {
            CharacterSlotUI slotUI = child.GetComponent<CharacterSlotUI>();
            if (slotUI.GetCharacter().ID == updatedCharacter.ID)
            {
                slotUI.SetCharacter(updatedCharacter);
                break;
            }
        }
    }
}
