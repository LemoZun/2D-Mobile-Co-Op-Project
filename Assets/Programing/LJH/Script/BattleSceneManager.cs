using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    private static BattleSceneManager _instance;
    public static BattleSceneManager Instance { get { return _instance; } set { _instance = value; } }


    
    private DraggableUI[] Draggables;

    [SerializeField] public GameObject[] inGridObject; // �Ʊ� ���� �迭 ���߿� Ÿ���� �ٲٸ� �ɵ�
    [SerializeField] public string[] enemyGridObject;// �� ���� �迭
    [SerializeField] public int _timeLimit; // private ������Ƽ�� �ٲ� ���� , 
   
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
            Debug.Log("5�� �ʰ� ��� �Ұ�");
        }
    }
    public void BackStage() 
    {
        // ���������г� , ��Ʋ�� �Ŵ������� �ʱ�ȭ �ؾ���
        for (int i = 0; i < inGridObject.Length; i++) 
        {
            inGridObject[i] = null; 
            enemyGridObject[i] = null;
        }
        for (int i = 0; i < Draggables.Length; i++) 
        {
            Destroy(Draggables[i].gameObject);
        }
    } 

    public void getDraggables() 
    {
        StartCoroutine(delay());
    }

    IEnumerator delay() 
    {
        yield return 0.1f;

        Draggables = FindObjectsOfType<DraggableUI>();
    }


    private void BattleSceneStart() 
    {
        // �� ��ġ, ����(id?)

        // �Ʊ� ��ġ, ���� ���� ���� 
        // �������� �ð� 

    }
}