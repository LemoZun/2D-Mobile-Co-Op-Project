using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;

public class GachaBtn : MonoBehaviour
{
    GachaSceneController gachaSceneController;
    GachaCheck gachaCheck;
    SceneChanger sceneChanger;

    // ��í ��ȭ ���� ������ ���� ���� - �ν�����â���� ���� ����
    [SerializeField] int gachaCost;
    [SerializeField] string gachaCostItem;

    [Header("GachaSceneController")]
    private List<GameObject> resultList = new List<GameObject>(); // �̱��� ����� ����

    // GachaSceneController�� csv�� ������ �����͸� �޾Ƽ� ���
    [Header("Gacha Lists")]
    private List<Gacha> baseGachaList = new List<Gacha>();
    private List<Gacha> eventGachaList = new List<Gacha>();

    private void Awake()
    {
        gachaSceneController = gameObject.GetComponent<GachaSceneController>();
        gachaCheck = gameObject.GetComponent<GachaCheck>();
        sceneChanger = gameObject.GetComponent<SceneChanger>();
    }

    public void BackToRobby()
    {
        sceneChanger.CanChangeSceen = true;
        sceneChanger.ChangeScene("Lobby_OJH");
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
        baseGachaList = gachaSceneController.BaseGachaList;
        // �⺻ �÷��̾��� ��ȭ DinoStone(3)�� 100 �̻��� ��쿡�� ����
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= gachaCost)
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
                    break;
                }
            }
            // �������� �÷��̾��� ������ �� ����
            // firebase �⺻ UserData ��Ʈ
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");

            // �̱⿡ ������ ��ȭ�� PlayerData ����
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost, false, root, PlayerDataManager.Instance.PlayerData);

            // ��� ����Ʈ�� ���� �˸��� �����۰� ĳ���� ��ȯ�� Ȯ���ϰ� ������ ����
            gachaCheck.CheckCharId(resultList, root, PlayerDataManager.Instance.PlayerData);
           
            gachaSceneController.UpdatePlayerUI(); // UI ������Ʈ
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
        baseGachaList = gachaSceneController.BaseGachaList;
        // �⺻ �÷��̾��� ��ȭ DinoStone(3)�� 1000 �̻��� ��쿡�� ����
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= gachaCost * 10)
        {
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
            // �̱⿡ ����� ��ȭ�� PlayerData ����
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost * 10, false, root, PlayerDataManager.Instance.PlayerData);

            // ��� ����Ʈ�� ���� �˸��� �����۰� ĳ���� ��ȯ�� Ȯ���ϰ� ������ ����
            gachaCheck.CheckCharId(resultList, root, PlayerDataManager.Instance.PlayerData);
            
            gachaSceneController.UpdatePlayerUI(); // UI ������Ʈ
        }
        else
        {
            Debug.Log("��ȭ �������� ���� �Ұ�");
            gachaSceneController.DisabledGachaResultPanel();
        }


    }


    /// <summary>
    /// �̺�Ʈ 1���� ��ư ���� ��
    /// - eventGachaList�� ����� Ȯ���� ���
    /// - eventGachaList�� ����� Ȯ���� ���
    /// </summary>
    public void EventSingleBtn()
    {
        eventGachaList = gachaSceneController.EventGachaList;
        // �⺻ �÷��̾��� ��ȭ DinoStone(3)�� 100 �̻��� ��쿡�� ����
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= gachaCost)
        {
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
            // �������� �÷��̾��� ������ �� ����
            // firebase �⺻ UserData ��Ʈ
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost, false, root, PlayerDataManager.Instance.PlayerData);

            // ��� ����Ʈ�� ���� �˸��� �����۰� ĳ���� ��ȯ�� Ȯ���ϰ� ������ ����
            gachaCheck.CheckCharId(resultList, root, PlayerDataManager.Instance.PlayerData);
            gachaSceneController.UpdatePlayerUI(); // UI ������Ʈ
        }
        else
        {
            Debug.Log("��ȭ �������� ���� �Ұ�");
            gachaSceneController.DisabledGachaResultPanel();
        }
    }

    /// <summary>
    /// �̺�Ʈ 10���� ��ư ���� ��
    /// - eventGachaList�� ����� Ȯ���� ���
    /// - eventGachaList�� ����� Ȯ���� ���
    /// </summary>
    public void EventTenBtn()
    {
        eventGachaList = gachaSceneController.EventGachaList;
        // �⺻ �÷��̾��� ��ȭ DinoStone(3)�� 1000 �̻��� ��쿡�� ����
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= gachaCost * 10)
        {
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
            // �̱⿡ ����� ��ȭ�� PlayerData ����
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost * 10, false, root, PlayerDataManager.Instance.PlayerData);

            // ��� ����Ʈ�� ���� �˸��� �����۰� ĳ���� ��ȯ�� Ȯ���ϰ� ������ ����
            gachaCheck.CheckCharId(resultList, root, PlayerDataManager.Instance.PlayerData);
            gachaSceneController.UpdatePlayerUI(); // UI ������Ʈ
        }
        else
        {
            Debug.Log("��ȭ �������� ���� �Ұ�");
            gachaSceneController.DisabledGachaResultPanel();
        }
    }
}



