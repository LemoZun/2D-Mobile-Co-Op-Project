using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnclockStageBtn : MonoBehaviour
{
    // ���̾� ���̽� ���� �ҷ��ͼ� ��ư ���ͷ��ͺ� ���� 
    [SerializeField] Button[] buttons;
    [SerializeField] bool[] stageClears;
    private void OnEnable()
    {
        for (int i = 1; i < buttons.Length; i++) 
        {
            if (stageClears[i] == false) 
            {
                buttons[i].interactable = false;
            }
        }
    }
}
