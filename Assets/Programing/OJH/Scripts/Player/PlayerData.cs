using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum E_Item { Coin, DinoBlood, BoneCrystal, DinoStone, Stone, Length }

// ���� �������ʿ��� ���������� �ٷ� �ʿ��� �͵鸸 �ּ������� ����. 
[System.Serializable]
public class PlayerData : MonoBehaviour
{
    [SerializeField] private string _playerName;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }


    [SerializeField] private string _exitTime; // ��������ð� -> ���� ��ġ�������ʿ��� ����� ����.

    public string ExitTime { get { return _exitTime; } set { _exitTime = value; } }

    [SerializeField] private int[] _items;

    public int[] Items { get { return _items; } private set { } }

    [SerializeField] private bool[] _isStageClear;   //�������� Ŭ���� ����

    public bool[] IsStageClear { get { return _isStageClear; } private set { } }

    [SerializeField] private List<PlayerUnitData> _unitDatas;

    public List<PlayerUnitData> UnitDatas { get { return _unitDatas; } private set { } }


}
