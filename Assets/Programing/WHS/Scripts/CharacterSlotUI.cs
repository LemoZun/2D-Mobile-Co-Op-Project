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
    private PlayerUnitData unitData;
    private GameObject characterPanel;

    private void Awake()
    {
        BindAll();
        AddEvent("Character(Clone)", EventType.Click, OnClick);

        Transform parent = GameObject.Find("MainPanel").transform;
        characterPanel = parent.Find("CharacterPanel").gameObject;
    }

    // �κ��丮�� ĳ���� ���� - �̹���, �̸�, ���� ( ���, ������ �� )
    public void SetCharacter(PlayerUnitData newUnitData)
    {
        unitData = newUnitData;

        GetUI<TextMeshProUGUI>("NameText").text = unitData.Name;
        GetUI<TextMeshProUGUI>("LevelText").text = unitData.UnitLevel.ToString();
        //GetUI<Image>("Character").sprite = character.image;
    }

    // Ŭ�� �� ( ĳ���� ���� ���, �߰� UI )
    private void OnClick(PointerEventData eventData)
    {
        characterPanel.SetActive(true);

        characterPanel.GetComponent<CharacterPanel>().UpdateCharacterInfo(unitData);
    }

    public PlayerUnitData GetCharacter()
    {
        return unitData;
    }

}
