using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Dictionary<int, Dictionary<string, string>> dataBaseList = new Dictionary<int, Dictionary<string, string>>();
    private Dictionary<int, GachaItem> itemDictionary = new Dictionary<int, GachaItem>(); // ������ Dictionary
    private Dictionary<int, GachaChar> charDictionary = new Dictionary<int, GachaChar>(); // ĳ���� Dictionary
    private Dictionary<int, GachaItemReturn> charReturnItemDic = new Dictionary<int, GachaItemReturn>(); // �ߺ� ĳ���� ��ȯ ������ Dictionary

    private List<Gacha> baseGachaList = new List<Gacha>(); // �⺻ �̱� List
    public List<Gacha> BaseGachaList { get { return baseGachaList; } set { baseGachaList = value; } }
    private List<Gacha> eventGachaList = new List<Gacha>(); // �̺�Ʈ �̱� List
    public List<Gacha> EventGachaList { get { return baseGachaList; } set { baseGachaList = value; } }

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
        ShowBaseGachaPanel();
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
    /// csv�����ͷ� �˸��� ���� ����Ʈ�� �и��ϴ� �Լ�
    /// - ���ο� ��í ������ ����Ʈ�� �߰��Ϸ��� ���
    ///     1. csv ���Ͽ� GachaGroup�� ��� ���� ����
    ///     2. LoadingCheck ��ũ��Ʈ �տ� GachaGroup�� ������ŭ ����Ʈ ����
    ///     2. �Լ��� switch���� ���ο� case�� GachaGroup �б��� ����
    ///     3. �� GachaGroup�� ����Ʈ �ʱ�ȭ
    //  - LoadingCheck.cs���� �̺�Ʈ�� ���
    /// </summary>
    public void MakeGachaList()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������

        for (int i = 1; i < dataBaseList.Count; i++)
        {
            Debug.Log(dataBaseList[i]["Check"]);
            Gacha gacha = new Gacha();
            gacha.Check = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Check"]);
            switch (gacha.Check) // ������ Ȯ��
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

            switch (dataBaseList[i]["GachaGroup"]) // GachaGroup�� Ȯ���Ͽ� List�� ����
            {
                case "1":
                    BaseGachaList.Add(gacha);
                    break;
                case "2":
                    EventGachaList.Add(gacha);
                    break;
                default:
                    break;
            }

        }
    }

    /// <summary>
    /// DB���� �޾ƿ� Item�� GachaItem ������ ����Ʈ�� ����� �� �ִ� ���·� �Ҵ�
    /// - GachaBtn.cs ���� �������� ��ȯ�� �� UI�� �������Ѽ� �����ϱ� ���� ���
    /// - Item�� ���� �߰��� ������ �����ؾ��ϰ� �� ItemId�� �����Ͽ� ����ؾ��ϸ� GachaItem.cs�� MakeItemList�Լ� �б� �߰��� �ʿ���
    //  - LoadingCheck.cs���� �̺�Ʈ�� ����
    /// </summary>
    public void MakeItemDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Item];
        GachaItem gold = new GachaItem();
        gold = gold.MakeItemList(dataBaseList, gold, 500);
        itemDictionary.Add(gold.ItemId, gold);

        GachaItem dinoBlood = new GachaItem();
        dinoBlood = dinoBlood.MakeItemList(dataBaseList, dinoBlood, 501);
        itemDictionary.Add(dinoBlood.ItemId, dinoBlood);

        GachaItem boneCrystal = new GachaItem();
        boneCrystal = boneCrystal.MakeItemList(dataBaseList, boneCrystal, 502);
        itemDictionary.Add(boneCrystal.ItemId, boneCrystal);

        GachaItem dinoStone = new GachaItem();
        dinoStone = dinoStone.MakeItemList(dataBaseList, dinoStone, 503);
        itemDictionary.Add(dinoStone.ItemId, dinoStone);

        GachaItem stone = new GachaItem();
        stone = stone.MakeItemList(dataBaseList, stone, 504);
        itemDictionary.Add(stone.ItemId, stone);

    }

    /// <summary>
    /// DB���� �޾ƿ� Character�� GachaChar ������ ����Ʈ���� ����� �� �ִ� ���·� ����
    /// - GachaBtn.cs ���� �������� ��ȯ�� �� UI�� �������Ѽ� �����ϱ� ���� ���
    /// - Character�� ���� �߰��� ������ �����ؾ��ϰ� �� CharId�� �����Ͽ� ����ؾ��ϸ� GachaChar.cs�� MakeCharList�Լ� �б� �߰��� �ʿ���
    //  - LoadingCheck.cs���� �̺�Ʈ�� ����
    /// </summary>
    public void MakeCharDic()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Character];
        GachaChar tricia = new GachaChar();
        tricia = tricia.MakeCharList(dataBaseList, tricia, 1);
        charDictionary.Add(tricia.CharId, tricia);
        GachaChar celes = new GachaChar();
        celes = celes.MakeCharList(dataBaseList, celes, 2);
        charDictionary.Add(celes.CharId, celes);
        GachaChar regina = new GachaChar();
        regina = regina.MakeCharList(dataBaseList, regina, 3);
        charDictionary.Add(regina.CharId, regina);
        GachaChar spinne = new GachaChar();
        spinne = spinne.MakeCharList(dataBaseList, spinne, 4);
        charDictionary.Add(spinne.CharId, spinne);
        GachaChar aila = new GachaChar();
        aila = aila.MakeCharList(dataBaseList, aila, 5);
        charDictionary.Add(aila.CharId, aila);
        GachaChar quezna = new GachaChar();
        quezna = quezna.MakeCharList(dataBaseList, quezna, 6);
        charDictionary.Add(quezna.CharId, quezna);
        GachaChar uloro = new GachaChar();
        uloro = uloro.MakeCharList(dataBaseList, uloro, 7);
        charDictionary.Add(uloro.CharId, uloro);
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
            charReturnItemDic.Add(i, gachaItemReturn);
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
        // Lobby�� ���ư��� ��ư �Լ� ����
        GetUI<Button>("BackBtn").onClick.AddListener(gachaBtn.BackToRobby);
    }

    /// <summary>
    /// BaseGachaPanel Ȱ��ȭ
    /// - EventGachaPanel ��Ȱ��ȭ
    /// ChangeBaseGachaBtn�� ����
    /// </summary>
    private void ShowBaseGachaPanel()
    {
        // �⺻ �г� Ȱ��ȭ
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(false);
        // ���ư��� ��ư Ȱ��ȭ
        GetUI<Image>("BackBtn").gameObject.SetActive(true);
        // ���� ĳ���� Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
        // ��í ��� �г� ��Ȱ��ȭ
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        // ��í ���� ��ư Ȱ��ȭ
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(true);
    }

    /// <summary>
    /// EventGachaPanel Ȱ��ȭ
    /// - BaseGachaPanel ��Ȱ��ȭ
    /// ChangeEventGachaBtn�� ����
    /// </summary>
    private void ShowEventGachaPanel()
    {
        // �̺�Ʈ �г� Ȱ��ȭ
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(false);
        // ���ư��� ��ư Ȱ��ȭ
        GetUI<Image>("BackBtn").gameObject.SetActive(true);
        // ���� ĳ���� Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
        // ��í ��� �г� ��Ȱ��ȭ
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        // ��í ���� ��ư Ȱ��ȭ
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(true);
    }

    /// <summary>
    /// Single/TenResult Panel Ȱ��ȭ �� ��Ȱ��ȭ �ϴ� UI
    /// </summary>
    private void DisableResultPanel()
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
        // ���� ĳ���� ��Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(false);
        // ���ư��� ��ư ��Ȱ��ȭ
        GetUI<Image>("BackBtn").gameObject.SetActive(false);
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
        DisableResultPanel(); // ��Ȱ��ȭ �Լ�
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
        DisableResultPanel(); // ��Ȱ��ȭ �Լ�
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
        // ���� ĳ���� Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
        // ���ư��� ��ư Ȱ��ȭ
        GetUI<Image>("BackBtn").gameObject.SetActive(true);
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
                GachaChar resultChar = charDictionary[GachaList[index].CharId];
                resultChar.Amount = GachaList[index].Count;
                GameObject resultCharUI = Instantiate(resultCharPrefab, singleResultContent);
                resultCharUI = resultChar.SetGachaCharUI(resultChar, resultCharUI);
                return resultCharUI;
            case 1: // ��ȯ�� �������� ���
                GachaItem result = itemDictionary[GachaList[index].ItemId]; // GachaItem ����
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
            case 0: // ��ȯ�� ĳ������ ���
                GachaChar resultChar = charDictionary[GachaList[index].CharId];
                resultChar.Amount = GachaList[index].Count;
                GameObject resultCharUI = Instantiate(resultCharPrefab, tenResultContent);
                resultCharUI = resultChar.SetGachaCharUI(resultChar, resultCharUI);
                return resultCharUI;
            case 1: // ��ȯ�� �������� ���
                GachaItem result = itemDictionary[GachaList[index].ItemId]; // GachaItem ����
                result.Amount = GachaList[index].Count; // GachaItem�� Amount�� ������ �������� ����

                GameObject resultUI = Instantiate(resultItemPrefab, tenResultContent); // Prefab���� ������ ��ġ�� ���� - ����
                resultUI = result.SetGachaItemUI(result, resultUI); // GachaItem�� ������ UI Setting

                return resultUI;
            default:
                return null;
        }
    }

    /// <summary>
    /// ��í������ Character�� �ߺ��� Ȯ���� �� Character�� �̹� �����ϰ� �ִ� ���
    /// ���������� ��ȯ�� �������� �˸��� UI�� ���
    //  - GachaCheck.cs���� ���
    /// </summary>
    /// <param name="UnitId"></param>
    /// <param name="resultListObj"></param>
    public GameObject CharReturnItem(int UnitId, GameObject resultListObj)
    {
        returnContent = resultListObj.transform.GetChild(3).GetComponent<RectTransform>();
        GameObject resultObjUI = Instantiate(returnPrefab, returnContent); // �� ��ġ�� ���ο� ���������� ����

        GachaItem resultItem = resultObjUI.gameObject.GetComponent<GachaItem>(); // �����տ� ���� ����
        resultItem.ItemId = charReturnItemDic[UnitId].ItemId;
        resultItem.Amount = charReturnItemDic[UnitId].Count;
        resultItem.ItemName = itemDictionary[charReturnItemDic[UnitId].ItemId].ItemName;
        resultItem.ItemImage = itemDictionary[charReturnItemDic[UnitId].ItemId].ItemImage;

        resultObjUI = resultItem.SetGachaReturnItemUI(resultItem, resultObjUI);

        return resultObjUI;
    }
}
