using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ItemG�� CharaterG�� �´� UI������ �� ������Ʈ Ǯ ���� �ʿ� - �� 10����

[System.Serializable]
public class Lottery
{
    private int id; // �� ǰ�� ID
    private int probability; // Ȯ��
}

[System.Serializable]
public class ItemG
{
    private int id;
    private string name;
    private Sprite sprite;
    private int count;
}

[System.Serializable]
public class CharaterG
{
    private int id;
    private string name;
    private Sprite sprite;
    private int rarity;
}
