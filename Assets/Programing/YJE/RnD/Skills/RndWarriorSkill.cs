using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "WarriorSkill")]
public class RndWarriorSkill : RndSkillData
{
    [SerializeField] private int damage;
    public override void DoSkill()
    {
        // TODO : ���� ��ų�� ��� ���� 
        Debug.Log($"������ ��ų ��� {damage}");
    }
}
