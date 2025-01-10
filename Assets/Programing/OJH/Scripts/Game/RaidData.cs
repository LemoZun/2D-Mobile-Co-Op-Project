using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidData : MonoBehaviour
{
    [SerializeField] private string _name; // �̸�+ID

    public string Name { get { return _name; } set { _name = value; } }

    [SerializeField] private int _totalDamage; // ���� �� ������

    public int TotalDamage { get { return _totalDamage; } set { _totalDamage = value; } }
}
