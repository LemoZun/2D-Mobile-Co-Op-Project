using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⺻���� �� ĳ������ �⺻ ����(Lv.1����)�� �����ϴ� Scriptable Object
/// - ���� csv ���Ͽ��� ������ ���� ��������
/// - ĳ���� ���� �ۼ��� ���� ������ �����ų��
/// ���� ���ǰ� �ʿ�
/// </summary>
[CreateAssetMenu(menuName = "Unit")]
public class UnitSO : ScriptableObject
{
    public int hp;
    public int damage;
    public int skillCode;
}
