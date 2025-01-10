using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DBTest�� �����Ͽ� DataManager�� Data�� ���� �ҷ��� �Ŀ� ���� ������ ����
/// - ��ü���� ���� ������׿� �ʿ��� ������� �ٸ� Ŭ������ �����Ϳ� ������ ������ ����
/// - ���� �ε����� �� ��� ������ ��ģ �� �÷��̾ �̱⸦ ���� �� �� �־����
/// - ��ü ���� �� LoadingPanel ��Ȱ��ȭ + button Ȱ��ȭ �ϸ鼭 �̱⸦ ����
/// - �κ���� ������ DBTest�� ������� LoadingPanel�� Ȱ��ȭ �� ���¿��� ������ �����ϰ�
///   ������ �Ϸ�Ǹ� ��ü ���� �� LoadingPanel ��Ȱ��ȭ + button Ȱ��ȭ �ϸ鼭 �̱⸦ �����ϵ��� �ؾ���
/// </summary>
public class LotterySetting : MonoBehaviour
{
    // csvDataManager.cs���� ������ Ư�� DataList�� ���� Disctionary
    [SerializeField] Dictionary<int, Dictionary<string, string>> gachaList = new Dictionary<int, Dictionary<string, string>>();
    [Header("Lottery Lists")]
    public List<Lottery> lotteryList1 = new List<Lottery>();
    public List<Lottery> lotteryList2 = new List<Lottery>();

    //List<Dictionary<string, string>> settingList = new List<Dictionary<string, string>>();
    private bool isChecked;
    public bool IsChecked { get { return isChecked; } set { isChecked = value; } }

    [Header("UI")]
    [SerializeField] private GameObject singleBtn;
    [SerializeField] private GameObject tenBtn;
    [SerializeField] private GameObject loadingPanel;

    private void Awake()
    {

        // TODO :
        // MakeLotteryList() ����
        // LotteryDataLoad() ����
        // ��ư�� Ȱ��ȭ
        // Loading Panel ��Ȱ��ȭ
        MakeLotteryList();
        singleBtn.SetActive(true);
        tenBtn.SetActive(true);
        loadingPanel.SetActive(false);

    }

    /// <summary>
    /// csv�����ͷ� �˸��� ��í ����Ʈ�� �и��ϴ� �Լ�
    /// - ���ο� ��í�� �߰��ϴ� ���
    ///    1. csv���Ͽ� GachaGroup�� ��� ���� ���ε�
    ///    2. �Լ��� switch���� ���ο� case�� GachaGroup �б��� ����
    ///    3. Lottery Lists�� �б��� list�� �̸� ���� �� �б⿡ �˸°� ������ ����
    /// </summary>
    private void MakeLotteryList()
    {
        gachaList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������
        for (int i = 1; i < gachaList.Count + 1; i++)
        {
            // Lottery Ÿ���� lottery�� �����ϰ� ����ȯ�� ���� ID�� Probability�� ����
            Lottery lottery = new Lottery();
            lottery.Id = TypeCastManager.Instance.TryParseInt(gachaList[i]["ItemID"]);
            lottery.Probability = TypeCastManager.Instance.TryParseInt(gachaList[i]["Probability"]);
            // GachaGroup�� �������� �˸��� loggeryList�� ����
            switch (gachaList[i]["GachaGroup"])
            {
                case "1":
                    lotteryList1.Add(lottery);
                    break;
                case "2":
                    lotteryList2.Add(lottery);
                    break;
                default:
                    break;
            }
        }
    }
}
