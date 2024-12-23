using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tester : MonoBehaviour
{
    [SerializeField] private int _baseUnitNum; //���� ������ �����ִ� ĳ���� ����.

    [SerializeField] private int[] _baseUnitIds;// ĳ���� ID

    [ContextMenu("Test")]
    public void Test()
    {

        List<Dictionary<string, string>> stageDic = DataManager.Instance.DataLists[(int)E_CsvData.Character];

        for (int i = 0; i < _baseUnitNum; i++)
        {
            PlayerUnitData unitData = new PlayerUnitData();

            Debug.Log($"{i}����");

            foreach (Dictionary<string, string> field in stageDic)
            {
                Debug.Log(field["CharID"]);
                if (int.Parse(field["CharID"]) == _baseUnitIds[i])
                {
                    unitData.Name = field["Name"];
                    unitData.UnitLevel = 0;
                    unitData.Type = field["Class"];
                    unitData.ElementName = field["ElementName"];
                    unitData.Hp = int.Parse(field["BaseHp"]);
                    unitData.Atk = int.Parse(field["BaseATK"]);
                    unitData.Def = int.Parse(field["BaseDef"]);
                    unitData.Grid = field["Grid"];
                    unitData.StatId = int.Parse(field["StatID"]);
                    unitData.PercentIncrease = int.Parse(field["PercentIncrease"]);

                    PlayerDataManager.Instance.PlayerData.UnitDatas.Add(unitData);
                    Debug.Log("hhhii");
                }
            }
        }
    }
}
