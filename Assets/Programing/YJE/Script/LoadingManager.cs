using System;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] ShopMakeStart shopMakeStart;
    [SerializeField] ShopSceneController shopSceneController;
    private bool isLoading = false;
    private event Action OnStartSetting;

    private void OnEnable()
    {
        OnStartSetting += shopMakeStart.MakeGachaList; // ����� ���� ����Ʈ �ϼ�
        OnStartSetting += shopMakeStart.MakeItemDic; // ����� itemDic �ϼ�
        OnStartSetting += shopMakeStart.MakeCharDic; // ����� charDic �ϼ�
        OnStartSetting += shopMakeStart.MakeCharReturnItemDic; // ����� charReturnItemDic �ϼ�
        OnStartSetting += shopMakeStart.ShopCharMaker; // ���� ĳ���� ���Ÿ�� �ϼ�
        OnStartSetting += shopSceneController.SettingStartUI; // ���� UI ����
    }
    private void OnDisable()
    {
        // GameObject ��Ȱ��ȭ �� �̺�Ʈ ����
        OnStartSetting -= shopMakeStart.MakeGachaList;
        OnStartSetting -= shopMakeStart.MakeItemDic;
        OnStartSetting -= shopMakeStart.MakeCharDic;
        OnStartSetting -= shopMakeStart.MakeCharReturnItemDic;
        OnStartSetting -= shopMakeStart.ShopCharMaker;
        OnStartSetting -= shopSceneController.SettingStartUI;
    }

    private void Update()
    {
        // CsvDataManger�� �ε��� �Ϸ�Ǿ����� Ȯ�� - �����׽�Ʈ �� if�� �ʿ� x
        // Setting �ϷḦ Ȯ���ؼ� LoadingCheck�� ����
        if (CsvDataManager.Instance.IsLoad)
        {
            if (!isLoading)
            {
                OnStartSetting?.Invoke();
                isLoading = true;
                gameObject.SetActive(false);
            }
            else
                gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
