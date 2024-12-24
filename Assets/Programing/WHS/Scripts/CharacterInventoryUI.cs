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

    private List<PlayerUnitData> characterList = new List<PlayerUnitData>();

    private void Start()
    {
        StartCoroutine(WaitForFirebaseInitialization());
    }

    // �α����� �����ؼ� �����ͺ��̽� ���� ����Ʈ��
    private IEnumerator WaitForFirebaseInitialization()
    {
        while (BackendManager.Database == null || BackendManager.Database.RootReference == null)
        {
            yield return null;
        }
        LoadCharacter();
    }

    // ���̾�̽����� ĳ���� �޾ƿ���
    private void LoadCharacter()
    {
        // �α����� ������ �ӽ÷� userID ����
        string userID = "poZb90DRTiczkoC5TpHOpaJ5AXR2";
        DatabaseReference root = BackendManager.Database.RootReference.Child("UserData").Child(userID).Child("_unitDatas");

        root.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("ĳ���� ������ �ε� ��ҵ�");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ĳ���� ������ �ε��� ���� �߻� " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            characterList.Clear();

            foreach(DataSnapshot childSnapshot in snapshot.Children)
            {
                string json = childSnapshot.GetRawJsonValue();
                PlayerUnitData character = JsonUtility.FromJson<PlayerUnitData>(json);
                characterList.Add(character);
            }

            PopulateGrid();

        });
    }

    // �׸��忡 ���� ĳ���� ����
    private void PopulateGrid()
    {
        // ��� �ʱ�ȭ
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // ��ũ�Ѻ信 ĳ���� ����
        foreach (PlayerUnitData unitData in characterList)
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
            if (slotUI.GetCharacter().Name == updatedCharacter.Name)
            {
                slotUI.SetCharacter(updatedCharacter);
                break;
            }
        }
    }
}
