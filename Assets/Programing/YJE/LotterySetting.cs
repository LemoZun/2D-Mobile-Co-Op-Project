using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotterySetting : MonoBehaviour
{
    // csvDataManager.cs���� ������ Ư�� DataList�� ���� List
    List<Dictionary<string, string>> settingList = new List<Dictionary<string, string>>();

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
        // TODO : csv ���Ͽ��� GachaID�� ������ �ľ��Ͽ� �ʿ��� �� List ����
    }
}
