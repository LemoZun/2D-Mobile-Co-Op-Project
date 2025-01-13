using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �̹� ���� ĳ���͸� �̾��� ���
// ���������� ��ȯ�Ͽ� ����ϱ� ���� Ŭ����
public class GachaReturn : MonoBehaviour
{
    private int itemId; // ��ȯ�� ������ID
    public int ItemId { get { return itemId; } set { itemId = value; } }
    private int count; // ��ȯ�� ����
    public int Count { get { return count; } set { count = value; } }

    /// <summary>
    /// DB�� ����� Return�� ���� ������ �����ϴ� �Լ�
    /// - ShopMakeStart.cs�� MakeCharReturnItemDic() ���
    /// </summary>
    /// <param name="dataBaseList"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public GachaReturn SetReturnInfo(Dictionary<int, Dictionary<string, string>> dataBaseList, int i)
    {
        GachaReturn gachaItemReturn = new GachaReturn();
        gachaItemReturn.ItemId = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["ItemID"]);
        gachaItemReturn.Count = TypeCastManager.Instance.TryParseInt(dataBaseList[i]["Count"]);

        return gachaItemReturn;
    }


}
