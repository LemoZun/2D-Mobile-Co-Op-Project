using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    private static BattleSceneManager _instance;
    public static BattleSceneManager Instance { get { return _instance; } set { _instance = value; } }



    [SerializeField] private DraggableUI[] Draggables;
    

    [SerializeField] public GameObject[] inGridObject; // �Ʊ� ���� �迭 ���߿� Ÿ���� �ٲٸ� �ɵ�

    [SerializeField] public string[] enemyGridObject;// �� ���� �迭

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
        
    }

    public void StageStart()
    {
        string datas = "";
        int count = 0;
        Debug.Log("�������� ����");
        // ���� �Ѿ�� �ص� �ɵ� �̹� �Ŵ����� �����Ͱ� ������ 
        for (int i = 0; i < inGridObject.Length; i++)
        {
            if (inGridObject[i] != null)
            {
                datas += i.ToString();
                count++;
            }
        }
        Debug.Log(datas);
        if (count > 5)
        {
            Debug.Log("5�� �ʰ� ��� �Ұ�");  // 0 �� ��ߵ� ���ϰ� �ؾ���
        }
    }
    public void BackStage()
    {
        // ���������г� , ��Ʋ�� �Ŵ������� �ʱ�ȭ �ؾ���
        for (int i = 0; i < inGridObject.Length; i++)
        {
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
                Debug.Log("Slot" + i.ToString());
                Draggables[i] = GameObject.Find("Slot" + i.ToString()).GetComponent<DraggableUI>();
                Draggables[i].gameObject.SetActive(false);

            }

        }
        Debug.Log(PlayerDataManager.Instance.PlayerData.UnitDatas.Count);
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; i++)
        {
            Draggables[i].gameObject.SetActive(true);

            int id = PlayerDataManager.Instance.PlayerData.UnitDatas[i].UnitId;
            string name = CsvDataManager.Instance.DataLists[5][id]["Name"];
            string level = PlayerDataManager.Instance.PlayerData.UnitDatas[i].UnitLevel.ToString();
            Sprite sprite = Resources.Load<Sprite>("Portrait/portrait_"+id.ToString());
            Draggables[i].GetComponent<CharSlot>().setCharSlotData(name, level,sprite); // �̹����� ���ҽ� ���ϱ������� �������
                                                                                 // ���ҽ� ���� �̸��� id ������ ���� ��Ű�� �ɵ��� 
            // ���� ���´� csv���� �ٷ� �������� ������ ,�̰� ���⼭ �ϴ°� �³� ?   
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