using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����۵� ( ��Ʈ���� �޾ƿ;� �ϳ�)
public class Item
{
    public int ItemID;          // ID 500~
    public string ItemName;     // �̸�
    public int amount;          // ������
}

public static class ItemID
{
    public const int Coin = 500;
    public const int DinoBlood = 501;
    public const int BoneCrystal = 502;
    public const int DinoStone = 503;
    public const int LeapStone = 504;
    public const int Gear = 505;
}