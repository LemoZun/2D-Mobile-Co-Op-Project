using System;
using UnityEngine;

/// <summary>
/// LoadingPanel�� Ȱ��ȭ ���θ� �Ǵ��ϴ� ��ũ��Ʈ
/// - GachaScene�� ���۵Ǹ� Ȱ��ȭ ���·� ����
/// - isLoading = true�� ����Ǹ� LoadingCheck Panel ��Ȱ��ȭ
/// - �׽�Ʈ �뵵�� �̱����� DataManager�� Data�� PlayerDataManager���� PlayerData�� ����� �ҷ��� �� ���� Ȯ�� ��
/// - (���� ���) ���� Setting�� �ϴ� �Լ� ����
/// - TODO : ���� �ÿ��� �׽�Ʈ �κ� �ּ�ó�� �ʼ�
/// - ��� ���� �� BaseGacahPanel / ChangeBaseGachaBtn / ChangeEventGacahBtn Ȱ��ȭ�ϰ� LoadingPanel�� ��Ȱ��ȭ
/// </summary>
public class LoadingCheck : MonoBehaviour
{
    [SerializeField] GachaSceneController gachaSceneController;
    private bool isLoading = false;

    // GachaSceneController���� Scene�� �����ϱ� �� �ʿ��� Setting�� �ϴ� �̺�Ʈ ����
    private event Action OnStartSetting;

    private void OnEnable()
    {
        OnStartSetting += gachaSceneController.SettingStartUI; // PlayerData���� ���� �ҷ��� �� ��ȭ ����
        OnStartSetting += gachaSceneController.MakeGachaList; // �׷캰�� �̱� List Setting
        OnStartSetting += gachaSceneController.MakeItemDic; // ����ϴ� Item�� GachaItem������ Dictionary Setting
        OnStartSetting += gachaSceneController.MakeCharDic; // ����ϴ� ĳ���͸� GachaChar������ Dictionary Setting
        OnStartSetting += gachaSceneController.MakeCharReturnItemDic; // �ߺ� ĳ���� �̱� �� GachaItemReturn������ Dictionary Setting
        OnStartSetting += gachaSceneController.SettingBtn; // �� ��ư�� �˸��� �Լ� �Ҵ�
    }
    private void OnDisable()
    {
        // GameObject ��Ȱ��ȭ �� �̺�Ʈ ����
        OnStartSetting -= gachaSceneController.SettingStartUI;
        OnStartSetting -= gachaSceneController.MakeGachaList;
        OnStartSetting -= gachaSceneController.MakeItemDic;
        OnStartSetting -= gachaSceneController.MakeCharDic;
        OnStartSetting -= gachaSceneController.MakeCharReturnItemDic;
        OnStartSetting -= gachaSceneController.SettingBtn;
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
