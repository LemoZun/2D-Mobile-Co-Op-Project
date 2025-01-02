using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class BattleSceneUi : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject winUi;
    [SerializeField] private GameObject loseUi;



    [SerializeField] private float time;
    [SerializeField] private float curTime;

    private int minute;
    private int second;

    private void OnEnable()
    {
        time = BattleSceneManager.Instance._timeLimit;
        //time = 120f;

        StartCoroutine(startTimer());
    }

    IEnumerator startTimer() 
    {
        curTime = time;
        while (curTime > 0)
        {
            curTime -= Time.deltaTime;
            minute = (int)curTime / 60;
            second = (int)curTime % 60;
            timerText.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if (curTime <= 0)
            {
                Debug.Log("�ð� ����");
                curTime = 0;
                yield break;
            }
        }
    }
    public void WinorLose() 
    {   
        
        Debug.Log("�׽�Ʈ");
        if (BattleSceneManager.Instance.myUnits.All(item => item.gameObject.activeInHierarchy ==false)) // list�� ������ ���� false�� 
        {
            // �й�
            Debug.Log("�й�");
            BattleSceneManager.Instance.curBattleState = BattleSceneManager.BattleState.Lose;
        }
        else if (BattleSceneManager.Instance.enemyUnits.All(item => item.gameObject.activeInHierarchy == false)) 
        {
            // �¸�
            Debug.Log("�¸�");
            BattleSceneManager.Instance.curBattleState = BattleSceneManager.BattleState.Win;
        }
        else if (curTime <= 0)
        {
            // �ð����� �й�
            Debug.Log("�ð����� �й�");
            BattleSceneManager.Instance.curBattleState = BattleSceneManager.BattleState.Lose;
        }

    }
    public void inToDbData() 
    {
        // �¸��� ȹ�� ������ + �¸� ���(�������� Ŭ���� ���� ����) , ���Ŀ� ���̵� Ŭ���� ��� ���Ե� ����
        // db ���� + playerdatamanager �����ؾ���
        Debug.Log("������ ����");
        

    }
    public void goToLobby() 
    {   
        inToDbData();
        // �¸��� ȹ�� ������ + �¸� ���(�������� Ŭ���� ���� ����)

    }


}
