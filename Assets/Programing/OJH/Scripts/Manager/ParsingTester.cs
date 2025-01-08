using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ParsingTester : MonoBehaviour
{
    [SerializeField] private int a;

    //string�̱� ������ ���Ŀ� int�� float���� ����ȯ �ʿ�.
    private Dictionary<int, Dictionary<string, string>> stageDic;



    [ContextMenu("DebugTest")]
    public void Test()
    {
       stageDic = CsvDataManager.Instance.DataLists[(int)E_CsvData.Stat];

        Debug.Log(TypeCastManager.Instance.TryParseInt(stageDic[2]["4"]));

        //������ ���� ���� ã�� ���
        foreach (var outerKeyValue in stageDic)
        {
            // �ܺ� ��ųʸ��� Ű (int)
            Debug.Log("Outer Key: " + outerKeyValue.Key);

            // ���� ��ųʸ� (Dictionary<string, string>) ���
            foreach (var innerKeyValue in outerKeyValue.Value)
            {
                // ���� ��ųʸ��� Ű�� �� (string, string)
                Debug.Log("  Inner Key: " + innerKeyValue.Key + ", Inner Value: " + innerKeyValue.Value);
            }
        }


    }
}
