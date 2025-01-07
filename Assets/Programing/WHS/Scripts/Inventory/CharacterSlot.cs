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

        if (int.TryParse(data["Rarity"], out int rarity))
        {
            Debug.Log(rarity);
            UpdateStar(rarity);
        }

        // ���� �Ӽ� �̹��� ����
        if (int.TryParse(data["ElementID"], out int elementId))
        {
            string elementPath = $"UI/element_{elementId}";
            Sprite elementSprite = Resources.Load<Sprite>(elementPath);
            if (elementSprite != null)
            {
                GetUI<Image>("ElementImage").sprite = elementSprite;
            }
            else
            {
                Debug.LogWarning($"�̹����� ã�� �� ����: {elementPath}");
            }
        }

        string portraitPath = $"Portrait/portrait_{unitData.UnitId}";
        if (portraitPath != null)
        {
            GetUI<Image>("Character(Clone)").sprite = Resources.Load<Sprite>(portraitPath);
        }
        else
        {
            Debug.Log($"�̹����� ã�� �� ���� {portraitPath}");
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

    private void UpdateStar(int rarity)
    {
        // rarity�� ���� �� ������ ���
        for (int i = 0; i < 5; i++)
        {
            Image starImage = GetUI<Image>($"Star_{i + 1}");
            if (starImage != null)
            {
                if (i < rarity)
                {
                    starImage.gameObject.SetActive(true);
                    starImage.sprite = Resources.Load<Sprite>("UI/icon_star");
                }
                else
                {
                    starImage.gameObject.SetActive(false);
                }
            }
        }
    }
}
