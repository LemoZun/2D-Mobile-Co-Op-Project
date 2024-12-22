using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : UIBInder
{
    public static ItemUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    private void Start()
    {
        BindAll();
        UpdateCurrencyUI();

        AddEvent("BackButton", EventType.Click, AddItem);
    }

    // ��ȭ UI ����
    public void UpdateCurrencyUI()
    {
        GetUI<TextMeshProUGUI>("DinoStoneText").text = Inventory.instance.GetItemAmount(ItemID.DinoStone).ToString();
        GetUI<TextMeshProUGUI>("CoinText").text = Inventory.instance.GetItemAmount(ItemID.Coin).ToString();
        GetUI<TextMeshProUGUI>("DinoBloodText").text = Inventory.instance.GetItemAmount(ItemID.DinoBlood).ToString();
        GetUI<TextMeshProUGUI>("BoneCrystalText").text = Inventory.instance.GetItemAmount(ItemID.BoneCrystal).ToString();
    }

    public void AddItem(PointerEventData eventData)
    {
        Inventory.instance.AddItem(ItemID.Coin, 500);
        UpdateCurrencyUI();
    }
    // Ȩ ��ư -> �κ�� �̵�

    // �� ��ư -> ���� ������ �̵�?
}
