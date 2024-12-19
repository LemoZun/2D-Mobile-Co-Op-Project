using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum CurrencyType
{
    DinoStone,      // [��ȥ��]   ��í ��ȭ ( ��, û�ּ� )
    Coin,           // [���]     ���� ��ȭ ( ���, ũ���� )
    DinoBlood,      // [����ġ]   ĳ���� ������ ���� ��� ( ����ġ, ����, ȥ )
    BoneCrystal,    // [������]   ĳ���� Ȥ�� ��� ��ȭ ( ��ȭ��, ��ġ )
}

[System.Serializable]
public class Currency
{
    public CurrencyType type;   // ��ȭ Ÿ��
    public int amount;          // ������
    public Sprite icon;         // UI�� ǥ�õ� ������
}

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
    }

    // ��ȭ ����Ʈ
    public List<Currency> currencies = new List<Currency>();

    // ĳ���� ����Ʈ
    public List<Character> characters = new List<Character>();

    // ��ȭ �߰��ϱ�, ���
    public void AddCurrency(CurrencyType type, int amount)
    {
        Currency currency = currencies.Find(x => x.type == type);
        if (currency != null)
        {
            // ��ȭ ������ ����
            currency.amount += amount;
            Debug.Log($"{type} {amount} �߰���. ���� {currency.amount}");
        }
    }

    // ��ȭ �Ҹ��ϱ�, �ұ�
    public bool SpendCurrency(CurrencyType type, int amount)
    {
        Currency currency = currencies.Find(x => x.type == type);

        if (currency != null && currency.amount >= amount)
        {
            currency.amount -= amount;
            Debug.Log($"{type} ��ȭ �Ҹ� : {amount}, �ܿ� {currency.amount}");
            return true;
        }
        Debug.Log($"��ȭ ���� {currency.amount - amount}");
        return false;
    }

    // ���� ������ ���
    public int GetCurrencyAmount(CurrencyType type)
    {
        Currency currency = currencies.Find(x => x.type == type);
        return currency != null ? currency.amount : 0;
    }

    // ��ȭ�� �����ͺ��̽��� ��� �����ؾ� �ϳ�
}
