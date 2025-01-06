using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterPanel : UIBInder
{
    private PlayerUnitData curCharacter;
    private GameObject levelUpPanel;

    private Dictionary<int, Dictionary<string, string>> characterData;

    private SceneChanger _sceneChanger;

    private int index;
    private List<PlayerUnitData> characterList;

    private void Awake()
    {
        BindAll();
        AddEvent("LevelUpButton", EventType.Click, OnLevelUpButtonClick);
        AddEvent("HomeButton", EventType.Click, GoLobby);
        AddEvent("PreviousCharacterButton", EventType.Click, PreviousButton);
        AddEvent("NextCharacterButton", EventType.Click, NextButton);

        Transform parent = GameObject.Find("CharacterPanel").transform;
        levelUpPanel = parent.Find("LevelUpPanel").gameObject;

        characterData = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];

        _sceneChanger = FindObjectOfType<SceneChanger>();

        characterList = PlayerDataManager.Instance.PlayerData.UnitDatas;
    }

    private void Start()
    {
        UpdateCharacterInfo(curCharacter);
    }

    // ĳ���� ���� ����
    public void UpdateCharacterInfo(PlayerUnitData character)
    {
        curCharacter = character;
        index = characterList.FindIndex(c => c.UnitId == character.UnitId);
        Debug.Log($"{index} ���� �ε���");

        int level = character.UnitLevel;
        if (characterData.TryGetValue(character.UnitId, out var data))
        {
            // TODO : ĳ������ ���� ���� ���� ( ������ ���� ����, �̹��� )

            // ĳ���� �̹���
            string path = $"Portrait/portrait_{character.UnitId}";
            if (path != null)
            {
                GetUI<Image>("CharacterImage").sprite = Resources.Load<Sprite>(path);
            }

            // ����, �̸�
            // GetUI<TextMeshProUGUI>("LevelText").text = character.UnitLevel.ToString();
            GetUI<TextMeshProUGUI>("NameText").text = data["Name"];

            // TODO : �Ӽ� ������ �̹��� 
            GetUI<Image>("ElementImage").sprite = null;

            // ����� ���� �� ���� ~5�� ���
            if (int.TryParse(data["Rarity"], out int rarity))
            {
                UpdateStar(rarity);
            }

            // TODO : ��ų ���� ��������
            GetUI<TextMeshProUGUI>("SkillNameText").text = "��ų �̸�";
            GetUI<TextMeshProUGUI>("CoolDownText").text = "��Ÿ��";
            GetUI<TextMeshProUGUI>("SkillDescriptionText").text = "������ â�� ���� ���� ���ظ� �����ϴ�";

            // TODO : ������ ���� ������ ����
            GetUI<TextMeshProUGUI>("HPText").text = "HP : " + CalculateStat(TypeCastManager.Instance.TryParseInt(data["BaseHp"]), level);
            GetUI<TextMeshProUGUI>("AttackText").text = "Atk : " + CalculateStat(int.Parse(data["BaseATK"]), level);
            GetUI<TextMeshProUGUI>("DefText").text = "Def : " + CalculateStat(int.Parse(data["BaseDef"]), level);

            GetUI<Button>("LevelUpButton").interactable = (character.UnitLevel < 30);

            // db�� ĳ���� ���� ����
            UpdateCharacterData(character);
        }
    }

    private void UpdateCharacterData(PlayerUnitData character)
    {
        string userID = BackendManager.Auth.CurrentUser.UserId;
        DatabaseReference characterRef = BackendManager.Database.RootReference
            .Child("UserData").Child(userID).Child("_unitDatas");

        characterRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("ĳ���� ������ �ε� �� ���� �߻�: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            foreach (var childSnapshot in snapshot.Children)
            {
                if (int.Parse(childSnapshot.Child("_unitId").Value.ToString()) == character.UnitId)
                {
                    Dictionary<string, object> updates = new Dictionary<string, object>
                    {
                        ["_unitLevel"] = character.UnitLevel,
                    };

                    childSnapshot.Reference.UpdateChildrenAsync(updates).ContinueWithOnMainThread(updateTask =>
                    {
                        if (updateTask.IsCompleted)
                        {
                            Debug.Log($"ĳ���� ID {character.UnitId}�� �����Ͱ� ������Ʈ��");
                        }
                        else
                        {
                            Debug.LogError($"ĳ���� ID {character.UnitId} ������Ʈ ����: " + updateTask.Exception);
                        }
                    });
                    break;
                }
            }
        });
    }

    private void OnLevelUpButtonClick(PointerEventData eventData)
    {
        if (curCharacter != null && curCharacter.UnitLevel < 30)
        {
            levelUpPanel.gameObject.SetActive(true);

            LevelUpPanel levelUp = levelUpPanel.GetComponent<LevelUpPanel>();
            levelUp.Init(curCharacter);
        }
    }

    private int CalculateStat(int stat, int level)
    {
        // TODO : ������ ���� ���� ���
        return stat;
    }

    private void PreviousButton(PointerEventData eventData)
    {
        // ���� ĳ���� ������ �̵�
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = characterList.Count - 1;
        }

        UpdateCharacterInfo(characterList[index]);
    }

    private void NextButton(PointerEventData eventData)
    {
        // ���� ĳ���� ������ �̵�
        if (index < characterList.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        UpdateCharacterInfo(characterList[index]);
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

    private void GoLobby(PointerEventData eventData)
    {
        _sceneChanger.CanChangeSceen = true;
        _sceneChanger.ChangeScene("Lobby_OJH");
    }
}
