using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⺻������ ��ų�� ������ �� ��ΰ� �ʿ�� �� �Լ��� �����Լ��� ����
/// �� ��ų �з�(���ϵ�/������/������/������)�� ���� ���� �����ϴ� Scriptable Object�� 
/// ����Ͽ� ���
/// </summary>
public class SkillBaseSO : ScriptableObject
{
    public virtual void DoSkill(int damage, List<GameObject> target, GameObject unit)
    {

    }
    public virtual void DoAnimationSkill()
    {

    }
    public virtual void DoSoundSkill()
    {

    }
}
