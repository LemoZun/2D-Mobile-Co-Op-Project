using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// LoadingManager.cs�� �����Ͽ� ����ϴ�
/// ShopScene�� ���� ����
/// </summary>
public class ShopMakeStart : MonoBehaviour
{
    ShopSceneController shopSceneController;

    // csvDataManager.cs���� ������ Ư�� DataList�� ���� Disctionary
    private Dictionary<int, Dictionary<string, string>> dataBaseList = new Dictionary<int, Dictionary<string, string>>();
    private Dictionary<int, Item> itemDic = new Dictionary<int, Item>(); // ������ Dictionary
    public Dictionary<int, Item> ItemDic { get { return itemDic; } set { itemDic = value; } } 
    private Dictionary<int, ShopChar> charDic = new Dictionary<int, ShopChar>(); // ĳ���� Dictionary
    public Dictionary<int, ShopChar> CharDic { get { return charDic; } set { charDic = value; }  }
    private Dictionary<int, GachaReturn> charReturnItemDic = new Dictionary<int, GachaReturn>(); // �ߺ� ĳ���� ��ȯ ������ Dictionary
    public Dictionary<int, GachaReturn> CharReturnItemDic { get { return charReturnItemDic; } set { charReturnItemDic = value; } }
    private List<Gacha> baseGachaList = new List<Gacha>(); // �⺻ �̱� List
    public List<Gacha> BaseGachaList { get { return baseGachaList; } set { baseGachaList = value; } }

    private RectTransform characterContent;
    private GameObject shopCharPrefab;
    private void Awake()
    {
        shopSceneController = gameObject.GetComponent<ShopSceneController>();
        shopCharPrefab = Resources.Load<GameObject>("Prefabs/ShopListChar");
    }


    /// <summary>
    /// csv�����ͷ� �˸��� ���� ����Ʈ�� �и��ϴ� �Լ�
    /// - ���ο� ��í ������ ����Ʈ�� �߰��Ϸ��� ���
    ///     1. csv ���Ͽ� GachaGroup�� ��� ���� ����
    ///     2. LoadingCheck ��ũ��Ʈ �տ� GachaGroup�� ������ŭ ����Ʈ ����
    ///     2. �Լ��� switch���� ���ο� case�� GachaGroup �б��� ����
    ///     3. �� GachaGroup�� ����Ʈ �ʱ�ȭ
    /// </summary>
    public void MakeGachaList()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������

