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

    [SerializeField] Dictionary<int,Dictionary<string, string>> stageDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> monsterGroupDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> monsterDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> stageRewardDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> itemDic;
     
    [SerializeField] List<string> curMobPos; // ���� ���������� ���� ��ġ
    [SerializeField] List<int> curmyPos;  // ���� ���������� �÷��̾��� ĳ���� ��ġ
   
    [SerializeField] int curStageID;   // ���� ���������� ������
    [SerializeField] string curStageNames;
    [SerializeField] string curTimeLimit;

    [SerializeField] int curMobGroup;
    [SerializeField] int curRewardGroup;


  
    [SerializeField] Image[] mygrid;
    [SerializeField] Image[] enemygrid;

    [SerializeField] Dictionary<int, int> itemValues = new Dictionary<int, int>();


    // Monster, Stages, MontserGroup, StageReward, Item  08 ~ 12 ���߿� ��ĥ�� ����Ʈ ���� �����ؾ��� 
    public void setStageData(int stageNum) //  stages csv�� �������� �������(0������)
    {
        stageDic = CsvDataManager.Instance.DataLists[1]; //�Ľ��� ����(url ������� ��)

       // curStageID = TypeCastManager.Instance.TryParseInt(stageDic[101]["StageID"]);// stageDic[stageNum]["StageID"];
        curStageNames = stageDic[stageNum]["StageName"];// stageDic[stageNum]["StageName"];
        curTimeLimit = stageDic[stageNum]["Limit"];
        BattleSceneManager.Instance._timeLimit = int.Parse(curTimeLimit);

        curMobGroup = TypeCastManager.Instance.TryParseInt(stageDic[stageNum]["MonsterGroupID"]);
        curRewardGroup = TypeCastManager.Instance.TryParseInt(stageDic[stageNum]["StageRewardID"]); 

        monsterGroupDic = CsvDataManager.Instance.DataLists[2];

        gridClearing();

        for (int i = 0; i < 9; i++)  // ���� ���������� �´� ���� �׷� ã�Ƽ� �´� ��ġ�� ���� �ֱ�(���� id)
        {
            curMobPos[i] = monsterGroupDic[curMobGroup]["MonsterLocation" + (i+1).ToString()];
        }

        monsterDic = CsvDataManager.Instance.DataLists[0];
        for (int i = 0; i < curMobPos.Count; i++)
        {
            if (curMobPos[i] != "0") 
            {
                enemygrid[i].color = Color.red;
                BattleSceneManager.Instance.enemyGridObject[i] = curMobPos[i]; // ���� id�� instantiate�� �ȵɰ� ������ ���� �̸�����?
            }
        }

        stageRewardDic = CsvDataManager.Instance.DataLists[3]; // �������� Ŭ����� ���� ���� �ҷ�����
        itemDic = CsvDataManager.Instance.DataLists[4];
        foreach (string item in stageRewardDic[curRewardGroup].Keys) 
        {
            itemValues.Add(int.Parse(item),int.Parse(stageRewardDic[curRewardGroup][item]));
            BattleSceneManager.Instance.curItemValues.Add(int.Parse(item), int.Parse(stageRewardDic[curRewardGroup][item]));
        }

        stageNumText.text = curStageID.ToString();
        stageNameText.text = curStageNames;
        timeLimitText.text =  "Time Limit : "+curTimeLimit+" sec";
      
    }


    private void gridClearing() 
    {
        curmyPos.Clear(); 
        curMobPos.Clear();
        for (int i = 0; i < enemygrid.Length; i++) // �� ���� �� �ʱ�ȭ
        {
            enemygrid[i].color = Color.black;
            mygrid[i].color = Color.black;
            curMobPos.Add(null);
            curmyPos.Add(0);
        }
    }
 
}
