using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BattleSceneManager : MonoBehaviour
{
    private static BattleSceneManager _instance;
    public static BattleSceneManager Instance { get { return _instance; } set { _instance = value; } }



    [SerializeField] private DraggableUI[] Draggables;


    //BaseUnitController �� ������ �����ؾ� �� 
    [SerializeField] public GameObject[] inGridObject; // �Ʊ� ���� �迭 ���߿� Ÿ���� �ٲٸ� �ɵ�
    [SerializeField] public List<BaseUnitController> myUnits;
    [SerializeField] public List<BaseUnitController> enemyUnits;

    [SerializeField] public string[] enemyGridObject;// �� ���� �迭 �迭�� �ε����� ��ġ�� , id ���� 

    [SerializeField] public int _timeLimit; // private ������Ƽ�� �ٲ� ����

    [SerializeField] public Dictionary<int, int> curItemValues = new Dictionary<int, int>();// Ŭ���� ����
        
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inGridObject = new GameObject[10]; // ĳ���� ����� 0�� 
        enemyGridObject = new string[9];
        myUnits = new List<BaseUnitController>();
        enemyUnits = new List<BaseUnitController>();
    }
    public UnityEvent startStage;
    public void StageStart()
    {
        // 1 ~ 5�θ� ��� ����
        for (int i = 1; i < inGridObject.Length; i++) 
        {
            if (inGridObject[i] != null) 
            {
                UnitStat unitStat = inGridObject[i].GetComponent<UnitStat>();
                string x =" ";
                for (int j = 0; j < unitStat.buffs.Count; j++) // ���� ����� ����غ��� �ҵ�
                {
                    x += unitStat.buffs[j].y.ToString() + ":" + unitStat.buffs[j].y.ToString() + " , ";
                }
                Debug.Log($"��ġ : {i} id :  {unitStat.Id} ���� {unitStat.Level} ����� �������� : {x}"); //���߿� ����� ������ ui�� �߸� ������ ����Ʈ�� �ǽð����� �������̴� 
            }
        }
        for (int i = 0; i < enemyGridObject.Length; i++) 
        {
            if (enemyGridObject[i] != null) 
            {
                int id = int.Parse(enemyGridObject[i]);
                string name = CsvDataManager.Instance.DataLists[5][id]["MonsterName"];
                string damage = CsvDataManager.Instance.DataLists[5][id]["Damage"];
                string armor = CsvDataManager.Instance.DataLists[5][id]["Armor"];
                string hp = CsvDataManager.Instance.DataLists[5][id]["MonsterHP"];
                Debug.Log($"��ġ :  {i+1} id : {id} �̸� : {name} �� : {damage} �� :{armor} ü : {hp}");
            }
        }
        
    }
    public void BackStage()
    {   
         
        // ���������г� , ��Ʋ�� �Ŵ������� �ʱ�ȭ �ؾ���
        for (int i = 0; i < inGridObject.Length; i++)
        {
            
            if (inGridObject[i] != null) 
            {
               // inGridObject[i].GetComponent<DraggableUI>().ResetDragables();
            }
            inGridObject[i] = null;

        }
        for (int i = 0; i < enemyGridObject.Length; i++)
        {
            enemyGridObject[i] = null;
        }

    }
    public void GetDraggables()
    {
       
        for (int i = 0; i < 13; i++)
        {
            if (Draggables[i] == null)
            {
              
                Draggables[i] = GameObject.Find("Slot" + i.ToString()).GetComponent<DraggableUI>();
                Draggables[i].gameObject.SetActive(false);

            }

        }
        Debug.Log(PlayerDataManager.Instance.PlayerData.UnitDatas.Count);
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; i++) // db ���� ���� ������ ��ŭ�� ���̰� 
        {
            Draggables[i].gameObject.SetActive(true);

            int id = PlayerDataManager.Instance.PlayerData.UnitDatas[i].UnitId;
            int maxHp = int.Parse(CsvDataManager.Instance.DataLists[0][id]["BaseHp"]);
            int atk = int.Parse(CsvDataManager.Instance.DataLists[0][id]["BaseATK"]);
            int def = int.Parse(CsvDataManager.Instance.DataLists[0][id]["BaseDef"]);
            string element = CsvDataManager.Instance.DataLists[0][id]["ElementID"];

            string name = CsvDataManager.Instance.DataLists[0][id]["Name"];
            string level = PlayerDataManager.Instance.PlayerData.UnitDatas[i].UnitLevel.ToString();
            Sprite sprite = Resources.Load<Sprite>("Portrait/portrait_"+id.ToString());
            Draggables[i].GetComponent<CharSlot>().setCharSlotData(id,name, level,sprite); // �̹����� ���ҽ� ���ϱ������� �������
                                                                                           // ���ҽ� ���� �̸��� id ������ ���� ��Ű�� �ɵ���
            Draggables[i].GetComponent<UnitStat>().setStats(maxHp,atk,def,int.Parse(level),id,element);
            //���� ���´� csv���� �ٷ� �������� ������ ,�̰� ���⼭ �ϴ°� �³� ?   
        }

    }

    private void BattleSceneStart()
    {
        // �� ��ġ, ����(id?)
        // �Ʊ� ��ġ, ���� ���� ���� 
        // �������� �ð� 

        Spawner spawner = FindAnyObjectByType<Spawner>();

        spawner.SpawnUnits();

    }


}