using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotterySetting : MonoBehaviour
{
    // csvDataManager.cs���� ������ Ư�� DataList�� ���� List
    List<Dictionary<string, string>> settingList = new List<Dictionary<string, string>>();

    [Header("UI")]
    [SerializeField] private GameObject SingleBtn;
    [SerializeField] private GameObject TenBtn;
    [SerializeField] private GameObject LoadingPanel;

    private void Awake()
    {
        // TODO :
        // MakeLotteryList() ����
        // LotteryDataLoad() ����
        // ��ư�� Ȱ��ȭ
        // Loading Panel ��Ȱ��ȭ
        MakeLotteryList();
        LotteryDataLoad();
        SingleBtn.SetActive(true);
        TenBtn.SetActive(true);
        LoadingPanel.SetActive(false);

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
