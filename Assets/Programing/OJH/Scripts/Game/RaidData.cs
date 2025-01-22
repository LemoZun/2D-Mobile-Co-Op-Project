using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaidData
{
    [SerializeField] private string _name; // �̸�+ID

    public string Name { get { return _name; } set { _name = value; } }

    [SerializeField] private int _totalDamage; // ���� �� ������

    public int TotalDamage { get { return _totalDamage; } set { _totalDamage = value; } }

    [SerializeField] public int Rank; // ����
}
