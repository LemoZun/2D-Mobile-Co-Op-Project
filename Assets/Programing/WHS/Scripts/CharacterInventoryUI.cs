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
        AddCharacter(new Character { Name = "tyrano", level = 10 });
        AddCharacter(new Character { Name = "yrano", level = 50 });
        AddCharacter(new Character { Name = "rano", level = 33 });
        AddCharacter(new Character { Name = "ano", level = 44 });
        AddCharacter(new Character { Name = "no", level = 12 });
        AddCharacter(new Character { Name = "o", level = 21 });
        AddCharacter(new Character { Name = "tyrano", level = 10 });
        AddCharacter(new Character { Name = "yrano", level = 50 });
        AddCharacter(new Character { Name = "rano", level = 33 });
        AddCharacter(new Character { Name = "ano", level = 44 });
        AddCharacter(new Character { Name = "no", level = 12 });
        AddCharacter(new Character { Name = "o", level = 21 });

        PopulateGrid();
    }

    // ĳ���� �߰��ϱ�
    public void AddCharacter(Character character)
    {
        characterList.Add(character);
    }

    // �׸��忡 ���� ĳ���� ����
    public void PopulateGrid()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // ĳ���� ����
        foreach (Character character in characterList)
        {
            GameObject slot = Instantiate(characterPrefab, content);
            CharacterSlotUI slotUI = slot.GetComponent<CharacterSlotUI>();

            // ���� ĳ���� ���� ����
            slotUI.SetCharacter(character);
        }
    }

    // �������� ���� ĳ���͵� ����?

    // �������� ���� ĳ���ʹ� ������ �������� �̵��ϰ�?

    // ���������� ĳ���͸� �����ϸ� ���� ��Ͽ� �߰�?
}
