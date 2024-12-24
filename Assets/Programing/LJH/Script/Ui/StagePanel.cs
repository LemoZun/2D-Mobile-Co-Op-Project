//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class StagePanel : MonoBehaviour
//{
//    [SerializeField] TMP_Text stageNumText;  // �������� ��ȣ 
//    [SerializeField] TMP_Text stageNameText; // �������� �̸� 
//    [SerializeField] TMP_Text timeLimitText; // �ð�����

//    [SerializeField] List<Dictionary<string, string>> stageDic;
//    [SerializeField] List<Dictionary<string, string>> monsterGroupDic;
//    [SerializeField] List<Dictionary<string, string>> monsterDic;
//    [SerializeField] List<Dictionary<string, string>> stageRewardDic;
//    [SerializeField] List<Dictionary<string, string>> itemDic;

//    [SerializeField] List<string> curMobPos; // ���� ���������� ���� ��ġ
//    [SerializeField] List<int> curmyPos;  // ���� ���������� �÷��̾��� ĳ���� ��ġ
     
//    [SerializeField] string curStageID;   // ���� ���������� ������
//    [SerializeField] string curStageNames;
//    [SerializeField] string curTimeLimit;

//    [SerializeField] string curMobGroup;
//    [SerializeField] string curRewardGroup;


    
//    [SerializeField] Image[] mygrid;
//    [SerializeField] Image[] enemygrid;

//    [SerializeField] List<string> itemIds;
//    [SerializeField] List<string> itemCounts;



//    // Monster, Stages, MontserGroup, StageReward, Item  08 ~ 12 ���߿� ��ĥ�� ����Ʈ ���� �����ؾ��� 
//    public void setStageData(int stageNum) //  stages csv�� �������� �������(0������)
//    {
//        stageDic = CsvDataManager.Instance.DataLists[1]; //�Ľ��� ����(url ������� ��)
//        curStageID = stageDic[stageNum]["StageID"];
//        curStageNames = stageDic[stageNum]["StageName"];
//        curTimeLimit = stageDic[stageNum]["Limit"];
//        BattleSceneManager.Instance._timeLimit = int.Parse(curTimeLimit);

//        curMobGroup = stageDic[stageNum]["MonsterGroupID"];
//        curRewardGroup = stageDic[stageNum]["StageRewardID"];
//        monsterGroupDic = CsvDataManager.Instance.DataLists[2];

//        gridClearing();

//        for (int i = 0; i < monsterGroupDic.Count; i++)  // ���� ���������� �´� ���� �׷� ã�Ƽ� �´� ��ġ�� ���� �ֱ�(���� id)
//        {
//            if (monsterGroupDic[i]["MonsterGroupID"] == curMobGroup) 
//            {
//                curMobPos[int.Parse(monsterGroupDic[i]["MonsterLocation"])] = monsterGroupDic[i]["MonsterID"];
//            }
//        }

//        monsterDic = CsvDataManager.Instance.DataLists[0];
//        for (int i = 0; i < curMobPos.Count; i++)
//        {
//            if (curMobPos[i] != null) 
//            {
//                enemygrid[i].color = Color.red;
//                BattleSceneManager.Instance.enemyGridObject[i] = curMobPos[i]; // ���� id�� instantiate�� �ȵɰ� ������ ���� �̸�����?
//            }
//        }

//        stageRewardDic = CsvDataManager.Instance.DataLists[3];
//        itemDic = CsvDataManager.Instance.DataLists[4];
//        for (int i = 0; i < stageRewardDic.Count; i++)   // �������� Ŭ����� ���� ������ ����  
//        {
//            if (stageRewardDic[i]["StageRewardID"] == curRewardGroup) 
//            {
//                itemIds.Add(stageRewardDic[i]["ItemID"]);
//                itemCounts.Add(stageRewardDic[i]["Count"]);
//                BattleSceneManager.Instance.curItemIDs.Add(stageRewardDic[i]["ItemID"]);
//                BattleSceneManager.Instance.curItemCounts.Add(stageRewardDic[i]["Count"]);
                
//            }
//        }
        
//        //BattleSceneManager.Instance.curRewardValue = Rewards;


//        stageNumText.text = curStageID;
//        stageNameText.text = curStageNames;
//        timeLimitText.text =  "Time Limit : "+curTimeLimit+" sec";
        
//    }
  

//    private void gridClearing() 
//    {
//        curmyPos.Clear(); 
//        curMobPos.Clear();
//        for (int i = 0; i < enemygrid.Length; i++) // �� ���� �� �ʱ�ȭ
//        {
//            enemygrid[i].color = Color.black;
//            mygrid[i].color = Color.black;
//            curMobPos.Add(null);
//            curmyPos.Add(0);
//        }
//    }
   
//}
