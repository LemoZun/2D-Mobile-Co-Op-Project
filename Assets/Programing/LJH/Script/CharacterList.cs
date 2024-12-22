using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{   // ĳ���� ��� ���� �� ���� �ֱ� 
    
    [SerializeField] string[] charaList; // �ӽ� ������ , ���߿� ĳ���� ������ �޾ƿ;� �� 
    [SerializeField] GameObject slot; // ������
    
    
    private void OnEnable()
    {
        for (int i = 0; i < charaList.Length; i++) 
        {
            GameObject obj =  Instantiate(slot, new Vector3(100+150 * i,125, 0),Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.GetComponent<TestSlot>().setTexttest(charaList[i]);
           
            
        }
    }
}
