using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContext : MonoBehaviour
{
    // ���� ���º��� �����س��� id�� csv ���� ���� �̸� ã�Ƽ� ���� ? 
    // ��Ʋ�� �Ŵ������ִ� ��ġ������ ����Ұ� 
    // ui �� �����ִ°� ����?
    // ������ �� �ֺ� �׸��� ���� �ٲ�� 
   
    // ���� : 0 ~ 8 �������� 4�� �߾����� �ΰ� ��� 
    private int[] Diagonal_2 = {-4,2};
    private int[] Type_D = {-3,-2,1,3,4};
    private int[] Bak_1 = {-1};
    private int[] T_Spine = { -3, -1, 1 };
    private int[] Diagonal_1 = {-2};
    private int[] Front_3 = {-2,1,4};
    private int[] Back_2 = {-1,2};

    


    
    public void selectGridBuff(string name,int curPos) // �̸� csv ���� �׸��� ���� �̸�  curPos �� 0~8 ���� ���� ��ġ -4 �� ���� ���� ��
                                                       //  ex ���� ��ġ 0  -4  = -4 
                                                       //               7  -4  = 3 
    {   
        
        switch (name)
        {
            case "Diagonal_2":
                applyGridBuff(curPos);
                    break;
            case "Type_D":

                    break;
            case "T_Spine":

                    break;
        }
    }
    public void applyGridBuff(int curPos) 
    {
        for (int i = 0; i < 9; i++) { }
    }

    private bool isInIndex(int[] PosList, int index)  // �迭 ���� �ȿ� �ε����� �������� �˻� , ���� ��ġ �迭
    {
        if (index >= 0 && PosList.Length > index) 
        {
            return true;  
        }
        else
            return false;

    }
}
