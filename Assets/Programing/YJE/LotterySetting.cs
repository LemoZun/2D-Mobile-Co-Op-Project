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

    //List<Dictionary<string, string>> settingList = new List<Dictionary<string, string>>();

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
        LotteryDataLoad();
        singleBtn.SetActive(true);
        tenBtn.SetActive(true);
        loadingPanel.SetActive(false);

    }

    private void LotteryDataLoad()
    {
        // TODO : cvs ���� DB���� ���ϴ� ������ �ҷ��� GachaID ���� List<Lottery> ����
        // Lottery.cs�� Lottery �׸� ����

    }

    private void MakeLotteryList()
    {
        gachaList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������
        for(int i = 0; i < gachaList.Count; i++)
        {

        }
        // TODO : csv ���Ͽ��� GachaID�� ������ �ľ��Ͽ� �ʿ��� �� List ����

    }
}
