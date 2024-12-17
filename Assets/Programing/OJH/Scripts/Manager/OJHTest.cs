using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OJHTest : MonoBehaviour
{
    [SerializeField] private int a;

    //string�̱� ������ ���Ŀ� int�� float���� ����ȯ �ʿ�.
    private List<Dictionary<string, string>> stageDic;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Test();
        }
    }

    private void Test()
    {
        stageDic = DataManager.Instance.DataLists[(int)E_CsvData.Stage];

        // ��Ʈ�� ù��° ���� TileLimit �� 150
        Debug.Log(stageDic[0]["TimeLimit"]);

        //������ ���� ���� ã�� ���
        foreach(Dictionary<string, string> field in stageDic)
        {
            if (field["Id"] == "0")
            {
                a = int.Parse(field["TimeLimit"]);
                Debug.Log(a);
            }
        }
        
        
    }
}
