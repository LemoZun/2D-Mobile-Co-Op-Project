using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUp
{
    public const int MAXLEVEL = 30;
    private Dictionary<int, Dictionary<string, string>> _levelUpData;
    private Dictionary<int, Dictionary<string, string>> _characterData;


    public LevelUp(Dictionary<int, Dictionary<string, string>> levelUpData, Dictionary<int, Dictionary<string, string>> characterData)
    {
        _levelUpData = levelUpData;
        _characterData = characterData;
    }

    // ������ ���� ����
    public bool CanLevelUp(PlayerUnitData character, int levels)
    {
        if (character.UnitLevel + levels > MAXLEVEL)
        {
            return false;
        }

        RequiredItems items = CalculateRequiredItems(character, levels);

        return PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] >= items.Coin &&
               PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] >= items.DinoBlood &&
               PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] >= items.BoneCrystal;
    }

    // �������� �ʿ��� ������ �䱸�� ���
    public RequiredItems CalculateRequiredItems(PlayerUnitData character, int levels)
    {
        RequiredItems items = new RequiredItems();
        int rarity = GetRarity(character.UnitId);

        for (int i = 0; i < levels; i++)
        {
            int curLevel = character.UnitLevel + i + 1;
            int levelUpId = FindLevelUpId(rarity, curLevel);

            if (_levelUpData.TryGetValue(levelUpId, out Dictionary<string, string> data))
            {
                if (int.TryParse(data["500"], out int coin))
                {
                    items.Coin += coin;
                }
                if (int.TryParse(data["501"], out int dinoBlood))
                {
                    items.DinoBlood += dinoBlood;
                }
                if (data.ContainsKey("502") && int.TryParse(data["502"], out int boneCrystal))
                {
                    items.BoneCrystal += boneCrystal;
                }
            }
            else
            {
                Debug.LogError($"������ �����͸� ã�� �� �����ϴ�. LevelUpID: {levelUpId}");
                return new RequiredItems();
            }
        }

        return items;
    }

    // ������ ����
    public void PerformLevelUp(PlayerUnitData character, int levels)
    {
        RequiredItems items = CalculateRequiredItems(character, levels);

        // PlayerDataManager���� ������ ����
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Coin, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin] - items.Coin);
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.DinoBlood, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood] - items.DinoBlood);
        PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.BoneCrystal, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal] - items.BoneCrystal);

        // ĳ������ ���� ����
        character.UnitLevel += levels;

        // DB�� ����, ������ ����
        UpdateDatabase(character, items);
    }

    // ĳ���� csv�����Ϳ��� ��� ��ȯ
    private int GetRarity(int unitId)
    {
        return int.Parse(_characterData[unitId]["Rarity"]);
    }

    // ������ csv�����Ϳ��� ����� ������ ���� ID ��ȯ
    private int FindLevelUpId(int rarity, int level)
    {
        return _levelUpData.First(entry =>
            int.Parse(entry.Value["Rarity"]) == rarity &&
            int.Parse(entry.Value["Level"]) == level).Key;
    }

    // DB�� ������ ������ ����
    private void UpdateDatabase(PlayerUnitData character, RequiredItems items)
    {
        string userId = BackendManager.Auth.CurrentUser.UserId;
        DatabaseReference userRef = BackendManager.Database.RootReference.Child("UserData").Child(userId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            ["_items/0"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin],
            ["_items/1"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood],
            ["_items/2"] = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal],
            [$"_unitDatas/{character.UnitId}/_unitLevel"] = character.UnitLevel
        };

        userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"�����ͺ��̽� ������Ʈ ����: {task.Exception}");
            }
            else
            {
                Debug.Log("�����ͺ��̽� ������Ʈ ����");
            }
        });
    }
}