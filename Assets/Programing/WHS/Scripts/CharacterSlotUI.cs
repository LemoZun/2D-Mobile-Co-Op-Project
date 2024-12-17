using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;    // ĳ���� �̸� �ؽ�Ʈ
    public TextMeshProUGUI levelText;   // ĳ���� �̸� �ؽ�Ʈ
    public Image iconImage;             // ĳ���� ������ �̹���

    private Character character;

    // �κ��丮�� ĳ���� ���� - �̹���, �̸�, ���� ( ���, ������ �� )
    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
        iconImage.sprite = character.image;
        nameText.text = $"{character.Name}";
        levelText.text = $"{character.level}";
    }

    // Ŭ�� �� ( ĳ���� ���� ���, �߰� UI )
    public void OnClick()
    {
        Debug.Log($"{character.Name}");
        Debug.Log($"{character.ID}");
    }
}
