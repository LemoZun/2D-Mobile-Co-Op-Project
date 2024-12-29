using UnityEngine;

/// <summary>
/// LoadingPanel�� Ȱ��ȭ ���θ� �Ǵ��ϴ� ��ũ��Ʈ
/// - GachaScene�� ���۵Ǹ� Ȱ��ȭ ���·� ����
/// - �׽�Ʈ �뵵�� �̱����� DataManager�� Data�� PlayerDataManager���� PlayerData�� ����� �ҷ��� �� ���� Ȯ�� ��
/// - (���� ���) ���� Setting�� �ϴ� �Լ� ����
/// - TODO : ���� �ÿ��� �׽�Ʈ �κ� �ּ�ó�� �ʼ�
/// - ��� ���� �� BaseGacahPanel / ChangeBaseGachaBtn / ChangeEventGacahBtn Ȱ��ȭ�ϰ� LoadingPanel�� ��Ȱ��ȭ
/// </summary>
public class LoadingCheck : MonoBehaviour
{
    [SerializeField] GachaSceneController gachaSceneController;
    private void Update()
    {
        // TODO : ������ �׽�Ʈ �� �ּ�ó�� �ʿ�
            if (CsvDataManager.Instance.IsLoad)
            {
                if (gachaSceneController.IsLoading) // �������� ���� �ε� �κ�
                {
                    // TODO : �̺�Ʈ�� IsLoadingClear�� �����Ͽ� �����Ű��
                    // - BaseGachaPanel�� Ȱ��ȭ�ϰ�
                    // - ChangeBaseGachaBtn Ȱ��ȭ
                    // - ChangeEventGachaBtn Ȱ��ȭ
                    // - ShopCharacter Ȱ��ȭ
                    gameObject.SetActive(false);
                }
            }
        else
        {
            return;
        }
    }

}
