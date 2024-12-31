using Firebase.Database;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GachaBtn : MonoBehaviour
{
    GachaSceneController gachaSceneController;
    GachaCheck gachaCheck;

    [SerializeField] int gachaCost;
    [SerializeField] string gachaCostItem;
    [Header("GachaSceneController")]
    public List<GameObject> resultList = new List<GameObject>(); // �̱��� ����� ����

    // GachaSceneController�� csv�� ������ �����͸� �޾Ƽ� ���
    [Header("Gacha Lists")]
    private List<Gacha> baseGachaList = new List<Gacha>();
    private List<Gacha> eventGachaList = new List<Gacha>();

    private void Awake()
    {
        gachaSceneController = gameObject.GetComponent<GachaSceneController>();
        gachaCheck = gameObject.GetComponent<GachaCheck>();
    }
    /*
/// <summary>
/// Gacha�� ���� ����� Cost(DinoStone)����
/// - PlayerData.SetItem()�� ���� ���� �� ����
/// - ����� ���� firebase�� ����
/// </summary>
private void ChangeGachaCost()
{

}

/// <summary>
/// Gacha�� ����� ���Ͽ� ���氪�� ������ ����
/// - �������� ��� PlayerData.SetItem()�� ���� ���� �� ���� �� firebase�� ����
/// - ĳ������ ��� PlayerData�� UnitData�� �� ��
///     - �ߺ��Ǵ� ���
///       csvData�� GachaReturn�� ���Ͽ� ��ȯ �� ����
///     - �ߺ����� �ʴ� ���
///       UnitData�� ����
/// </summary>
private void ApplyGachaResult()
{

}
*/
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
                    // Debug.Log($"��ȯ�� GameObject : {baseGachaList[i].ItemId}");
                    break;
                }
            }
            // �������� �÷��̾��� ������ �� ����
            // firebase �⺻ UserData ��Ʈ
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");

            gachaCheck.SendChangeValue(gachaCostItem, gachaCost, root, PlayerDataManager.Instance.PlayerData);

            // �̱⿡ ������ ��ȭ�� PlayerData ����
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].gameObject.GetComponent<GachaItem>()) // GachaItem�� �����ϴ� Item�� ���
                {
                    gachaCheck.SendChangeValue(resultList[i].gameObject.GetComponent<GachaItem>().ItemName, 
                                               resultList[i].gameObject.GetComponent<GachaItem>().Amount,
                                               root, PlayerDataManager.Instance.PlayerData);
                }
                else if (resultList[i].GetComponent<GachaChar>()) // GachaChar�� �����ϴ� ĳ������ ���
                {
                    // PlayerData�� UnitDatas�� ������ ĳ���� ���̵� �ִ��� ���θ� Ȯ��
                    for(int j = 0; j < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; j++)
                    {
                        if (PlayerDataManager.Instance.PlayerData.UnitDatas[j].UnitId == resultList[i].gameObject.GetComponent<GachaChar>().CharId)
                        {
                            // TODO : �̹� ������ ĳ�����̹Ƿ� Ư�� ��ȭ�� ��ȯ�Ͽ� ��ȯ
                            Debug.Log("�̹� ������ ĳ����");
                            GameObject resultItem = gachaSceneController.CharReturnItem(resultList[i].gameObject.GetComponent<GachaChar>().CharId, resultList[i].gameObject);
                            gachaCheck.SendChangeValue(resultItem.gameObject.GetComponent<GachaItem>().ItemName,
                                                       resultItem.gameObject.GetComponent<GachaItem>().Amount,
                                                       root, PlayerDataManager.Instance.PlayerData);
                            break;
                        }
                        else
                        {
                            // TODO : ���� ĳ�����̹Ƿ� UnitId�� UnitLevel�� ����
                            Debug.Log("���� ĳ����");
                            break;
                        }
                    }
                }
            }
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
        baseGachaList = gachaSceneController.baseGachaList;
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
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost * 10, root, PlayerDataManager.Instance.PlayerData);

            // �̱⿡ ������ ��ȭ�� PlayerData ����
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].gameObject.GetComponent<GachaItem>()) // GachaItem�� �����ϴ� Item�� ���
                {
                    gachaCheck.SendChangeValue(resultList[i].gameObject.GetComponent<GachaItem>().ItemName,
                                               resultList[i].gameObject.GetComponent<GachaItem>().Amount,
                                               root, PlayerDataManager.Instance.PlayerData);
                }
                else if (resultList[i].GetComponent<GachaChar>()) // GachaChar�� �����ϴ� ĳ������ ���
                {
                    // PlayerData�� UnitDatas�� ������ ĳ���� ���̵� �ִ��� ���θ� Ȯ��
                    for (int j = 0; j < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; j++)
                    {
                        if (PlayerDataManager.Instance.PlayerData.UnitDatas[j].UnitId == resultList[i].gameObject.GetComponent<GachaChar>().CharId)
                        {
                            // TODO : �̹� ������ ĳ�����̹Ƿ� Ư�� ��ȭ�� ��ȯ�Ͽ� ��ȯ
                            Debug.Log("�̹� ������ ĳ����");
                            GameObject resultItem = gachaSceneController.CharReturnItem(resultList[i].gameObject.GetComponent<GachaChar>().CharId, resultList[i].gameObject);
                            gachaCheck.SendChangeValue(resultItem.gameObject.GetComponent<GachaItem>().ItemName,
                                                       resultItem.gameObject.GetComponent<GachaItem>().Amount,
                                                       root, PlayerDataManager.Instance.PlayerData);
                            break;
                        }
                        else
                        {
                            // TODO : ���� ĳ�����̹Ƿ� UnitId�� UnitLevel�� ����
                            Debug.Log("���� ĳ����");
                            break;
                        }
                    }
                }
            }
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
        eventGachaList = gachaSceneController.eventGachaList;
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
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost, root, PlayerDataManager.Instance.PlayerData);

            // �̱⿡ ������ ��ȭ�� PlayerData ����
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].gameObject.GetComponent<GachaItem>()) // GachaItem�� �����ϴ� Item�� ���
                {
                    gachaCheck.SendChangeValue(resultList[i].gameObject.GetComponent<GachaItem>().ItemName,
                                               resultList[i].gameObject.GetComponent<GachaItem>().Amount,
                                               root, PlayerDataManager.Instance.PlayerData);
                }
                else if (resultList[i].GetComponent<GachaChar>()) // GachaChar�� �����ϴ� ĳ������ ���
                {
                    // PlayerData�� UnitDatas�� ������ ĳ���� ���̵� �ִ��� ���θ� Ȯ��
                    for (int j = 0; j < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; j++)
                    {
                        if (PlayerDataManager.Instance.PlayerData.UnitDatas[j].UnitId == resultList[i].gameObject.GetComponent<GachaChar>().CharId)
                        {
                            // TODO : �̹� ������ ĳ�����̹Ƿ� Ư�� ��ȭ�� ��ȯ�Ͽ� ��ȯ
                            Debug.Log("�̹� ������ ĳ����");
                            GameObject resultItem = gachaSceneController.CharReturnItem(resultList[i].gameObject.GetComponent<GachaChar>().CharId, resultList[i].gameObject);
                            gachaCheck.SendChangeValue(resultItem.gameObject.GetComponent<GachaItem>().ItemName,
                                                       resultItem.gameObject.GetComponent<GachaItem>().Amount,
                                                       root, PlayerDataManager.Instance.PlayerData);
                            break;
                        }
                        else
                        {
                            // TODO : ���� ĳ�����̹Ƿ� UnitId�� UnitLevel�� ����
                            Debug.Log("���� ĳ����");
                            break;
                        }
                    }
                }
            }
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
        eventGachaList = gachaSceneController.eventGachaList;
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
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost * 10, root, PlayerDataManager.Instance.PlayerData);

            // �̱⿡ ������ ��ȭ�� PlayerData ����
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].gameObject.GetComponent<GachaItem>()) // GachaItem�� �����ϴ� Item�� ���
                {
                    gachaCheck.SendChangeValue(resultList[i].gameObject.GetComponent<GachaItem>().ItemName,
                                               resultList[i].gameObject.GetComponent<GachaItem>().Amount,
                                               root, PlayerDataManager.Instance.PlayerData);
                }
                else if (resultList[i].GetComponent<GachaChar>()) // GachaChar�� �����ϴ� ĳ������ ���
                {
                    // PlayerData�� UnitDatas�� ������ ĳ���� ���̵� �ִ��� ���θ� Ȯ��
                    for (int j = 0; j < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; j++)
                    {
                        if (PlayerDataManager.Instance.PlayerData.UnitDatas[j].UnitId == resultList[i].gameObject.GetComponent<GachaChar>().CharId)
                        {
                            // TODO : �̹� ������ ĳ�����̹Ƿ� Ư�� ��ȭ�� ��ȯ�Ͽ� ��ȯ
                            Debug.Log("�̹� ������ ĳ����");
                            GameObject resultItem = gachaSceneController.CharReturnItem(resultList[i].gameObject.GetComponent<GachaChar>().CharId, resultList[i].gameObject);
                            gachaCheck.SendChangeValue(resultItem.gameObject.GetComponent<GachaItem>().ItemName,
                                                       resultItem.gameObject.GetComponent<GachaItem>().Amount,
                                                       root, PlayerDataManager.Instance.PlayerData);
                            break;
                        }
                        else
                        {
                            // TODO : ���� ĳ�����̹Ƿ� UnitId�� UnitLevel�� ����
                            Debug.Log("���� ĳ����");
                            break;
                        }
                    }
                }
            }
            gachaSceneController.UpdatePlayerUI(); // UI ������Ʈ
        }
        else
        {
            Debug.Log("��ȭ �������� ���� �Ұ�");
            gachaSceneController.DisabledGachaResultPanel();
        }
    }
}



