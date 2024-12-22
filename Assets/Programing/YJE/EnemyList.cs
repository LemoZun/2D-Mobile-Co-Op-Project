using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemyList = new List<GameObject>();
    public bool isChanged = false;
    private void Awake()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            enemyList.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }
    private void Update()
    {
        isChanged = false;
    }
    /*
        private void Update()
        {
            // ����Ʈ�� ������ �ڽĿ�����Ʈ�� ������ �ٸ���(�������ִ°���)
            if(enemyList.Count != gameObject.transform.childCount)
            {
                enemyList.Clear(); // ����Ʈ ����
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    enemyList.Add(gameObject.transform.GetChild(i).gameObject);
                }
                isChanged = true;
            }
            else
            {
                isChanged = false;
            }
        }
    */
    /// <summary>
    /// �ڽ��� ���� ����� ������ ȣ��Ǵ� �Լ�
    /// </summary>
    private void OnTransformChildrenChanged()
    {

        if (transform.childCount == 0)
        {
            Debug.Log("�� ����");
        }
        else if (transform.childCount >= 1)
        {
            enemyList.Clear(); // ����Ʈ ����
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                enemyList.Add(gameObject.transform.GetChild(i).gameObject);
            }
            isChanged = true;
        }

    }
}
