using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO : 
// 1. Firebase�� ����� �÷��̾� �����͸� Ȯ���ϰ�
//    - ���̳� ������ ������ ��� �̱� ���� x
//    - �̱⸦ �����ϴ� ��� ����� ���̳� ���� ��ȭ�� Firebase�� ����
// 2. �̱� ����� �����Ͽ� Firebase�� ����� ���� ����
//    - Item�� ��� ���� �÷��̾��� �����Ϳ� ���� ������ ����
//    - ĳ������ ��� ���� �÷��̾ ������ �ִ��� Ȯ��
//      1. �÷��̾ ������ �ִ� ��� ���������� ��ȯ�Ͽ� ����
//      2. �÷��̾ ������ ���� ���� ��� �÷��̾� �������� ���ֿ� �߰��� ����

/// <summary>
/// GachaScene�� ��ü���� ������ �ϴ� ��ũ��Ʈ
/// - CsvDataManager�� ����
/// - PlayData�� ����
/// - UIBInder�� ����Ͽ� �̺�Ʈ ���� �� �˸°� �̺�Ʈ�� �� UI�� Ȱ��ȭ ����
/// </summary>
public class GachaSceneController : UIBInder
{
    GachaBtn gachaBtn;

    // csvDataManager.cs���� ������ Ư�� DataList�� ���� Disctionary
    Dictionary<int, Dictionary<string, string>> dataBaseList = new Dictionary<int, Dictionary<string, string>>();

    public Dictionary<int, GachaItem> ItemDictionary = new Dictionary<int, GachaItem>();
    public Dictionary<int, GachaChar> CharDictionary = new Dictionary<int, GachaChar>();
    public Dictionary<int, GachaItemReturn> CharReturnItemDic = new Dictionary<int, GachaItemReturn>();

    [Header("Gacha Lists")]
    public List<Gacha> baseGachaList = new List<Gacha>();
    public List<Gacha> eventGachaList = new List<Gacha>();

    [Header("UI")]
    [SerializeField] RectTransform singleResultContent; // 1���� ��� ���� �������� ���� �� ��ġ
    [SerializeField] RectTransform tenResultContent; // 10���� ��� ���� �������� ���� �� ��ġ
    [SerializeField] GameObject resultCharPrefab; // ����� ĳ������ ��� ����� ������
    [SerializeField] GameObject resultItemPrefab; // ����� �������� ��� ����� ������
    [SerializeField] RectTransform returnContent; // �ߺ�ĳ���� ������ ��ȯ �������� ���� �� ��ġ
    [SerializeField] GameObject returnPrefab; // �ߺ�ĳ���� ������ ��ȯ ������

