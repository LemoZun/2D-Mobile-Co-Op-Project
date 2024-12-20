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
    private GameObject characterPanel;

    private void Awake()
    {
        BindAll();
        AddEvent("Character(Clone)", EventType.Click, OnClick);

        Transform parent = GameObject.Find("MainPanel").transform;
        characterPanel = parent.Find("CharacterPanel").gameObject;
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
    private void OnClick(PointerEventData eventData)
    {
        characterPanel.SetActive(true);
        characterPanel.GetComponent<CharacterPanel>().UpdateCharacterInfo(character);
    }

    public Character GetCharacter()
    {
        return character;
    }

}
