using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ItemG�� CharaterG�� �´� UI������ �� ������Ʈ Ǯ ���� �ʿ� - �� 10����
/// <summary>
/// ��í ������ ���̺� ���� �� ��û�� ����
/// 1. Gacha ��Ʈ���� Count ���� �� GachaReturn ��Ʈ���� Count ��� �������� Ȯ��
/// 2. GachaReturn ��Ʈ���� CharID�� ItemID�� �����ؼ� GachaID�� ����ϵ� ������ CharID�� ItemID�� ����
///    ���� Check(?)�� ĳ������ ��� 0 / �������� ��� 1 �� ǥ���� �������� Ȯ��
/// </summary>

[System.Serializable]
public class Lottery
{
    private int id; // �� ǰ�� ID
    public int Id { get { return id; } set { id = value; } }
    private int probability; // Ȯ��
    public int Probability { get { return probability; } set { probability = value; } }

}

[System.Serializable]
public class ItemG
{
    private int id;
    public int ID { get { return id; } set { id = value; }}
    private string name;
    public string Name { get { return name; } set { name = value; } }
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } set { sprite = value; } }
    private int count;
    public int Count { get { return count; } set { count = value; } }
}

[System.Serializable]
public class CharaterG
{
    private int id;
    public int ID { get { return id; } set { id = value; } }
    private string name;
    public string Name { get { return name; } set { name = value; } }
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } set { sprite = value; } }
    private int rarity;
    public int Rarity { get { return rarity; } set { rarity = value; } }
}