    private void Awake()
    {
        gachaBtn = gameObject.GetComponent<GachaBtn>();
        BindAll();
        SettingStartPanel();
    }
    /// <summary>
    /// ���� �� ��ư�� ���� ����
    /// - ��ư�� ���� ���� ����
    //  - LoadingCheck.cs���� �̺�Ʈ�� ���
    /// </summary>
    public void SettingStartUI()
    {
        // �� Button �ؽ�Ʈ ����
        GetUI<TextMeshProUGUI>("BaseSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("BaseTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("EventSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("EventTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("ChangeBaseGacahText").SetText("��");
        GetUI<TextMeshProUGUI>("ChangeEventGacahText").SetText("�̺�Ʈ");
        UpdatePlayerUI();
    }
    /// <summary>
    /// �� Item ��ȭ ��� ǥ��
    /// - ���� �� ��� ������Ʈ�� �ʿ��ϹǷ� �Լ��� �����Ͽ� ���
    /// </summary>
    public void UpdatePlayerUI()
    {
        GetUI<TextMeshProUGUI>("CoinText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin].ToString());
        GetUI<TextMeshProUGUI>("DinoBloodText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood].ToString());
        GetUI<TextMeshProUGUI>("BoneCrystalText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal].ToString());
        GetUI<TextMeshProUGUI>("DinoStoneText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone].ToString());
        GetUI<TextMeshProUGUI>("StoneText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Stone].ToString());
    }
    /// <summary>
    /// ���� �� �г� Ȱ��ȭ�� ��Ȱ��ȭ ����
    /// </summary>
    private void SettingStartPanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(false);
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ShopCharacter").gameObject.SetActive(false);
    }

    /// <summary>
    /// csv�����ͷ� �˸��� ���� ����Ʈ�� �и��ϴ� �Լ�
    //  - LoadingCheck.cs���� �̺�Ʈ�� ���
    /// - ���ο� ��í ������ ����Ʈ�� �߰��Ϸ��� ���
    ///     1. csv ���Ͽ� GachaGroup�� ��� ���� ����
    ///     2. LoadingCheck ��ũ��Ʈ �տ� GachaGroup�� ������ŭ ����Ʈ ����
    ///     2. �Լ��� switch���� ���ο� case�� GachaGroup �б��� ����
    ///     3. �� GachaGroup�� ����Ʈ �ʱ�ȭ
    /// </summary>
    public void MakeGachaList()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������
        for (int i = 1; i < dataBaseList.Count; i++)
        {
            Debug.Log(dataBaseList[i]["Check"]);
            Gacha gachatem = new Gacha();
            gachatem.Check = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Check"]);
            switch (gachatem.Check) // ������ Ȯ��
            {
                case 0: // ������ Character�� ���
                    gachatem.CharId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["CharID"]);
                    break;
                case 1: // ������ Item�� ���
                    gachatem.ItemId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["ItemID"]);
                    break;
                default:
                    break;
            }
            gachatem.Probability = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Probability"]); // Ȯ�� ����
            gachatem.Count = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Count"]); // ��ȯ ���� ����

            switch (dataBaseList[i]["GachaGroup"]) // GachaGroup�� Ȯ���Ͽ� List�� ����
            {
                case "1":
                    baseGachaList.Add(gachatem);
                    break;
                case "2":
                    eventGachaList.Add(gachatem);
                    break;
                default:
                    break;
            }

        }
    }

    /// <summary>
    /// DB���� �޾ƿ� Item�� GachaItem ������ ����Ʈ�� ����� �� �ִ� ���·� ����
    /// - GachaBtn.cs ���� �������� ��ȯ�� �� UI�� �������Ѽ� �����ϱ� ���� ���
    //  - LoadingCheck.cs���� �̺�Ʈ�� ����
    /// </summary>
    public void MakeItemDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Item];
        GachaItem gold = new GachaItem();
        gold = gold.MakeItemList(dataBaseList, gold, 500);
        ItemDictionary.Add(gold.ItemId, gold);

        GachaItem dinoBlood = new GachaItem();
        dinoBlood = dinoBlood.MakeItemList(dataBaseList, dinoBlood, 501);
        ItemDictionary.Add(dinoBlood.ItemId, dinoBlood);

        GachaItem boneCrystal = new GachaItem();
        boneCrystal = boneCrystal.MakeItemList(dataBaseList, boneCrystal, 502);
        ItemDictionary.Add(boneCrystal.ItemId, boneCrystal);

        GachaItem dinoStone = new GachaItem();
        dinoStone = dinoStone.MakeItemList(dataBaseList, dinoStone, 503);
        ItemDictionary.Add(dinoStone.ItemId, dinoStone);

        GachaItem stone = new GachaItem();
        stone = stone.MakeItemList(dataBaseList, stone, 504);
        ItemDictionary.Add(stone.ItemId, stone);

    }

    /// <summary>
    /// DB���� �޾ƿ� Character�� GachaChar ������ ����Ʈ���� ����� �� �ִ� ���·� ����
    /// - GachaBtn.cs ���� �������� ��ȯ�� �� UI�� �������Ѽ� �����ϱ� ���� ���
    /// - ĳ���� ��� �߰� �� �߰� �ʿ�
    //  - LoadingCheck.cs���� �̺�Ʈ�� ����
    /// </summary>
    public void MakeCharDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];
        GachaChar tricia = new GachaChar();
        tricia = tricia.MakeCharList(dataBaseList, tricia, 1);
        CharDictionary.Add(tricia.CharId, tricia);
        GachaChar celes = new GachaChar();
        celes = celes.MakeCharList(dataBaseList, celes, 2);
        CharDictionary.Add(celes.CharId, celes);
        GachaChar regina = new GachaChar();
        regina = regina.MakeCharList(dataBaseList, regina, 3);
        CharDictionary.Add(regina.CharId, regina);
        GachaChar spinne = new GachaChar();
        spinne = spinne.MakeCharList(dataBaseList, spinne, 4);
        CharDictionary.Add(spinne.CharId, spinne);
        GachaChar aila = new GachaChar();
        aila = aila.MakeCharList(dataBaseList, aila, 5);
        CharDictionary.Add(aila.CharId, aila);
        GachaChar quezna = new GachaChar();
        quezna = quezna.MakeCharList(dataBaseList, quezna, 6);
        CharDictionary.Add(quezna.CharId, quezna);
        GachaChar uloro = new GachaChar();
        uloro = uloro.MakeCharList(dataBaseList, uloro, 7);
        CharDictionary.Add(uloro.CharId, uloro);
    }
    /// <summary>
    /// DB���� �޾ƿ� GachaReturn�� GachaItemReturn ������ ����Ʈ���� ����� �� �ִ� ���·� ����
    /// - GachaBtn.cs ���� �ߺ�ĳ���͸� ���������� ��ȯ�� �� ���
    //  - LoadingCheck.cs���� �̺�Ʈ�� ����
    /// </summary>
    public void MakeCharReturnItemDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.GachaReturn];
        for (int i = 1; i <= dataBaseList.Count; i++)
        {
            GachaItemReturn gachaItemReturn = new GachaItemReturn();
            gachaItemReturn.ItemId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["ItemID"]);
            gachaItemReturn.Count= TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Count"]);
            CharReturnItemDic.Add(i, gachaItemReturn);
        }
    }

    /// <summary>
    /// UI��ư����
    //  - LoadingCheck.cs���� �̺�Ʈ�� ���
    /// </summary>
    public void SettingBtn()
    {
        // ����г� ��ư Ŭ�� �� �г� ��Ȱ��ȭ �Լ� ����
        GetUI<Button>("SingleResultPanel").onClick.AddListener(DisabledGachaResultPanel);
        GetUI<Button>("TenResultPanel").onClick.AddListener(DisabledGachaResultPanel);
        // GachaBtn ��ũ��Ʈ�� �� ��ư�� �Լ� ����
        GetUI<Button>("BaseSingleBtn").onClick.AddListener(gachaBtn.BaseSingleBtn);
        GetUI<Button>("BaseTenBtn").onClick.AddListener(gachaBtn.BaseTenBtn);
        GetUI<Button>("EventSingleBtn").onClick.AddListener(gachaBtn.EventSingleBtn);
        GetUI<Button>("EventTenBtn").onClick.AddListener(gachaBtn.EventTenBtn);
        // Gacha ���� ���� ��ư �Լ� ����
        GetUI<Button>("ChangeBaseGachaBtn").onClick.AddListener(ShowBaseGachaPanel);
        GetUI<Button>("ChangeEventGachaBtn").onClick.AddListener(ShowEventGachaPanel);
    }

    /// <summary>
    /// BaseGachaPanel Ȱ��ȭ
    /// - EventGachaPanel ��Ȱ��ȭ
    /// ChangeBaseGachaBtn�� ����
    /// </summary>
    private void ShowBaseGachaPanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(false);
    }
    /// <summary>
    /// EventGachaPanel Ȱ��ȭ
    /// - BaseGachaPanel ��Ȱ��ȭ
    /// ChangeEventGachaBtn�� ����
    /// </summary>
    private void ShowEventGachaPanel()
    {
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(false);
    }

    /// <summary>
    /// Single/TenResult Panel Ȱ��ȭ �� ��Ȱ��ȭ �ϴ� UI
    /// </summary>
    private void ShowResultPanelDisable()
    {
        // �⺻ �̱� ���� ���� ��ư ��Ȱ��ȭ
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(false);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(false);
        // �� ������ ��ȭ Text ��Ȱ��ȭ
        GetUI<TextMeshProUGUI>("CoinText").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("DinoBloodText").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("BoneCrystalText").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("DinoStoneText").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("StoneText").gameObject.SetActive(false);
    }

    /// <summary>
    /// SingleResultPanel Ȱ��ȭ
    //  - GachaBtn.cs���� ���
    /// </summary>
    public void ShowSingleResultPanel()
    {
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(true);
        GetUI<Image>("SingleResultPanel").gameObject.SetActive(true);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(false);
        ShowResultPanelDisable(); // ��Ȱ��ȭ �Լ�
    }
    /// <summary>
    /// TenResultPanel Ȱ��ȭ
    //  - GachaBtn.cs���� ���
    /// </summary>
    public void ShowTenResultPanel()
    {
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(true);
        GetUI<Image>("SingleResultPanel").gameObject.SetActive(false);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(true);
        ShowResultPanelDisable(); // ��Ȱ��ȭ �Լ�
    }


    /// <summary>
    /// GachaResultPanel ��Ȱ��ȭ
    /// - ��� ���� ����Ʈ�� �ʱ�ȭ
    /// - ��� �г��� ��Ȱ��ȭ
    // - GachaBtn.cs������ ���
    /// </summary>
    public void DisabledGachaResultPanel()
    {
        gachaBtn.ClearResultList(); // �̱��� ����� GachaBtn ��ũ��Ʈ�� ����Ǿ������� �ʱ�ȭ �ʼ�
        // �⺻ �̱� ���� ���� ��ư Ȱ��ȭ
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(true);
        // ��� �г� ��Ȱ��ȭ
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        GetUI<Image>("SingleResultPanel").gameObject.SetActive(false);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(false);
        // �� ������ ��ȭ Text Ȱ��ȭ
        GetUI<TextMeshProUGUI>("CoinText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("DinoBloodText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("BoneCrystalText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("DinoStoneText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("StoneText").gameObject.SetActive(true);
    }

    /// <summary>
    /// 1ȸ �̱� ���� ��
    /// GachaList�� index���� �޾Ƽ� �ش��ϴ� ����� ������/ĳ�������� �Ǵ�
    /// �з��� ���� Prefab���� GameObject�� ����
    /// �˸��� ����� UI�� ���
    /// GameObject�� ��ȯ�ϴ� �Լ�
    //  - GachaBtn.cs���� ���
    /// </summary>
    /// <param name="GachaList"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GachaSingleResultUI(List<Gacha> GachaList, int index)
    {
        switch (GachaList[index].Check)
        {
            case 0: // ��ȯ�� ĳ������ ���
                GachaChar resultChar = CharDictionary[GachaList[index].CharId];
                resultChar.Amount = GachaList[index].Count;
                GameObject resultCharUI = Instantiate(resultCharPrefab, singleResultContent);
                resultCharUI = resultChar.SetGachaCharUI(resultChar, resultCharUI);
                return resultCharUI;
            case 1: // ��ȯ�� �������� ���
                GachaItem result = ItemDictionary[GachaList[index].ItemId]; // GachaItem ����
                result.Amount = GachaList[index].Count; // GachaItem�� Amount�� ������ �������� ����

                GameObject resultUI = Instantiate(resultItemPrefab, singleResultContent); // Prefab���� ������ ��ġ�� ���� - �Ѱ�
                resultUI = result.SetGachaItemUI(result, resultUI); // GachaItem�� ������ UI Setting
                return resultUI;
            default:
                return null;
        }
    }

    /// <summary>
    /// 10ȸ �̱� ���� ��
    /// GachaList�� index���� �޾Ƽ� �ش��ϴ� ����� ������/ĳ�������� �Ǵ�
    /// �з��� ���� Prefab���� GameObject�� ����
    /// �˸��� ����� UI�� ���
    /// GameObject�� ��ȯ�ϴ� �Լ�
    //  - GachaBtn.cs���� ���
    /// </summary>
    /// <param name="GachaList"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GachaTenResultUI(List<Gacha> GachaList, int index)
    {
        switch (GachaList[index].Check)
        {
            case 0:
                // TODO : ��ȯ�� ĳ������ ���
                GachaChar resultChar = CharDictionary[GachaList[index].CharId];
                resultChar.Amount = GachaList[index].Count;
                GameObject resultCharUI = Instantiate(resultCharPrefab, tenResultContent);
                resultCharUI = resultChar.SetGachaCharUI(resultChar, resultCharUI);
                return resultCharUI;
            case 1: // ��ȯ�� �������� ���
                GachaItem result = ItemDictionary[GachaList[index].ItemId]; // GachaItem ����
                result.Amount = GachaList[index].Count; // GachaItem�� Amount�� ������ �������� ����

                GameObject resultUI = Instantiate(resultItemPrefab, tenResultContent); // Prefab���� ������ ��ġ�� ���� - ����
                resultUI = result.SetGachaItemUI(result, resultUI); // GachaItem�� ������ UI Setting

                return resultUI;
            default:
                return null;
        }
    }

    /// <summary>
    /// �̹� �̾Ƽ� ������ resultList�� ĳ���� ������ ������Ʈ resultListObj
    /// </summary>
    /// <param name="UnitId"></param>
    /// <param name="resultListObj"></param>
    public GameObject CharReturnItem(int UnitId, GameObject resultListObj)
    {
        returnContent = resultListObj.transform.GetChild(3).GetComponent<RectTransform>();
        GameObject resultObjUI = Instantiate(returnPrefab, returnContent); // �� ��ġ�� ���ο� ���������� ����

        GachaItem resultItem = resultObjUI.gameObject.GetComponent<GachaItem>();
        resultItem.ItemId = CharReturnItemDic[UnitId].ItemId;
        resultItem.Amount = CharReturnItemDic[UnitId].Count;
        resultItem.ItemName = ItemDictionary[CharReturnItemDic[UnitId].ItemId].ItemName;
        resultItem.ItemImage = ItemDictionary[CharReturnItemDic[UnitId].ItemId].ItemImage;

        resultObjUI = resultItem.SetGachaReturnItemUI(resultItem, resultObjUI);

        return resultObjUI;
    }
}
