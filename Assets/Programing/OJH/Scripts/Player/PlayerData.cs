using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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

    public int[] Items { get { return _items; } private set {} }

    public void SetItem(int index, int value)
    {
        if(index >= 0 && index < _items.Length)
        {
            _items[index] = value;
            OnItemChanged[index]?.Invoke(value);
        }
    }

    [SerializeField] private int[] _unitPos;

    public int[]  UnitPos{ get { return _unitPos; } private set { } }

    [SerializeField] private bool[] _isStageClear;   //�������� Ŭ���� ����

    public bool[] IsStageClear { get { return _isStageClear; } private set { } }

    [SerializeField] private List<PlayerUnitData> _unitDatas;

    public List<PlayerUnitData> UnitDatas { get { return _unitDatas; } private set { } }

    public UnityAction<int>[] OnItemChanged;

}
