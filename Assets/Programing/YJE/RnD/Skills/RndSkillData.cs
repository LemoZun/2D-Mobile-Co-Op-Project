using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RndSkillData : ScriptableObject
{
    // TODO : ��ų���� ������ ���� ����

    // �����Լ��� �پ��� �Ű�����(ȸ����/���ݰ�/�� ��)�� �޴� �����Լ��� �����Ͽ� ���
    public virtual void DoSkill() { }
    public virtual void DoSkill(List<GameObject> enemyList, int damage) { }

}
