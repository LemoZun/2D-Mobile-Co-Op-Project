using UnityEngine;

/// <summary>
/// �ӽ� test�� ���� ��ũ��Ʈ
/// - DataManager�� �� �ҷ����� ���¿��� �ε��ǵ��� �ϱ� ���� ��ũ��Ʈ
/// - ���� �κ���� ���� ���� ����
/// </summary>
public class DBTest : MonoBehaviour
{
    // LotterySetting
    [SerializeField] GameObject target;
    private void Update()
    {
        if (CsvDataManager.Instance.IsLoad)
        {
            Debug.Log("����");
            target.SetActive(true); // LotterySetting.cs ����
            gameObject.SetActive(false); // Tester ��Ȱ��ȭ
        }
    }
}
