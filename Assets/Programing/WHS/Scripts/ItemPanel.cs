using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPanel : UIBInder
{
    public static ItemPanel instance;

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

        AddEvent("BackButton", EventType.Click, ItemTEST);
    }

    // ��ȭ UI ����
    public void UpdateCurrencyUI()
    {
        GetUI<TextMeshProUGUI>("DinoStoneText").text = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone].ToString();
        GetUI<TextMeshProUGUI>("CoinText").text = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin].ToString();
        GetUI<TextMeshProUGUI>("DinoBloodText").text = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood].ToString();
        GetUI<TextMeshProUGUI>("BoneCrystalText").text = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal].ToString();
    }

    public void ItemTEST(PointerEventData eventData)
    {
        int currentCoinAmount = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin];
        PlayerDataManager.Instance.PlayerData.SetItem(GetUI<TextMeshProUGUI>("CoinText"), (int)E_Item.Coin, currentCoinAmount + 500);
        UpdateCurrencyUI();
    }
    // Ȩ ��ư -> �κ�� �̵�

    // �� ��ư -> ���� ������ �̵�?
}
