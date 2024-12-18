using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class RnDLoadData : MonoBehaviour
{
    // Database�� ���� �� ���� ��ġ�� ���� => Firebase���� ��ũ �κ�
    DatabaseReference root;
    DatabaseReference stages;

    private void Update()
    {
        root = BackendManager.Database.RootReference;
        // �⺻ �������� �ڽ� �� Stages�� ��������
        stages = root.Child("Stages");
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            RnDSettingDate(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RnDSettingDate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RnDSettingDate(2);
        }
    }

    /// <summary>
    /// �⺻ ������ �ҷ����� ���
    /// </summary>
    private void RnDFirebaseLoad()
    {
        DatabaseReference testCleared = stages.Child("0").Child("stageName");
        testCleared.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            // �������� ���� �� ����
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            // DataSnapshot���� ����� �ޱ� => key, Value������
            DataSnapshot snapshot = task.Result;
            Debug.Log(snapshot.Value.ToString());
        });
    }

    private void RnDSettingDate(int id)
    {
        RnDStage rnDStage = new RnDStage();

        DatabaseReference getID = stages.Child($"{id}");

        getID.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted) // ������ �������⿡ ������ ���
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            // �����͸� DataSnapshot���� ��� ��������
            DataSnapshot snapshot = task.Result;

            // �� ����� string���� ������ �˸��� �ڷ������� ��ȯ - ����
            string result = snapshot.Child("isCleared").Value.ToString();
            rnDStage.isCleared = bool.Parse(result);
            rnDStage.name = snapshot.Child("stageName").Value.ToString();
            result = snapshot.Child("timeLimit").Value.ToString();
            rnDStage.timeLimit = int.Parse(result);

            Debug.Log(rnDStage.isCleared);
            Debug.Log(rnDStage.name);
            Debug.Log(rnDStage.timeLimit);
        });

        /*DatabaseReference testCleared = stages.Child($"{id}").Child("isCleared");
        DatabaseReference teststageName = stages.Child($"{id}").Child("stageName");
        DatabaseReference testTimeLimit = stages.Child($"{id}").Child("timeLimit");

        testCleared.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            // �������� ���� �� ����
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            // DataSnapshot���� ����� �ޱ� => key, Value������
            DataSnapshot snapshot = task.Result;
            string result = snapshot.Value.ToString();
            bool isCleared = bool.TryParse(result, out isCleared);
            RnDStage.isCleared = isCleared;
            Debug.Log(RnDStage.isCleared);
        });
        teststageName.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            // �������� ���� �� ����
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            // DataSnapshot���� ����� �ޱ� => key, Value������
            DataSnapshot snapshot = task.Result;
            string result = snapshot.Value.ToString();
            RnDStage.name = result;
            Debug.Log(RnDStage.name);

        });
        testTimeLimit.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            // �������� ���� �� ����
            if (task.IsFaulted)
            {
                Debug.Log("GetValueAsync encountered an error: " + task.Exception);
                return;
            }

            // DataSnapshot���� ����� �ޱ� => key, Value������
            DataSnapshot snapshot = task.Result;
            string result = snapshot.Value.ToString();
            int testTimeLimit = int.Parse(result);
            RnDStage.timeLimit = testTimeLimit;
            Debug.Log(RnDStage.timeLimit);
        });*/
    }
}
