using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : UIBInder
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

        Dictionary<int, Dictionary<string, string>> characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];
        if (characterData.TryGetValue(unitData.UnitId, out var data))
        {
            GetUI<TextMeshProUGUI>("NameText").text = data["Name"];
        }
        else
        {
            GetUI<TextMeshProUGUI>("NameText").text = unitData.UnitId.ToString();
        }

        GetUI<TextMeshProUGUI>("LevelText").text = unitData.UnitLevel.ToString();

        // TODO : ĳ���� ��������Ʈ �̹���
        string path = $"Portrait/portrait_{unitData.UnitId}";
        if(path != null)
        {
            GetUI<Image>("Character(Clone)").sprite = Resources.Load<Sprite>(path);
        }
        else
        {
            Debug.Log($"�̹����� ã�� �� ���� {path}");
        }
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
