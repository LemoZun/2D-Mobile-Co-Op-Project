using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����۵� ( ��Ʈ���� �޾ƿ;� �ϳ�)
public class Item
{
    public int id;          // ID 500~
    public string name;     // �̸�
    public int amount;      // ������
    public Sprite icon;     // UI�� ǥ�õ� ������?
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