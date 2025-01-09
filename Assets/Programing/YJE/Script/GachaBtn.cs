using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBtn : MonoBehaviour
{
    GachaSceneController gachaSceneController;
    GachaCheck gachaCheck;
    SceneChanger sceneChanger;

    private RectTransform singleVideoContent; // 1���� ��� ���� �������� ���� �� ��ġ
    private RectTransform tenVideoContent; // 10���� ��� ���� �������� ���� �� ��ġ

    // ��í ��ȭ ���� ������ ���� ���� - �ν�����â���� ���ϰ� ���� ����
    [SerializeField] int gachaCost;
    [SerializeField] string gachaCostItem;

    [Header("GachaSceneController")]
    private List<GameObject> resultList = new List<GameObject>(); // �̱��� ����� ����

    // GachaSceneController�� csv�� ������ �����͸� �޾Ƽ� ���
    [Header("Gacha Lists")]
    private List<Gacha> baseGachaList = new List<Gacha>();

    private void Awake()
    {
        gachaSceneController = gameObject.GetComponent<GachaSceneController>();
        gachaCheck = gameObject.GetComponent<GachaCheck>();
        sceneChanger = gameObject.GetComponent<SceneChanger>();
        singleVideoContent = gachaSceneController.GetUI<RectTransform>("SingleResultPanel");
        tenVideoContent = gachaSceneController.GetUI<RectTransform>("TenResultPanel");
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
        GameObject resultUI = null;
        // �⺻ �÷��̾��� ��ȭ DinoStone(3)�� 100 �̻��� ��쿡�� ����
        // TODO : ���� ��ȭ�� ��ģ ���� �ʿ� - ���� ���̳뽺�� ������ �߰�
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= gachaCost)
        {
            // baseGachaList�� ��ü Probability�� �ջ��� ���ϱ�
            int total = 0;
            foreach (Gacha gacha in baseGachaList)
            {
                total += gacha.Probability;
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
                    resultUI = gachaSceneController.GachaSingleResultUI(baseGachaList, i);
                    resultList.Add(resultUI);
                    StartCoroutine(CharacterVideoR(resultUI)); // ��í ��ƾ ����
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
            // UI ������Ʈ
            gachaSceneController.UpdatePlayerUI();
            StopCoroutine(CharacterVideoR(resultUI)); // ��í ��ƾ ����
            //gachaSceneController.DisableSingleImage();
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
        GameObject resultUI = null;
        // �⺻ �÷��̾��� ��ȭ DinoStone(3)�� 1000 �̻��� ��쿡�� ����
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone] >= gachaCost * 10)
        {
            // baseGachaList�� ��ü Probability�� �ջ��� ���ϱ�
            int total = 0;
            foreach (Gacha gacha in baseGachaList)
            {
                total += gacha.Probability;
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
                        resultUI = gachaSceneController.GachaTenResultUI(baseGachaList, i);
                        resultList.Add(resultUI);
                        count++;
                        weight = 0;
                        break;
                    }
                }
            } while (count < 10);
            StartCoroutine(CharacterTenVideoR());
            // �̱⿡ ����� ��ȭ�� PlayerData ����
            DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");
            gachaCheck.SendChangeValue(gachaCostItem, gachaCost * 10, false, root, PlayerDataManager.Instance.PlayerData);
            // ��� ����Ʈ�� ���� �˸��� �����۰� ĳ���� ��ȯ�� Ȯ���ϰ� ������ ����
            gachaCheck.CheckCharId(resultList, root, PlayerDataManager.Instance.PlayerData);
            // UI ������Ʈ
            gachaSceneController.UpdatePlayerUI();
        }
        else
        {
            Debug.Log("��ȭ �������� ���� �Ұ�");
            gachaSceneController.DisabledGachaResultPanel();
        }
        StopCoroutine(CharacterTenVideoR());
    }

    /// <summary>
    /// ��í�� ĳ���� �̱� �� ������ ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator CharacterVideoR(GameObject gameObj)
    {
        if (gameObj.GetComponent<GachaChar>())
        {
            GameObject obj = Instantiate(gameObj.GetComponent<GachaChar>().Video, singleVideoContent);
            obj.SetActive(true);
            yield return new WaitUntil(() => obj.gameObject == false);
        }
        gachaSceneController.DisableSingleImage();
    }

    /// <summary>
    /// ��í�� ĳ���� 10ȸ �̱� �� ������ ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator CharacterTenVideoR()
    {
        foreach (GameObject gameObj in resultList)
        {
            if (gameObj.GetComponent<GachaChar>())
            {
                GameObject obj = Instantiate(gameObj.GetComponent<GachaChar>().Video, tenVideoContent);
                obj.SetActive(true);
                yield return new WaitUntil(() => obj.gameObject == false);
                continue;
            }
            else
            {
                continue;
            }
        }
        gachaSceneController.DisableTenImage();
    }
}



