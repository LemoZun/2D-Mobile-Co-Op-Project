using Firebase.Database;
using Firebase.Extensions;
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

    // private List<PlayerUnitData> characterList = new List<PlayerUnitData>();

    private void Start()
    {
        StartCoroutine(WaitForPlayerData());
    }

    private IEnumerator WaitForPlayerData()
    {
        // PlayerDataManager�� �ʱ�ȭ�ǰ� PlayerData�� �ε�� ������ ���
        yield return new WaitUntil(() =>
            PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.PlayerData != null &&
            PlayerDataManager.Instance.PlayerData.UnitDatas != null &&
            PlayerDataManager.Instance.PlayerData.UnitDatas.Count > 0);

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

        // PlayerDataManager���� UnitDatas�� ������ ��ũ�Ѻ信 ĳ���� ����
        foreach (PlayerUnitData unitData in PlayerDataManager.Instance.PlayerData.UnitDatas)
        {
            GameObject slot = Instantiate(characterPrefab, content);
            CharacterSlotUI slotUI = slot.GetComponent<CharacterSlotUI>();

            // ���� ĳ���� ���� ����
            slotUI.SetCharacter(unitData);
        }
    }

    // �ٲ� ĳ���� ����
    public void UpdateCharacterUI(PlayerUnitData updatedCharacter)
    {
        foreach (Transform child in content)
        {
            CharacterSlotUI slotUI = child.GetComponent<CharacterSlotUI>();
            if (slotUI.GetCharacter().UnitId == updatedCharacter.UnitId)
            {
                slotUI.SetCharacter(updatedCharacter);
                break;
            }
        }
    }    
}
