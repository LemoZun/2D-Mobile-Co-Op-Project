using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RnDUintParser : MonoBehaviour
{
    // DataManager���� ������ DataLists�� ���� List����
    List<Dictionary<string, string>> dictionary = new List<Dictionary<string, string>>();
    // �ҷ��� unit�� ���������� ������ ����Ʈ
    List<RnDUnitBase> unitBases = new List<RnDUnitBase>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnitDataLoadTest();
        }
    }

    private void UnitDataLoadTest()
    {
        dictionary = DataManager.Instance.DataLists[0];
        for (int i = 0; i < dictionary.Count; i++)
        {
            Debug.Log(dictionary[i]["Id"]);
            Debug.Log(dictionary[i]["Name"]);
            Debug.Log(dictionary[i]["Atk"]);
            Debug.Log(dictionary[i]["Def"]);
            Debug.Log(dictionary[i]["Hp"]);
            Debug.Log(dictionary[i]["HealHp"]);
            Debug.Log(dictionary[i]["Hit"]);
            Debug.Log(dictionary[i]["Critical"]);
            Debug.Log(dictionary[i]["HealCost"]);
            Debug.Log(dictionary[i]["Dodge"]);
            Debug.Log(dictionary[i]["Sight"]);
            Debug.Log(dictionary[i]["Type"]);
            Debug.Log(dictionary[i]["LevelUp"]);
        }
    }
}
