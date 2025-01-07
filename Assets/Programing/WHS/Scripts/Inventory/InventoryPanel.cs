using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : UIBInder
{
    public GameObject characterPrefab;
    public Transform content;

    private List<PlayerUnitData> allCharacters = new List<PlayerUnitData>();

    private Dictionary<int, Dictionary<string, string>> characterData;

    private void Awake()
    {
        BindAll();
        AddEvent("AllElementButton", EventType.Click, AllElementButtonClicked);
        AddEvent("FireElementButton", EventType.Click, FireElementButtonClicked);
        AddEvent("WaterElementButton", EventType.Click, WaterElementButtonClicked);
        AddEvent("GroundElementButton", EventType.Click, GroundElementButtonClicked);
        AddEvent("GrassElementButton", EventType.Click, GrassElementButtonClicked);

        // characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];
    }

    private void Start()
    {
        StartCoroutine(WaitForPlayerData());
    }

    private IEnumerator WaitForPlayerData()
    {
        // PlayerDataManager�� �ʱ�ȭ�ǰ� PlayerData�� �ε�� ������ ���
        yield return new WaitUntil(() => PlayerDataManager.Instance.PlayerData.UnitDatas.Count > 0);

        PopulateGrid();

        characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];
    }

    // �׸��忡 ���� ĳ���� ����
    private void PopulateGrid()
    {
        // ��� �ʱ�ȭ
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Firebase���� UnitDatas�� ������ ��ũ�Ѻ信 ĳ���� ����
        string userID = BackendManager.Auth.CurrentUser.UserId;

        DatabaseReference unitDatasRef = BackendManager.Database.RootReference
            .Child("UserData").Child(userID).Child("_unitDatas");

        unitDatasRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("���� ������ �ε� �� ���� �߻�: " + task.Exception);
                return;
            }

            allCharacters.Clear();
            DataSnapshot snapshot = task.Result;
            foreach (var childSnapshot in snapshot.Children)
            {
                PlayerUnitData unitData = new PlayerUnitData
                {
                    UnitId = int.Parse(childSnapshot.Child("_unitId").Value.ToString()),
                    UnitLevel = int.Parse(childSnapshot.Child("_unitLevel").Value.ToString())
                };

                allCharacters.Add(unitData);
            }

            DisplayCharacters(allCharacters);
            Debug.Log($"ĳ���� {allCharacters.Count}��");
        });
    }

    // �ٲ� ĳ���� ����
    public void UpdateCharacterUI(PlayerUnitData updatedCharacter)
    {
        foreach (Transform child in content)
        {
            CharacterSlot slotUI = child.GetComponent<CharacterSlot>();
            if (slotUI.GetCharacter().UnitId == updatedCharacter.UnitId)
            {
                slotUI.SetCharacter(updatedCharacter);
                break;
            }
        }
    }

    // ������ ĳ���� �����ֱ�
    private void DisplayCharacters(List<PlayerUnitData> characters)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var unitData in characters)
        {
            GameObject slot = Instantiate(characterPrefab, content);
            CharacterSlot slotUI = slot.GetComponent<CharacterSlot>();
            slotUI.SetCharacter(unitData);
        }
    }

    private int GetElementId(int unitId)
    {
        if (characterData.TryGetValue(unitId, out var data))
        {
            int elementID = int.Parse(data["ElementID"]);
            Debug.Log($"UnitID: {unitId}, ElementID: {elementID}");
            return elementID;
        }
        Debug.LogWarning($"No data found for UnitID: {unitId}");
        return 0;
    }

    // ElementID�� ���� ���� 1�� 2�� 3�� 4Ǯ
    private void AllElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("All");
        DisplayCharacters(allCharacters);
    }

    private void FireElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("Fire");
        DisplayCharacters(allCharacters.Where(c => GetElementId(c.UnitId) == 2).ToList());
    }

    private void WaterElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("Water");
        DisplayCharacters(allCharacters.Where(c => GetElementId(c.UnitId) == 3).ToList());
    }

    private void GroundElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("Ground");
        DisplayCharacters(allCharacters.Where(c => GetElementId(c.UnitId) == 1).ToList());
    }

    private void GrassElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("grass");
        DisplayCharacters(allCharacters.Where(c => GetElementId(c.UnitId) == 4).ToList());
    }
}
