using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    [SerializeField] TMP_Text stageNumText;  // �������� ��ȣ 
    [SerializeField] TMP_Text stageNameText; // �������� �̸� 
    [SerializeField] TMP_Text timeLimitText; // �ð�����

    [SerializeField] List<Dictionary<string, string>> stageDic;

    [SerializeField] List<int> curMobPos; // ���� ���������� ���� ��ġ
    [SerializeField] List<int> curmyPos;  // ���� ���������� �÷��̾��� ĳ���� ��ġ
     
    [SerializeField] string curStageNum;   // ���� ���������� ������
    [SerializeField] string curStageNames;
    [SerializeField] string curTimeLimit;
    [SerializeField] int curMobCount;
    [SerializeField] Image[] mygrid;
    [SerializeField] Image[] enemygrid;

    public void setStageData(int stageNum) //  stages csv�� �������� �������(0������)
    {
        stageDic = DataManager.Instance.DataLists[(int)E_CsvData.Stage]; //�Ľ��� ����(url ������� ��)
        curStageNum = stageDic[stageNum]["Id"];
        curStageNames = stageDic[stageNum]["StageName"];
        curTimeLimit = stageDic[stageNum]["TimeLimit"];
        curMobCount = int.Parse(stageDic[stageNum]["MonsterCount"]);

        for (int i = 0; i < enemygrid.Length; i++) // �� ���� �� �ʱ�ȭ
        {
            enemygrid[i].color = Color.black;
            mygrid[i].color = Color.black;
            curmyPos.Clear();
            curMobPos.Clear();
        }

        foreach (char val in stageDic[stageNum]["MonsterPos"])// �� ���ھ� ���ڷ� ��ȯ
        {
            curMobPos.Add(int.Parse(val.ToString()));
        }

        for (int i = 0; i < curMobPos.Count; i++)
        {

            enemygrid[curMobPos[i] - 1].color = Color.red;
        }


        stageNumText.text = curStageNum;
        stageNameText.text = curStageNames;
        timeLimitText.text =  "Time Limit : "+curTimeLimit+" sec";
    }

    public void setMyGrid() 
    {
        // Todo : ����ϸ� �ش� ��ġ�� �ö󰡸鼭 ���� ���� asdasd
    }


    public void StageStart()
    {
        Debug.Log("�������� ����");
        // Todo :  ĳ������ ��ġ ��ġ ���� + ��ġ����? + ������ ����� ������ ������ ������ ��Ʋ������ �Ѿ�� �� , + ������ ��ġ ���� 
    }
}
