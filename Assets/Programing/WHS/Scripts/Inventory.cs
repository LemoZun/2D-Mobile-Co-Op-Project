using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum CurrencyType
{
    DinoStone,      // ��í ��ȭ ( ��, û�ּ� )
    Coin,           // ���� ��ȭ ( ���, ũ���� )
    DinoBlood,      // ĳ���� ������ ���� ��� ( ����ġ, ����, ȥ )
    BoneCrystal,    // ĳ���� Ȥ�� ��� ��ȭ ( ��ȭ��, ��ġ )
}

[System.Serializable]
public class Currency
{
    public CurrencyType type;   // ��ȭ Ÿ��
    public int amount;          // ������
    public Sprite icon;         // UI�� ǥ�õ� ������
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

    // ��ȭ �߰��ϱ�
    public void AddCurrency(CurrencyType type, int amount)
    {
        Currency currency = currencies.Find(x => x.type == type);
        if(currency != null)
        {
            // ��ȭ ������ ����
            currency.amount += amount;
            Debug.Log($"{type} {amount} �߰���. ���� {currency.amount}");
        }
    }

    // ��ȭ �Ҹ��ϱ�
    public bool SpendCurrency(CurrencyType type, int amount)
    {
        Currency currency = currencies.Find(x => x.type == type);

        if(currency != null && currency.amount >= amount)
        {
            currency.amount -= amount;
            Debug.Log($"{type} ��ȭ �Ҹ� : {amount}, �ܿ� {currency.amount}");
            return true;
        }

        return false;
    }

    // ���� ������ ���
    public int GetCurrencyAmount(CurrencyType type)
    {
        Currency currency = currencies.Find(x => x.type == type);
        return currency != null ? currency.amount : 0;
    }
}
