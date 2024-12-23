using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����� �������� ���� ���� ���� Scriptable Object
/// - SkillBaseSO�� ��ӹ޾� �����ϴ� �Լ�
/// - ����� ����ȭ ��Ų �� UnitManager�� �Բ� Unit�� �־� ������ �� 
/// - �� ��ġ�� UnitManager���� �޾Ƽ� ����ϴ� ������� �����Ǿ�����
/// </summary>
[CreateAssetMenu(menuName = "AtkSingleSkill")]
public class AtkSingleSkillSO : SkillBaseSO
{
    private EnemyController enemyController;

    public override void DoSkill(int damage, List<GameObject> targetList, GameObject unit)
    {
        List<float> list = new List<float>();
        Debug.Log("���� ����� Ÿ�� ����");
        // ��ų�� ����ϴ� unit������Ʈ�� ���� ����Ʈ �� ���� ������ �����ϴ� ������ Ÿ������ ����
        for (int i = 0; i < targetList.Count; i++)
        {
            list.Add(Vector2.Distance(unit.transform.position, targetList[i].transform.position));
        }
        float value = list.Min();
        int index = list.IndexOf(value); // �ּҰ��� �ε��� ��ȣ
        GameObject target = targetList[index]; // ���� �� Ÿ��

        // ������ Ÿ���� TakeDamage() �Լ��� �ִ� ��ũ��Ʈ�� ����
        // TODO : ����� EnemyController.cs�̳� ���� EnemyUnit������ ������ ����
        enemyController = target.GetComponent<EnemyController>(); 
        Debug.Log("��ų ����" + target);
        Debug.Log("��ų ���� - " + damage);

        // ���ݹ��� Ÿ���� TakeDamge() ����
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
