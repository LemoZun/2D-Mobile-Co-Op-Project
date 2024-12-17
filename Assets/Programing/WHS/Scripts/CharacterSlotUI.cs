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

    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
        iconImage.sprite = character.image;
        nameText.text = $"{character.name}";
        levelText.text = $"{character.level}";
    }

    public void OnClick()
    {
        Debug.Log($"{character.name}");
    }
}
