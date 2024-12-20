using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable] // ĳ���� ���� ( �����ͺ��̽����� �޾ƿð�)
public class Character
{
    public int ID;
    public string Name;
    public string Dinosaur;
    public string Job;
    public string Element;
    public string Skills;
    public string SkillsDiscription;
    public int Rare;

    public int level;
    public Sprite image;
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

        InitInventory();
    }

    // ������ ��ųʸ�
    public Dictionary<int, Item> items = new Dictionary<int, Item>();

    // ĳ���� ����Ʈ
    public List<Character> characters = new List<Character>();


    // ��ȭ �ʱ�ȭ ( �������� �޾ƿ;� �� ��)
    private void InitInventory()
    {
        items = new Dictionary<int, Item>();

        AddItem(ItemID.Coin, 100000);
        AddItem(ItemID.DinoBlood, 50000);
        AddItem(ItemID.BoneCrystal, 2500);
        AddItem(ItemID.DinoStone, 30000);
        AddItem(ItemID.LeapStone, 300);
        AddItem(ItemID.Gear, 100);
    }

    // ��ȭ �߰��ϱ�, ���
    public void AddItem(int itemID, int amount)
    {
        if (items.TryGetValue(itemID, out Item item))
        {
            item.amount += amount;
        }
        else
        {
            Item newItem = new Item { id = itemID, amount = amount };
            items[itemID] = newItem;
        }
        Debug.Log($"{itemID}�� {amount} �߰�, {items[itemID].amount}");
    }

    // ��ȭ �Ҹ��ϱ�, �ұ�
    public bool SpendItem(int itemId, int amount)
    {
        if(items == null || !items.ContainsKey(itemId))
        {
            Debug.Log("�������� ����");
            return false;
        }

        if (items.TryGetValue(itemId, out Item item) && item.amount >= amount)
        {
            item.amount -= amount;
            Debug.Log($"{itemId} �Ҹ� : {amount}, �ܿ� {item.amount}");

            return true;
        }

        Debug.Log($"������ ID {itemId}�� {amount - item.amount} ����");
        return false;
    }

    // ���� ������ ���
    public int GetItemAmount(int itemId)
    {
        return items.TryGetValue(itemId, out Item item) ? item.amount : 0;
    }

    // ��ȭ�� �����ͺ��̽��� ��� �����ؾ� �ϳ�
}
