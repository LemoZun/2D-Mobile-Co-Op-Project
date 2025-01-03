using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaItem : MonoBehaviour
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
    /// </summary>
    /// <param name="dataBaseList"></param>
    /// <param name="result"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public GachaItem MakeItemList(Dictionary<int, Dictionary<string, string>> dataBaseList, GachaItem result, int index)
    {
        switch (index)
        {
            case 500:
                result.ItemId = index;
                result.ItemName = dataBaseList[index]["ItemName"];
                result.ItemImage = Resources.Load<Sprite>("Lottery/TestG");
                break;
            case 501:
                result.ItemId = index;
                result.ItemName = dataBaseList[index]["ItemName"];
                result.ItemImage = Resources.Load<Sprite>("Lottery/TestDB");
                break;
            case 502:
                result.ItemId = index;
                result.ItemName = dataBaseList[index]["ItemName"];
                result.ItemImage = Resources.Load<Sprite>("Lottery/TestBC");
                break;
            case 503:
                result.ItemId = index;
                result.ItemName = dataBaseList[index]["ItemName"];
                result.ItemImage = Resources.Load<Sprite>("Lottery/TestDS");
                break;
            case 504:
                result.ItemId = index;
                result.ItemName = dataBaseList[index]["ItemName"];
                result.ItemImage = Resources.Load<Sprite>("Lottery/TestS");
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
    public GameObject SetGachaItemUI(GachaItem gachaItem, GameObject resultUI)
    {
        resultUI.gameObject.GetComponent<GachaItem>().ItemId = gachaItem.ItemId;
        resultUI.gameObject.GetComponent<GachaItem>().ItemName = gachaItem.ItemName;
        resultUI.gameObject.GetComponent<GachaItem>().Amount = gachaItem.Amount;

        // �˸��� UI ���
        resultUI.transform.GetChild(0).GetComponent<Image>().sprite = gachaItem.ItemImage;
        resultUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gachaItem.ItemName;
        resultUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gachaItem.Amount.ToString();
        return resultUI;
    }

    /// <summary>
    /// �̹� ������ Character�� ���� ���
    /// ��ȯ�Ǵ� �������� �����ִ� UI Setting �Լ�
    //  - GachaSceneController.cs���� ���
    /// </summary>
    /// <param name="gachaItem"></param>
    /// <param name="resultUI"></param>
    /// <returns></returns>
    public GameObject SetGachaReturnItemUI(GachaItem gachaItem, GameObject resultUI)
    {
        resultUI.gameObject.GetComponent<GachaItem>().ItemId = gachaItem.ItemId;
        resultUI.gameObject.GetComponent<GachaItem>().Amount = gachaItem.Amount;

        // �˸��� UI ���
        resultUI.transform.GetChild(0).GetComponent<Image>().sprite = gachaItem.ItemImage;
        resultUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gachaItem.Amount.ToString();
        return resultUI;
    }

}
