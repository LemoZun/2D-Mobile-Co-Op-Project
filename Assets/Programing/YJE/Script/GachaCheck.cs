using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : ���� �� BackendManager.Auth.CurrentUser.UserId �ּ� �����ϰ� �׽�Ʈ �ڵ� �ּ�ó�� �ʼ�

public class GachaCheck : MonoBehaviour
{
    GachaSceneController gachaSceneController;

    private void Awake()
    {
        gachaSceneController = gameObject.GetComponent<GachaSceneController>();
    }

    /// <summary>
    /// itemName�� ���� switch������ �б��Ͽ� �˸��� root��ġ�� playerData�� �����ϴ� �Լ�
    /// - add : false �̸� ���� / ture�̸� ����
    //  - GachaBtn.cs���� ���
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="amount"></param>
    /// <param name="root"></param>
    /// <param name="playerData"></param>
    public void SendChangeValue(string itemName, int amount, bool add, DatabaseReference root, PlayerData playerData)
    {
        int result = 0;
        DatabaseReference setItemRoot;
        if (add)
        {
            switch (itemName)
            {
                case "Coin":
                    result = playerData.Items[(int)E_Item.Coin] + amount;
                    playerData.SetItem((int)E_Item.Coin, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/0");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/0");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoBlood":
                    result = playerData.Items[(int)E_Item.DinoBlood] + amount;
                    playerData.SetItem((int)E_Item.DinoBlood, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/1");
                    // Test��
                     setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/1");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "BoneCrystal":
                    result = playerData.Items[(int)E_Item.BoneCrystal] + amount;
                    playerData.SetItem((int)E_Item.BoneCrystal, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/2");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/2");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoStone":
                    result = playerData.Items[(int)E_Item.DinoStone] + amount;
                    playerData.SetItem((int)E_Item.DinoStone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/3");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/3");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "Stone":
                    result = playerData.Items[(int)E_Item.Stone] + amount;
                    playerData.SetItem((int)E_Item.Stone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/4");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/4");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                default:
                    break;
            }
        }
        else if (!add)
        {
            switch (itemName)
            {
                case "Coin":
                    result = playerData.Items[(int)E_Item.Coin] - amount;
                    playerData.SetItem((int)E_Item.Coin, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/0");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/0");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoBlood":
                    result = playerData.Items[(int)E_Item.DinoBlood] - amount;
                    playerData.SetItem((int)E_Item.DinoBlood, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/1");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/1");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "BoneCrystal":
                    result = playerData.Items[(int)E_Item.BoneCrystal] - amount;
                    playerData.SetItem((int)E_Item.BoneCrystal, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/2");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/2");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoStone":
                    result = playerData.Items[(int)E_Item.DinoStone] - amount;
                    playerData.SetItem((int)E_Item.DinoStone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/3");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/3");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "Stone":
                    result = playerData.Items[(int)E_Item.Stone] - amount;
                    playerData.SetItem((int)E_Item.Stone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    // setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/4");
                    // Test��
                    setItemRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_items/4");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                default:
                    break;
            }
        }
        
    }

    /// <summary>
    /// Character�� �ߺ��� CharId�� ���ؼ� Ȯ���ϰ� ������ Character�� ���ο� ���� �˸��� ������ ����
    //  - GachaBtn.cs���� ���
    /// </summary>
    public void CheckCharId(List<GameObject> resultList, DatabaseReference root, PlayerData playerData)
    {
        for (int i = 0; i < resultList.Count; i++)
        {
            if (resultList[i].GetComponent<GachaItem>()) // GachaItem�� �����ϴ� Item�� ���
            {
                SendChangeValue(resultList[i].gameObject.GetComponent<GachaItem>().ItemName,
                                           resultList[i].gameObject.GetComponent<GachaItem>().Amount, true,
                                           root, playerData);
            }
            else if (resultList[i].GetComponent<GachaChar>()) // GachaChar�� �����ϴ� ĳ������ ���
            {

                int index = -1;

                for (int j = 0; j < playerData.UnitDatas.Count; j++)
                {
                    if (resultList[i].GetComponent<GachaChar>().CharId == playerData.UnitDatas[j].UnitId)
                    {
                        index = j;
                    }
                }
                // PlayerData�� UnitDatas�� ������ ĳ���� ���̵� �ִ��� ���θ� Ȯ��
                if (index == -1)
                {
                    Debug.Log("���� ĳ����");
                    // ���ο� Unit�� ����
                    PlayerUnitData newUnit = new PlayerUnitData();
                    newUnit.UnitId = resultList[i].GetComponent<GachaChar>().CharId;
                    newUnit.UnitLevel = 1;
                    playerData.UnitDatas.Add(newUnit);
                    // ���� ���� �� ��� - UserId�ҷ����� 
                    // DatabaseReference unitRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_unitDatas");
                    // Test ��
                     DatabaseReference unitRoot = root.Child("CHSmbrwghYNzZb7AIkdLRtvpHaW2").Child("_unitDatas");

                    for (int num = 0; num < playerData.UnitDatas.Count; num++)
                    {
                        PlayerUnitData nowData = new PlayerUnitData();
                        nowData.UnitId = playerData.UnitDatas[num].UnitId;
                        nowData.UnitLevel = playerData.UnitDatas[num].UnitLevel;
                        unitRoot.Child($"{num}/_unitId").SetValueAsync(nowData.UnitId);
                        unitRoot.Child($"{num}/_unitLevel").SetValueAsync(nowData.UnitLevel);
                    }
                }
                else
                {
                    Debug.Log("�̹� ������ ĳ����");
                    GameObject resultItem = gachaSceneController.CharReturnItem(resultList[i].gameObject.GetComponent<GachaChar>().CharId, resultList[i].gameObject);
                    SendChangeValue(resultItem.gameObject.GetComponent<GachaItem>().ItemName,
                                               resultItem.gameObject.GetComponent<GachaItem>().Amount, true,
                                               root, playerData);
                }
            }

        }
    }
}
