using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlotUI : UIBInder
{
    private Character character;

    private void Awake()
    {
        Bind();
        AddEvent("Character(Clone)", EventType.Click, OnClick);
    }

    // �κ��丮�� ĳ���� ���� - �̹���, �̸�, ���� ( ���, ������ �� )
    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;

        GetUI<TextMeshProUGUI>("NameText").text = character.Name;
        GetUI<TextMeshProUGUI>("LevelText").text = character.level.ToString();
        //GetUI<Image>("Character").sprite = character.image;
    }

    // Ŭ�� �� ( ĳ���� ���� ���, �߰� UI )
    public void OnClick(PointerEventData eventData)
    {
        Debug.Log($"{character.Name}");


    }
}
