using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RnDStageCSV
{
    // ����� Stage�� ������
    // CSV���Ͽ��� �ҷ��� ���� ���缭 �˸��� �ڷ����� �Բ� ����
    public int id {  get; set; }
    public string stageName { get; set; }
    public int timeLimit { get; set; }
    // public bool isCleared { get; set; }
    public int monsterCount { get; set; }
    public int monsterPos { get; set; }
}
