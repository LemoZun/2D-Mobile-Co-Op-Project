using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "AtkSingleSkill")]
public class AtkSingleSkillSO : SkillBaseSO
{
    public EnemyController enemyController;

    public override void DoSkill(int damage, List<GameObject> targetList, GameObject unit)
    {
        List<float> list = new List<float>();
        Debug.Log("���� ����� Ÿ�� ����");
        for (int i = 0; i < targetList.Count; i++)
        {
            list.Add(Vector2.Distance(unit.transform.position, targetList[i].transform.position));
        }
        float value = list.Min();
        int index = list.IndexOf(value); // �ּҰ��� �ε��� ��ȣ

        GameObject target = targetList[index]; // ���� �� Ÿ��
        enemyController = target.GetComponent<EnemyController>();
        Debug.Log("��ų ����" + target);
        Debug.Log("��ų ���� - " + damage);
        enemyController.TakeDamage(damage);
    }
    public override void DoAnimationSkill()
    {
        Debug.Log("�ִϸ��̼� ���");
    }
    public override void DoSoundSkill()
    {
        Debug.Log("�Ҹ� ���");
    }
}
