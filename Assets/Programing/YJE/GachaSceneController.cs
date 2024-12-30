using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D;
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

    [Header("Gacha Lists")]
    public List<Gacha> baseGachaList = new List<Gacha>();
    public List<Gacha> eventGachaList = new List<Gacha>();

    [Header("UI")]
    [SerializeField] RectTransform singleResultContent; // 1���� ��� ���� �������� ���� �� ��ġ
    [SerializeField] RectTransform tenResultContent; // 10���� ��� ���� �������� ���� �� ��ġ
    [SerializeField] GameObject resultCharPrefab; // ����� ĳ������ ��� ����� ������
    [SerializeField] GameObject resultItemPrefab; // ����� �������� ��� ����� ������

    private void Awake()
    {
        gachaBtn = gameObject.GetComponent<GachaBtn>();
        BindAll();
        // SettingStartUI();
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
                case 1: // ������ Item�� ���
                    gachatem.ItemId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["ItemID"]);
                    break;
                case 2: // ������ Character�� ���
                    gachatem.CharId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["CharID"]);
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
    /// DB���� �޾ƿ� Item�� GachaItme ������ ����Ʈ�� ����� �� �ִ� ���·� ����
    /// - GachaBtn.cs ���� �������� ��ȯ�� �� UI�� �������Ѽ� �����ϱ� ���� ���
    //  - LoadingCheck.cs���� �̺�Ʈ�� ���
    /// - �ǹ�
    ///   1. UI�� GachaBtn���� �����ϴ� �� �´���
    ///   2. DB�� �����ϴ� GachaSceneController.cs���� �ϴ� �� �´� �� ������, Item�� ĳ���� ��� ������ �����ϴ°�
    ///      �������� + Ȯ�强 ���鿡�� �������� Ȯ���� ����
    /// </summary>
    public void MakeItemList()
    {
        dataBaseList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Item];
        GachaItem gold = new GachaItem();
        gold.ItemId = 500;
        gold.ItemName = dataBaseList[500]["ItemName"];
        gold.ItemImage = Resources.Load<Sprite>("Lottery/TestG");
        ItemDictionary.Add(gold.ItemId, gold);

        GachaItem dinoBlood = new GachaItem();
        dinoBlood.ItemId = 501;
        dinoBlood.ItemName = dataBaseList[501]["ItemName"];
        dinoBlood.ItemImage = Resources.Load<Sprite>("Lottery/TestDB");
        ItemDictionary.Add(dinoBlood.ItemId, dinoBlood);

        GachaItem boneCrystal = new GachaItem();
        boneCrystal.ItemId = 502;
        boneCrystal.ItemName = dataBaseList[502]["ItemName"];
        boneCrystal.ItemImage = Resources.Load<Sprite>("Lottery/TestBC");
        ItemDictionary.Add(boneCrystal.ItemId, boneCrystal);

        GachaItem dinoStone = new GachaItem();
        dinoStone.ItemId = 503;
        dinoStone.ItemName = dataBaseList[503]["ItemName"];
        dinoStone.ItemImage = Resources.Load<Sprite>("Lottery/TestDS");
        ItemDictionary.Add(dinoStone.ItemId, dinoStone);

        GachaItem stone = new GachaItem();
        stone.ItemId = 504;
        stone.ItemName = dataBaseList[504]["ItemName"];
        stone.ItemImage = Resources.Load<Sprite>("Lottery/TestS");
        ItemDictionary.Add(stone.ItemId, stone);

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
            case 1: // ��ȯ�� �������� ���
                GachaItem result = ItemDictionary[GachaList[index].ItemId];
                GameObject resultUI = Instantiate(resultItemPrefab, singleResultContent);

                // �˸��� UI ���
                resultUI.transform.GetChild(0).GetComponent<Image>().sprite = result.ItemImage;
                resultUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = result.ItemName;
                resultUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GachaList[index].Count.ToString();
                return resultUI;
            case 2:
                // TODO : ��ȯ�� ĳ������ ���
                return null;
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
            case 1: // ��ȯ�� �������� ���
                GachaItem result = ItemDictionary[GachaList[index].ItemId];
                GameObject resultUI = Instantiate(resultItemPrefab, tenResultContent);

                // �˸��� UI ���
                resultUI.transform.GetChild(0).GetComponent<Image>().sprite = result.ItemImage;
                resultUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = result.ItemName;
                resultUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GachaList[index].Count.ToString();
                return resultUI;
            case 2:
                // TODO : ��ȯ�� ĳ������ ���
                return null;
            default:
                return null;
        }
    }
}
