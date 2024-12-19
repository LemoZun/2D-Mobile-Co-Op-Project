using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
/// <summary>
/// CSV���� �ҷ����� Unit�� ���� 1 �⺻ �����͸� �����ϱ� ���� Ŭ����
/// id / name / ���ݷ� / ���� / ü�� / ȸ���� / ���߷� / ġ��Ÿ / �ڽ�Ʈ ȸ���� / ȸ�Ƿ� / ��Ÿ� / type / ������ ���
/// 
/// </summary>

public enum UnitType
{
    water, fire, earth, grass
    }
public class RnDUnitBase : RnDUnit
{
    public int id { get; private set; }
    public string name { get; private set; }
    public int atk { get; private set; }
    public int def { get; private set; }
    public int hp { get; private set; }
    public int healHp { get; private set; }
    public int hit { get; private set; }
    public int critical { get; private set; }
    public int healCost { get; private set; }
    public int dodge { get; private set; }
    public int sight { get; private set; }
    public UnitType type { get; private set; }
    public double levelUp { get; private set; }

}
