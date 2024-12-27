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

    [SerializeField] private int _giftCoin;

    public int GiftCoin { get { return _giftCoin; } set { _giftCoin = value; } }


    [SerializeField] private int _canFollow;

    public int CanFollow { get { return _canFollow; } set { _canFollow = value; } }

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

    [SerializeField] private int[] _storedItems;

    public int[] StoredItems { get { return _storedItems; } private set { } }

    public void SetStoredItem(int index, int value)
    {
        if (index >= 0 && index < _storedItems.Length)
        {
            _storedItems[index] = value;
            OnStoredItemChanged[index]?.Invoke(value);
        }
    }

    [SerializeField] private int[] _unitPos;

    public int[]  UnitPos{ get { return _unitPos; } private set { } }

    [SerializeField] private bool[] _isStageClear;   //�������� Ŭ���� ����

    public bool[] IsStageClear { get { return _isStageClear; } private set { } }

    [SerializeField] private List<string> _followingIds;

    public List<string> FollowingIds { get { return _followingIds; } private set { } }

    [SerializeField] private List<PlayerUnitData> _unitDatas;

    public List<PlayerUnitData> UnitDatas { get { return _unitDatas; } private set { } }

    public UnityAction<int>[] OnItemChanged = new UnityAction<int>[(int)E_Item.Length];

    public UnityAction<int>[] OnStoredItemChanged = new UnityAction<int>[(int)E_Item.Length];
}
