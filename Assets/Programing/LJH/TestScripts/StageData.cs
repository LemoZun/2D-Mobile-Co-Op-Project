using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageData")]
public class StageData : ScriptableObject 
{
    public string stageName; // �������� �̸�
    public float timeLimit; // ���ѽð� 
    public bool isClear; // Ŭ���� ����


    // �������� ��ȣ, ������ ��ġ , � �������� , ������ ����(����������ȣ�� ���󰡸� ������?) 
    // � ������ � ���Ͱ� ��ġ�Ǿ��ִ����� ����� ������(����? , )�� �޾Ƽ� ����� �����ұ� ? 

}
