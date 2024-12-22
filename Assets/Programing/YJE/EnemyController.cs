using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] UnitSO unit;
    int curHp;

    private void Awake()
    {
        curHp = unit.hp;
    }


    public void TakeDamage(int damage)
    {
        Debug.Log("���� ü�� ���� �̺�Ʈ �߻�" + damage);
        curHp -= damage;
        Debug.Log("���� ü�� : " + curHp);
        if (curHp <= 0)
        {
            Debug.Log("���� ������� �Ǵ�");
            Die();
        }
        else return;
    }
    private void Die()
    {
        Debug.Log("���� ü�� : " + curHp);
        Debug.Log("���� ���������");
        Destroy(gameObject);
    }
}