        for (int i = 1; i <= dataBaseList.Count; i++) // dataBaseList�� ���� Ȯ���ϸ鼭
        {
            Gacha gacha = new Gacha();
            gacha.Check = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Check"]);
            switch (gacha.Check) // ������ Ȯ���Ͽ� id ����
            {
                case 0: // ������ Character�� ���
                    gacha.CharId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["CharID"]);
                    break;
                case 1: // ������ Item�� ���
                    gacha.ItemId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["ItemID"]);
                    break;
                default:
                    break;
            }
            gacha.Probability = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Probability"]); // Ȯ�� ����
            gacha.Count = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Count"]); // ��ȯ ���� ����

            // GachaGroup�� Ȯ���Ͽ� List�� ���� - �̺�Ʈ ��í�� �߰��ϰ� ���� ��� GachaGroup�� �����Ͽ� �б��ϰ� ���ο� �̱� ����Ʈ�� �����Ͽ� ���
            switch (dataBaseList[i]["GachaGroup"])
            {
                case "1":
                    baseGachaList.Add(gacha);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// DB���� �޾ƿ� Item�� Item ������ ����Ʈ�� ����� �� �ִ� ���·� �Ҵ��Ͽ� itemDic �ϼ�
    /// - Item�� ���� �߰��� ������ �����ؾ��ϰ� �� ItemId�� �����Ͽ� ����ؾ��ϸ� GachaItem.cs�� MakeItemList�Լ� �б� �߰��� �ʿ���
    /// </summary>
    public void MakeItemDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Item];
        Item gold = new Item();
        gold = gold.MakeItemList(dataBaseList, gold, 500);
        itemDic.Add(gold.ItemId, gold);

        Item dinoBlood = new Item();
        dinoBlood = dinoBlood.MakeItemList(dataBaseList, dinoBlood, 501);
        itemDic.Add(dinoBlood.ItemId, dinoBlood);

        Item boneCrystal = new Item();
        boneCrystal = boneCrystal.MakeItemList(dataBaseList, boneCrystal, 502);
        itemDic.Add(boneCrystal.ItemId, boneCrystal);

        Item dinoStone = new Item();
        dinoStone = dinoStone.MakeItemList(dataBaseList, dinoStone, 503);
        itemDic.Add(dinoStone.ItemId, dinoStone);

        Item stone = new Item();
        stone = stone.MakeItemList(dataBaseList, stone, 504);
        itemDic.Add(stone.ItemId, stone);
    }

    /// <summary>
    ///  DB���� �޾ƿ� Character�� ShopChar ������ ����Ʈ���� ����� �� �ִ� ���·� ����
    /// - Character�� ���� �߰��� ������ �����ؾ��ϰ� �� CharId�� �����Ͽ� ����ؾ��ϸ� ShopChar.cs�� MakeCharList�Լ� �б� �߰��� �ʿ���
    /// - ShopChar.cs�� SetCharInfo()�� ����Ͽ� ���� ���� ShopChar�� ������ ����
    /// </summary>
    public void MakeCharDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];
        ShopChar tricia = new ShopChar();
        tricia = tricia.SetCharInfo(dataBaseList, 1);
        charDic.Add(tricia.CharId, tricia);
        ShopChar celes = new ShopChar();
        celes = celes.SetCharInfo(dataBaseList, 2);
        charDic.Add(celes.CharId, celes);
        ShopChar regina = new ShopChar();
        regina = regina.SetCharInfo(dataBaseList, 3);
        charDic.Add(regina.CharId, regina);
        ShopChar spinne = new ShopChar();
        spinne = spinne.SetCharInfo(dataBaseList, 4);
        charDic.Add(spinne.CharId, spinne);
        ShopChar aila = new ShopChar();
        aila = aila.SetCharInfo(dataBaseList, 5);
        charDic.Add(aila.CharId, aila);
        ShopChar quezna = new ShopChar();
        quezna = quezna.SetCharInfo(dataBaseList, 6);
        charDic.Add(quezna.CharId, quezna);
        ShopChar uloro = new ShopChar();
        uloro = uloro.SetCharInfo(dataBaseList, 7);
        charDic.Add(uloro.CharId, uloro);
        ShopChar eost = new ShopChar();
        eost = eost.SetCharInfo(dataBaseList, 8);
        charDic.Add(eost.CharId, eost);
        ShopChar melorin = new ShopChar();
        melorin = melorin.SetCharInfo(dataBaseList, 9);
        charDic.Add(melorin.CharId, melorin);
    }

    /// <summary>
    /// DB���� �޾ƿ� GachaReturn�� GachaItemReturn ������ ����Ʈ���� ����� �� �ִ� ���·� ����
    /// - GachaReturn.cs�� SetReturnInfo()�� ����Ͽ� ���� ���� GachaReturn�� ������ ����
    /// </summary>
    public void MakeCharReturnItemDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.GachaReturn];
        for (int i = 1; i <= dataBaseList.Count; i++)
        {
            GachaReturn gachaItemReturn = new GachaReturn();
            gachaItemReturn = gachaItemReturn.SetReturnInfo(dataBaseList, i);
            charReturnItemDic.Add(i, gachaItemReturn);
        }
    }


    /// <summary>
    /// charDic�� �̿��Ͽ� ������ ���Ÿ�Ͽ� ĳ���͸� ���� ���� ������Ʈ ����
    /// </summary>
    public void ShopCharMaker()
    {
        characterContent = shopSceneController.GetUI<RectTransform>("CharacterContent");

        for (int i = 1; i <= charDic.Count; i++)
        {
            GameObject shopCharUI = Instantiate(shopCharPrefab, characterContent);
            ShopChar shopChar = shopCharUI.GetComponent<ShopChar>();
            charDic.TryGetValue(i, out shopChar);

            shopCharUI = shopChar.SetShopCharInfo(shopChar, shopCharUI);
        }
    }
}
