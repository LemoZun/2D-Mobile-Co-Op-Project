using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OJHTest : MonoBehaviour
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
        //foreach(Dictionary<string, string> field in stageDic)
        //{
        //    if (field["Id"] == "0")
        //    {
        //        a = int.Parse(field["TimeLimit"]);
        //        Debug.Log(a);
        //    }
        //}
        
        
    }
}
