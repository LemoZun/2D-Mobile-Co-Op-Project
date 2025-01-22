using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private int itemId;
    public int ItemId { get { return itemId; } set { itemId = value; } }
    private string itemName;
    public string ItemName { get { return itemName; } set { itemName = value; } }
    private int amount;
    public int Amount { get { return amount; } set { amount = value; } }
    private Sprite itemImage;
    public Sprite ItemImage { get { return itemImage; } set { itemImage = value; } }

    /// <summary>
    /// ItemList�� Dictionary�� ������ �� ���
    /// Item�� ������ �߰��Ǵ� ��� switch���� �߰��Ͽ� ��� ����
    // - GachaSceneController.cs�� MakeItemList()���� �����Ͽ� ���
    // - �� �̹��� ������ Resources.Load<Sprite>("���/�����̸�")���� ���� ���� �ʿ�
    /// </summary>
    /// <param name="dataBaseList"></param>
    /// <param name="result"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item MakeItemList(Dictionary<int, Dictionary<string, string>> dataBaseList, Item result, int index)
    {
        switch (index)
        {
            case 500:
                result.itemId = index;
                result.itemName = dataBaseList[index]["ItemName"];
                result.itemImage = Resources.Load<Sprite>("ShopTest/Gold");
                break;
            case 501:
                result.itemId = index;
                result.itemName = dataBaseList[index]["ItemName"];
                result.itemImage = Resources.Load<Sprite>("ShopTest/DinoBlood");
                break;
            case 502:
                result.itemId = index;
                result.itemName = dataBaseList[index]["ItemName"];
                result.itemImage = Resources.Load<Sprite>("ShopTest/BoneCrystal");
                break;
            case 503:
                result.itemId = index;
                result.itemName = dataBaseList[index]["ItemName"];
                result.itemImage = Resources.Load<Sprite>("ShopTest/DinoStone");
                break;
            case 504:
                result.itemId = index;
                result.itemName = dataBaseList[index]["ItemName"];
                result.itemImage = Resources.Load<Sprite>("ShopTest/Stone");
                break;
        }
        return result;
    }

    /// <summary>
    /// GachaItem�� ������ ResultPanel/Panel �Ʒ��� ���� ������� ������UI�� �����ϴ� �Լ�
    // - GachaSceneController.cs���� ���
    /// </summary>
    /// <param name="gachaItem"></param>
    /// <param name="resultUI"></param>
    /// <returns></returns>
    public GameObject SetGachaItemUI(Item gachaItem, GameObject resultUI)
    {
    resultUI.GetComponent<Item>().itemId = gachaItem.itemId;
    resultUI.GetComponent<Item>().itemName = gachaItem.itemName;
    resultUI.GetComponent<Item>().amount = gachaItem.amount;

        // �˸��� UI ���
        resultUI.transform.GetChild(0).GetComponent<Image>().sprite = gachaItem.itemImage;
        resultUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gachaItem.itemName;
        resultUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = gachaItem.amount.ToString();
        return resultUI;
    }

    /// <summary>
    /// �̹� ������ Character�� ���� ���
    /// ��ȯ�Ǵ� �������� �����ִ� UI Setting �Լ�
    /// </summary>
    /// <param name="gachaItem"></param>
    /// <param name="resultUI"></param>
    /// <returns></returns>
    public GameObject SetGachaReturnItemUI(Item gachaItem, GameObject resultUI)
    {
        resultUI.GetComponent<Item>().ItemId = gachaItem.ItemId;
        resultUI.GetComponent<Item>().Amount = gachaItem.Amount;

        // �˸��� UI ���
        resultUI.transform.GetChild(1).GetComponent<Image>().sprite = gachaItem.ItemImage;
        resultUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gachaItem.Amount.ToString();
        return resultUI;
    }
}
