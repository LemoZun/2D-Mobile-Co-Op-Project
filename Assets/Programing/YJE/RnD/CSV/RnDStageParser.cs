using System.Collections.Generic;
using UnityEngine;

public class RnDStageParser : MonoBehaviour
{
    // DataManager���� ������ DataLists�� ���� List����
    List<Dictionary<string, string>> dictionary = new List<Dictionary<string, string>>();

    // ���ϴ� �����͸� ����ȯ �� ������ ����Ʈ(Stage �ڷ���)
    List<RnDStageCSV> stages = new List<RnDStageCSV>();
    private void Update()
    {
        // �ڷ� �׽�Ʈ ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DataLoadTest();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DataShow();
        }
    }

    private void DataLoadTest()
    {
        // DataManager.cs���� �Ľ̵� ������ �ҷ�����
        dictionary = RnDDataManager.Instance.DataLists[0];
        // [index][���ϴ� �׸� ���� string]
        for(int i = 0; i < dictionary.Count; i++)
        {
            Debug.Log(dictionary[i]["Id"]);
            Debug.Log(dictionary[i]["StageName"]);
            Debug.Log(dictionary[i]["TimeLimit"]);
            Debug.Log(dictionary[i]["MonsterCount"]);
            //Debug.Log(dictionary[i]["MonsterPos"]);

            RnDStageCSV stage = new RnDStageCSV();
            // �� stage�� �������� �ڷ����� ���߾� ��ȯ �� ����
            stage.id = int.Parse(dictionary[i]["Id"]);
            stage.stageName = dictionary[i]["StageName"];
            stage.timeLimit = int.Parse(dictionary[i]["TimeLimit"]);
            stage.monsterCount = int.Parse(dictionary[i]["MonsterCount"]);
            //stage.monsterPos = int.Parse(dictionary[i]["MonsterPos"]);
            stages.Add(stage); // �ϼ��� stage�� stages ����Ʈ�� ����
        }
    }
    private void DataShow()
    {
        for (int i = 0; i < stages.Count; i++)
        {
            Debug.Log($"{i} ��°");
            Debug.Log(stages[i].id);
            Debug.Log(stages[i].stageName);
            Debug.Log(stages[i].timeLimit);
            Debug.Log(stages[i].monsterCount);
            //Debug.Log(stages[i].monsterPos);
        }
    }
}
