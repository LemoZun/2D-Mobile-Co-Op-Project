using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : UIBInder
{
    public GameObject characterPrefab;  // ĳ���� ĭ ������
    public Transform content;           // ��ũ�Ѻ��� content

    private void Awake()
    {
        BindAll();
        AddEvent("AllElementButton", EventType.Click, AllElementButtonClicked);
        AddEvent("FireElementButton", EventType.Click, FireElementButtonClicked);
        AddEvent("WaterElementButton", EventType.Click, WaterElementButtonClicked);
        AddEvent("GroundElementButton", EventType.Click, GroundElementButtonClicked);
        AddEvent("GrassElementButton", EventType.Click, GrassElementButtonClicked);
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

            DataSnapshot snapshot = task.Result;
            foreach (var childSnapshot in snapshot.Children)
            {
                PlayerUnitData unitData = new PlayerUnitData
                {
                    UnitId = int.Parse(childSnapshot.Child("_unitId").Value.ToString()),
                    UnitLevel = int.Parse(childSnapshot.Child("_unitLevel").Value.ToString())
                    // TODO : ���Կ� ĳ���� �̹����� ������Ұ�
                };

                GameObject slot = Instantiate(characterPrefab, content);
                CharacterSlot slotUI = slot.GetComponent<CharacterSlot>();
                slotUI.SetCharacter(unitData);
            }
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

    private void AllElementButtonClicked(PointerEventData eventData)
    {
        // ��� ĳ���� ǥ��
    }

    private void FireElementButtonClicked(PointerEventData eventData)
    {
        // �ҿ��� ĳ���� ǥ��
    }

    private void WaterElementButtonClicked(PointerEventData eventData)
    {
        // ������ ĳ���� ǥ��
    }

    private void GroundElementButtonClicked(PointerEventData eventData)
    {
        // ������ ĳ���� ǥ��
    }

    private void GrassElementButtonClicked(PointerEventData eventData)
    {
        // Ǯ���� ĳ���� ǥ��
    }
}
