using Firebase.Database;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class GachaBtn : MonoBehaviour
{
    GachaSceneController gachaSceneController;

    [Header("GachaSceneController")]
    public List<GameObject> resultList = new List<GameObject>(); // �̱��� ����� ����

    // GachaSceneController�� csv�� ������ �����͸� �޾Ƽ� ���
    [Header("Gacha Lists")]
    private List<Gacha> baseGachaList = new List<Gacha>();
    private List<Gacha> eventGachaList = new List<Gacha>();

    private void Awake()
    {
        gachaSceneController = gameObject.GetComponent<GachaSceneController>();
    }

    /// <summary>
    /// ��� �г� ��Ȱ��ȭ ��
    /// resultList �� �ʱ�ȭ
    //  - GachaSceneController.cs���� ���
    /// </summary>
    public void ClearResultList()
    {
        for (int i = 0; i < resultList.Count; i++)
        {
            Destroy(resultList[i]);
        }
        resultList.Clear();
    }

    /// <summary>
    /// �⺻ 1���� ��ư ���� ��
    /// - baseGachaList�� ����� Ȯ���� ���
    /// - baseGachaList�� ����� Ȯ���� ���
    /// </summary>
    public void BaseSingleBtn()
    {
        baseGachaList = gachaSceneController.baseGachaList;
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= 100)
        {
            // baseGachaList�� ��ü Probability�� �ջ��� ���ϱ�
            int total = 0;
            for (int i = 0; i < baseGachaList.Count; i++)
            {
                total += baseGachaList[i].Probability;
            }

            int weight = 0;
            int selectNum = 0;
            selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f)); // ���� ���� �̱�
            gachaSceneController.ShowSingleResultPanel(); // 1���� ��� �г� Ȱ��ȭ

            for (int i = 0; i < baseGachaList.Count; i++)
            {
                weight += baseGachaList[i].Probability;
                if (selectNum <= weight) // ����ġ�� ���ڸ� ��
                {
                    // �����۰� ĳ���Ϳ� ���� ����� ���
                    // GachaSceneController.cs�� GachaResultUI()�� ��ȯ�� GameObject�� resultList�� ����
                    GameObject resultUI = gachaSceneController.GachaSingleResultUI(baseGachaList, i);
                    resultList.Add(resultUI);
                    Debug.Log($"��ȯ�� GameObject : {baseGachaList[i].ItemId}");
                    break;
                }
            }
            /*
            // �̱⿡ ����� ��ȭ�� PlayerData ����
            int DSValue = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] - 100;
            // �÷��̾� �������� �� ����
            PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.DinoStone, DSValue);
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData"); // firebase �⺻ UserData ��Ʈ
            // ���� ���� �� ��� - UserId�ҷ�����
            // DatabaseReference items = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/3");
            // Test��
            DatabaseReference items = root.Child("Y29oJ7Tu2RQr0SZlbgYzZcDz5Xb2").Child("_items/3");
            items.SetValueAsync(DSValue); // firebase �� ����
            gachaSceneController.UpdatePlayerUI(); // UI ������Ʈ

            // �̱⿡ ������ ��ȭ�� PlayerData ����
            */
        }
        else
        {
            Debug.Log("��ȭ �������� ���� �Ұ�");
            gachaSceneController.DisabledGachaResultPanel();
        }
        
    }

    /// <summary>
    /// �⺻ 10���� ��ư ���� ��
    /// - baseGachaList�� ����� Ȯ���� ���
    /// - baseGachaList�� ����� Ȯ���� ���
    /// </summary>
    public void BaseTenBtn()
    {
        baseGachaList = gachaSceneController.baseGachaList;
        // baseGachaList�� ��ü Probability�� �ջ��� ���ϱ�
        int total = 0;
        for (int i = 0; i < baseGachaList.Count; i++)
        {
            total += baseGachaList[i].Probability;
        }

        gachaSceneController.ShowTenResultPanel(); // 10���� ����г� Ȱ��ȭ

        int weight = 0; // ���� ��ġ�� ����ġ
        int selectNum = 0; // ������ ���� ��ȣ
        int count = 0; // �� 10���� ȸ���� ī���� �ϴ� ����

        do
        {
            selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

            // ��í�� ����Ʈ�� Ƚ�� ��ŭ �ݺ��ϸ� ����ġ�� �ش��ϴ� ��� ���
            for (int i = 0; i < baseGachaList.Count; i++)
            {
                weight += baseGachaList[i].Probability;
                if (selectNum <= weight)
                {
                    // �����۰� ĳ���Ϳ� ���� ����� ���
                    // GachaSceneController.cs�� GachaResultUI()�� ��ȯ�� GameObject�� resultList�� ����
                    GameObject resultUI = gachaSceneController.GachaTenResultUI(baseGachaList, i);
                    resultList.Add(resultUI);
                    Debug.Log($"��ȯ�� GameObject : {baseGachaList[i].ItemId}");
                    count++;
                    weight = 0;
                    break;
                }
            }
        } while (count < 10);
    }

    /// <summary>
    /// �̺�Ʈ 1���� ��ư ���� ��
    /// - eventGachaList�� ����� Ȯ���� ���
    /// - eventGachaList�� ����� Ȯ���� ���
    /// </summary>
    public void EventSingleBtn()
    {
        eventGachaList = gachaSceneController.eventGachaList;
        // eventGachaList�� ��ü Probability�� �ջ��� ���ϱ�
        int total = 0;
        for (int i = 0; i < eventGachaList.Count; i++)
        {
            total += eventGachaList[i].Probability;
        }

        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f)); // ���� ���� �̱�
        gachaSceneController.ShowSingleResultPanel(); // 1���� ��� �г� Ȱ��ȭ

        for (int i = 0; i < eventGachaList.Count; i++)
        {
            weight += eventGachaList[i].Probability;
            if (selectNum <= weight) // ����ġ�� ���ڸ� ��
            {
                // �����۰� ĳ���Ϳ� ���� ����� ���
                // GachaSceneController.cs�� GachaSingleResultUI()�� ��ȯ�� GameObject�� resultList�� ����
                GameObject resultUI = gachaSceneController.GachaSingleResultUI(eventGachaList, i);
                resultList.Add(resultUI);
                Debug.Log($"��ȯ�� GameObject : {eventGachaList[i].ItemId}");
                break;
            }
        }

    }

    /// <summary>
    /// �̺�Ʈ 10���� ��ư ���� ��
    /// - eventGachaList�� ����� Ȯ���� ���
    /// - eventGachaList�� ����� Ȯ���� ���
    /// </summary>
    public void EventTenBtn()
    {
        eventGachaList = gachaSceneController.eventGachaList;
        int total = 0;
        for (int i = 0; i < eventGachaList.Count; i++)
        {
            total += eventGachaList[i].Probability;
        }
        gachaSceneController.ShowTenResultPanel(); // 10���� ����г� Ȱ��ȭ

        int weight = 0; // ���� ��ġ�� ����ġ
        int selectNum = 0; // ������ ���� ��ȣ
        int count = 0; // �� 10���� ȸ���� ī���� �ϴ� ����

        do
        {
            selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

            // ��í�� ����Ʈ�� Ƚ�� ��ŭ �ݺ��ϸ� ����ġ�� �ش��ϴ� ��� ���
            for (int i = 0; i < eventGachaList.Count; i++)
            {
                weight += eventGachaList[i].Probability;
                if (selectNum <= weight)
                {
                    // �����۰� ĳ���Ϳ� ���� ����� ���
                    // GachaSceneController.cs�� GachaTenResultUI�� ��ȯ�� GameObject�� resultList�� ����
                    GameObject resultUI = gachaSceneController.GachaTenResultUI(eventGachaList, i);
                    resultList.Add(resultUI);
                    Debug.Log($"��ȯ�� GameObject : {eventGachaList[i].ItemId}");
                    count++;
                    weight = 0;
                    break;
                }
            }
        } while (count < 10);
    }
}



