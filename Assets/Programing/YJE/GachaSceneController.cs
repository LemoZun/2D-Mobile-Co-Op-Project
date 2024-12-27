using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GachaScene�� ��ü���� ������ �ϴ� ��ũ��Ʈ
/// - UIBInder�� ����Ͽ� �̺�Ʈ ���� �� �˸°� �̺�Ʈ�� �� UI�� Ȱ��ȭ ����
/// </summary>
public class GachaSceneController : UIBInder
{
    private bool isLoading = false;
    public bool IsLoading { get { return isLoading; } set { isLoading = value; } }
    // csvDataManager.cs���� ������ Ư�� DataList�� ���� Disctionary
    Dictionary<int, Dictionary<string, string>> gachaList = new Dictionary<int, Dictionary<string, string>>();

    [Header("Gacha Lists")]
    [SerializeField] public List<Gacha> baseGachaList = new List<Gacha>();
    [SerializeField] public List<Gacha> eventGachaList = new List<Gacha>();

    private void Awake()
    {
        BindAll();
        //MakeGachaList();
        ShowUIStart();
        DisablePanel();

    }
    private void Start()
    {

    }

    /// <summary>
    /// csv�����ͷ� �˸��� ���� ����Ʈ�� �и��ϴ� �Լ�
    /// - ���ο� ��í ������ ����Ʈ�� �߰��Ϸ��� ���
    ///     1. csv ���Ͽ� GachaGroup�� ��� ���� ����
    ///     2. LoadingCheck ��ũ��Ʈ �տ� GachaGroup�� ������ŭ ����Ʈ ����
    ///     2. �Լ��� switch���� ���ο� case�� GachaGroup �б��� ����
    ///     3. �� GachaGroup�� ����Ʈ �ʱ�ȭ
    /// </summary>
    private void MakeGachaList()
    {
        gachaList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������
        for (int i = 0; i < gachaList.Count; i++)
        {
            Gacha gachatem = new Gacha();
            gachatem.Check = TypeCastManager.Instance.TryParseInt(gachaList[i]["Check"]);
            switch (gachatem.Check) // ������ Ȯ��
            {
                case 1: // ������ Item�� ���
                    gachatem.ItemId = TypeCastManager.Instance.TryParseInt(gachaList[i]["ItemID"]);
                    break;
                case 2: // ������ Character�� ���
                    gachatem.CharId = TypeCastManager.Instance.TryParseInt(gachaList[i]["CharID"]);
                    break;
                default:
                    break;
            }
            gachatem.Probability = TypeCastManager.Instance.TryParseInt(gachaList[i]["Probability"]); // Ȯ�� ����
            gachatem.Count = TypeCastManager.Instance.TryParseInt(gachaList[i]["Count"]); // ��ȯ ���� ����

            switch (gachaList[i]["GachaGroup"]) // GachaGroup�� Ȯ���Ͽ� List�� ����
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
        // ��� ����Ʈ ����
        isLoading = true;
    }

    private void DisablePanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(false);
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ShopCharacter").gameObject.SetActive(false);
    }
    private void ShowUIStart()
    {
        GetUI<TextMeshProUGUI>("BaseSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("BaseTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("EventSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("EventTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("ChangeBaseGacahText").SetText("��");
        GetUI<TextMeshProUGUI>("ChangeEventGacahText").SetText("�̺�Ʈ");
    }

    private void SettingList()
    {

    }

    private void SettingBtn()
    {

    }

}
