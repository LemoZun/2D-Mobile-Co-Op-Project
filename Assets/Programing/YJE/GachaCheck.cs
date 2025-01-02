using Firebase.Database;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaCheck : MonoBehaviour
{
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
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/0");
                    // Test��
                    //setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/0");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoBlood":
                    result = playerData.Items[(int)E_Item.DinoBlood] + amount;
                    playerData.SetItem((int)E_Item.DinoBlood, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                     setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/1");
                    // Test��
                    // setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/1");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "BoneCrystal":
                    result = playerData.Items[(int)E_Item.BoneCrystal] + amount;
                    playerData.SetItem((int)E_Item.BoneCrystal, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                     setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/2");
                    // Test��
                    // setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/2");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoStone":
                    result = playerData.Items[(int)E_Item.DinoStone] + amount;
                    playerData.SetItem((int)E_Item.DinoStone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/3");
                    // Test��
                    // setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/3");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "Stone":
                    result = playerData.Items[(int)E_Item.Stone] + amount;
                    playerData.SetItem((int)E_Item.Stone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/4");
                    // Test��
                    // setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/4");
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
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/0");
                    // Test��
                    //setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/0");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoBlood":
                    result = playerData.Items[(int)E_Item.DinoBlood] - amount;
                    playerData.SetItem((int)E_Item.DinoBlood, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/1");
                    // Test��
                    //setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/1");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "BoneCrystal":
                    result = playerData.Items[(int)E_Item.BoneCrystal] - amount;
                    playerData.SetItem((int)E_Item.BoneCrystal, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/2");
                    // Test��
                    //setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/2");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "DinoStone":
                    result = playerData.Items[(int)E_Item.DinoStone] - amount;
                    playerData.SetItem((int)E_Item.DinoStone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/3");
                    // Test��
                    //setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/3");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                case "Stone":
                    result = playerData.Items[(int)E_Item.Stone] - amount;
                    playerData.SetItem((int)E_Item.Stone, result);
                    // ���� ���� �� ��� - UserId�ҷ�����
                    setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/4");
                    // Test��
                    //setItemRoot = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/4");
                    setItemRoot.SetValueAsync(result); // firebase �� ����
                    break;
                default:
                    break;
            }
        }
        
    }
}
