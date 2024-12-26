using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

/*
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


    // ��ȭ �ʱ�ȭ ( �������� �޾ƿ;� �� ��)
    private void InitInventory()
    {
        items = new Dictionary<int, Item>();

        AddItem(ItemID.Coin, 1000000);
        AddItem(ItemID.DinoBlood, 1000000);
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
            Item newItem = new Item { ItemID = itemID, amount = amount };
            items[itemID] = newItem;
        }
        // Debug.Log($"{itemID}�� {amount} �߰�, {items[itemID].amount}");
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
*/