using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
     
    [SerializeField] TMP_Text stageNumText;  // �������� ��ȣ 
    [SerializeField] TMP_Text stageNameText; // �������� �̸� 
    [SerializeField] TMP_Text timeLimitText; // �ð�����

    [SerializeField] public RewardSlot[] rewards;

    [SerializeField] Dictionary<int,Dictionary<string, string>> stageDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> monsterGroupDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> monsterDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> stageRewardDic;
    [SerializeField] Dictionary<int, Dictionary<string, string>> itemDic;
     

    // cur �����Ŵ� �� ���������� ����
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
    //ĳ���� 0 ���� 5 ��������6 ���ͱ׷�7 ������׷�8 ������9 
    public void setStageData(int stageNum) //  stages csv�� �������� �������(0������)
    {
        stageDic = CsvDataManager.Instance.DataLists[6]; //�Ľ��� ����(url ������� ��)
        curStageNames = stageDic[stageNum]["StageName"];
        curTimeLimit = stageDic[stageNum]["Limit"];
        BattleSceneManager.Instance._timeLimit = int.Parse(curTimeLimit);

        curMobGroup = TypeCastManager.Instance.TryParseInt(stageDic[stageNum]["MonsterGroupID"]);
        curRewardGroup = TypeCastManager.Instance.TryParseInt(stageDic[stageNum]["StageRewardID"]); 
        monsterGroupDic = CsvDataManager.Instance.DataLists[7];
        gridClearing();

        for (int i = 0; i < 9; i++)  // ���� ���������� �´� ���� �׷� ã�Ƽ� �´� ��ġ�� ���� �ֱ�(���� id)
        {
            curMobPos[i] = monsterGroupDic[curMobGroup]["MonsterLocation" + (i+1).ToString()];
        }

        monsterDic = CsvDataManager.Instance.DataLists[5];
        for (int i = 0; i < curMobPos.Count; i++)
        {
            if (curMobPos[i] != "0") 
            {
                enemygrid[i].color = Color.red;
                BattleSceneManager.Instance.enemyGridObject[i] = curMobPos[i]; // ���� id�� instantiate�� �ȵɰ� ������ ���� �̸�����?
            }
        }

        stageRewardDic = CsvDataManager.Instance.DataLists[8]; // �������� Ŭ����� ���� ���� �ҷ����� 
        itemDic = CsvDataManager.Instance.DataLists[9];
        foreach (string item in stageRewardDic[curRewardGroup].Keys) 
        {
            if (stageRewardDic[curRewardGroup][item] != "0") 
            {
                itemValues[int.Parse(item)] = int.Parse(stageRewardDic[curRewardGroup][item]);
                BattleSceneManager.Instance.curItemValues[int.Parse(item)] = int.Parse(stageRewardDic[curRewardGroup][item]);
            }
        }
        int count = 0; 
        foreach (int id in itemValues.Keys) 
        {
            rewards[count].setRewardData(itemValues[id].ToString());
            count++;  
        }
        for (int i = count; i < rewards.Length; i++) 
        {
            rewards[i].gameObject.SetActive(false);
        }
        count = 0;
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
