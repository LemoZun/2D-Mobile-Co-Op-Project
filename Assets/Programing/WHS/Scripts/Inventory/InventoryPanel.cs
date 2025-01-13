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
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private Transform _content;

    private List<PlayerUnitData> _allCharacters = new List<PlayerUnitData>();

    private Dictionary<int, Dictionary<string, string>> _characterData;

    private SceneChanger _sceneChanger;

    private DynamicGrid _dynamicGrid;

    [SerializeField] private AudioClip _bgmClip;
    [SerializeField] private AudioClip _buttonClip;

    private void Awake()
    {
        BindAll();
        AddEvent("AllElementButton", EventType.Click, AllElementButtonClicked);
        AddEvent("FireElementButton", EventType.Click, FireElementButtonClicked);
        AddEvent("WaterElementButton", EventType.Click, WaterElementButtonClicked);
        AddEvent("GroundElementButton", EventType.Click, GroundElementButtonClicked);
        AddEvent("GrassElementButton", EventType.Click, GrassElementButtonClicked);

        SoundManager.Instance.PlayeBGM(_bgmClip);
        /*
        GetUI<Button>("AllElementButton").onClick.AddListener(() => SoundManager.Instance.PlaySFX(_buttonClip));
        GetUI<Button>("FireElementButton").onClick.AddListener(() => SoundManager.Instance.PlaySFX(_buttonClip));
        GetUI<Button>("WaterElementButton").onClick.AddListener(() => SoundManager.Instance.PlaySFX(_buttonClip));
        GetUI<Button>("GroundElementButton").onClick.AddListener(() => SoundManager.Instance.PlaySFX(_buttonClip));
        GetUI<Button>("GrassElementButton").onClick.AddListener(() => SoundManager.Instance.PlaySFX(_buttonClip));
        GetUI<Button>("HomeButton").onClick.AddListener(() => SoundManager.Instance.PlaySFX(_buttonClip));
        */
        // characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];

        _sceneChanger = FindObjectOfType<SceneChanger>();

    }

    private void Start()
    {
        StartCoroutine(WaitForPlayerData());

        AddEvent("HomeButton", EventType.Click, GoLobby);
    }

    private void OnDisable()
    {
        SoundManager.Instance.StopBGM();
    }

    private IEnumerator WaitForPlayerData()
    {
        // PlayerDataManager�� �ʱ�ȭ�ǰ� PlayerData�� �ε�� ������ ���
        yield return new WaitUntil(() => PlayerDataManager.Instance != null && PlayerDataManager.Instance.PlayerData != null);
        yield return new WaitUntil(() => PlayerDataManager.Instance.PlayerData.UnitDatas != null && PlayerDataManager.Instance.PlayerData.UnitDatas.Count > 0);

        _characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];

        PopulateGrid();

        _dynamicGrid = GetComponentInChildren<DynamicGrid>();
        _dynamicGrid.SetItemCount(_allCharacters.Count);
    }

    // �׸��忡 ���� ĳ���� ����
    private void PopulateGrid()
    {
        // ��� �ʱ�ȭ
        foreach (Transform child in _content)
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

            _allCharacters.Clear();
            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                PlayerUnitData unitData = new PlayerUnitData
                {
                    UnitId = int.Parse(childSnapshot.Child("_unitId").Value.ToString()),
                    UnitLevel = int.Parse(childSnapshot.Child("_unitLevel").Value.ToString())
                };

                _allCharacters.Add(unitData);
            }

            Debug.Log($"ĳ���� {_allCharacters.Count}�� �ε�");
        });

        _allCharacters = PlayerDataManager.Instance.PlayerData.UnitDatas;
        DisplayCharacters(_allCharacters);
    }

    // �ٲ� ĳ���� ����
    public void UpdateCharacterUI(PlayerUnitData updatedCharacter)
    {
        foreach (Transform child in _content)
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
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        foreach (PlayerUnitData unitData in characters)
        {
            GameObject slot = Instantiate(_characterPrefab, _content);
            CharacterSlot slotUI = slot.GetComponent<CharacterSlot>();
            slotUI.SetCharacter(unitData);
        }
    }

    private int GetElementId(int unitId)
    {
        if (_characterData.TryGetValue(unitId, out var data))
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
        DisplayCharacters(_allCharacters);
    }

    private void FireElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("Fire");
        DisplayCharacters(_allCharacters.Where(c => GetElementId(c.UnitId) == 2).ToList());
    }

    private void WaterElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("Water");
        DisplayCharacters(_allCharacters.Where(c => GetElementId(c.UnitId) == 3).ToList());
    }

    private void GroundElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("Ground");
        DisplayCharacters(_allCharacters.Where(c => GetElementId(c.UnitId) == 1).ToList());
    }

    private void GrassElementButtonClicked(PointerEventData eventData)
    {
        Debug.Log("grass");
        DisplayCharacters(_allCharacters.Where(c => GetElementId(c.UnitId) == 4).ToList());
    }

    public int GetCharacterCount()
    {
        return _allCharacters.Count;
    }
        
    public void GoLobby(PointerEventData eventData)
    {
        _sceneChanger.CanChangeSceen = true;
        _sceneChanger.ChangeScene("Lobby_OJH");
    }
    
}
