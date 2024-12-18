using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class RnDSaveData : MonoBehaviour
{
    // Database�� ���� �� ���� ��ġ�� ���� => Firebase���� ��ũ �κ�
    DatabaseReference root;
    DatabaseReference stages;

    // �̸� string ���� ��θ� �����Ͽ� ��뵵 ����
    // ex. public const string DataPath = "Stages";
    //     stages = root.Child(DataPath);

    private void Update()
    {
        root = BackendManager.Database.RootReference;
        // �⺻ �������� �ڽ� �� Stages�� ��������
        stages = root.Child("Stages");
        if (Input.GetKeyDown(KeyCode.X))
        {
            RnDChangeDatabase(50);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RnDChangeDatabase(150);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="test"></param>
    private void RnDChangeDatabase(bool test)
    {
        DatabaseReference testChange = stages.Child("0/isCleared");
        // ������ �ѹ� ����
        testChange.SetValueAsync(test);
    }
    private void RnDChangeDatabase(int test)
    {
        DatabaseReference testChange = stages.Child("0/timeLimit");
        // ������ �ѹ� ����
        testChange.SetValueAsync(test);
    }
}
