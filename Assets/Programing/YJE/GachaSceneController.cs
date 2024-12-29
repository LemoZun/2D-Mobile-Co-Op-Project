using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO : 
// 1. �� �гκ� ��Ȱ��ȭ�� �ʿ��� ��� �Լ� ����
// 2. �̺�Ʈ�� Ȱ���Ͽ� LoadingCheck ��ũ��Ʈ�� ���� Setting ��������
// 3. resultList�� ����� �̱⸦ resultPrefab�� �˸��� ��ġ�� ������ �־ ����ϵ��� �Լ� �����ϱ�

/// <summary>
/// GachaScene�� ��ü���� ������ �ϴ� ��ũ��Ʈ
/// - CsvDataManager�� ����
/// - PlayData�� ����
/// - UIBInder�� ����Ͽ� �̺�Ʈ ���� �� �˸°� �̺�Ʈ�� �� UI�� Ȱ��ȭ ����
/// </summary>
public class GachaSceneController : UIBInder
{
    private bool isLoading = false;
    public bool IsLoading { get { return isLoading; } set { isLoading = value; } }

    // csvDataManager.cs���� ������ Ư�� DataList�� ���� Disctionary
    Dictionary<int, Dictionary<string, string>> gachaList = new Dictionary<int, Dictionary<string, string>>();

    List<GameObject> resultList = new List<GameObject>(); // �̱��� ����� ����

    [Header("Gacha Lists")]
    [SerializeField] public List<Gacha> baseGachaList = new List<Gacha>();
    [SerializeField] public List<Gacha> eventGachaList = new List<Gacha>();

    [Header("UI")]
    [SerializeField] RectTransform resultContent; // 10���� ��� ���� �������� ���� �� ��ġ
    [SerializeField] GameObject resultPrefab; // 10���� ��� ���� ������
    [SerializeField] GameObject resultPanel; // 10���� ��� â

    private void Awake()
    {
        BindAll();
        SettingStartUI();
        SettingStartPanel();

    }

    /// <summary>
    /// �׽�Ʈ�� ���� Update�� ����
    /// - ���� ���� �� LoadingCheck ��ũ��Ʈ���� �Ǵ� �� �̺�Ʈ�� ����ϱ�
    /// </summary>
    private void Update()
    {
        if (CsvDataManager.Instance.IsLoad && !isLoading)
        {
            MakeGachaList();
            SettingBtn();
        }
        else if (isLoading)
        {
            return;
        }
    }

    /// <summary>
    /// csv�����ͷ� �˸��� ���� ����Ʈ�� �и��ϴ� �Լ�
    /// - ���ο� ��í ������ ����Ʈ�� �߰��Ϸ��� ���
    ///     1. csv ���Ͽ� GachaGroup�� ��� ���� ����
    ///     2. LoadingCheck ��ũ��Ʈ �տ� GachaGroup�� ������ŭ ����Ʈ ����
    ///     2. �Լ��� switch���� ���ο� case�� GachaGroup �б��� ����
    ///     3. �� GachaGroup�� ����Ʈ �ʱ�ȭ
    /// </summary>
    private void MakeGachaList()
    {
        gachaList = CsvDataManager.Instance.DataLists[(int)E_CsvData.Gacha]; // csv�����ͷ� ��í����Ʈ ��������
        for (int i = 1; i < gachaList.Count; i++)
        {
            Debug.Log(gachaList[i]["Check"]);
            Gacha gachatem = new Gacha();
            gachatem.Check = TypeCastManager.Instance.TryParseInt(gachaList[i]["Check"]);
            switch (gachatem.Check) // ������ Ȯ��
            {
                case 1: // ������ Item�� ���
                    gachatem.ItemId = TypeCastManager.Instance.TryParseInt(gachaList[i]["ItemID"]);
                    break;
                case 2: // ������ Character�� ���
                    gachatem.CharId = TypeCastManager.Instance.TryParseInt(gachaList[i]["CharID"]);
                    break;
                default:
                    break;
            }
            gachatem.Probability = TypeCastManager.Instance.TryParseInt(gachaList[i]["Probability"]); // Ȯ�� ����
            gachatem.Count = TypeCastManager.Instance.TryParseInt(gachaList[i]["Count"]); // ��ȯ ���� ����

            switch (gachaList[i]["GachaGroup"]) // GachaGroup�� Ȯ���Ͽ� List�� ����
            {
                case "1":
                    baseGachaList.Add(gachatem);
                    break;
                case "2":
                    eventGachaList.Add(gachatem);
                    break;
                default:
                    break;
            }

        }
        // ��� ����Ʈ ����
        isLoading = true;
    }

    /// <summary>
    /// UI��ư����
    /// </summary>
    private void SettingBtn()
    {
        GetUI<Button>("TenResultPanel").onClick.AddListener(DisabledGachaResultPanel);
        GetUI<Button>("BaseSingleBtn").onClick.AddListener(BaseSingleBtn);
        GetUI<Button>("BaseTenBtn").onClick.AddListener(BaseTenBtn);
        GetUI<Button>("EventSingleBtn").onClick.AddListener(EventSingleBtn);
        GetUI<Button>("EventTenBtn").onClick.AddListener(EventTenBtn);

    }

    /// <summary>
    /// ���� �� �г� Ȱ��ȭ�� ��Ȱ��ȭ ����
    /// </summary>
    private void SettingStartPanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(false);
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ChangeEventGachaBtn").gameObject.SetActive(true);
        GetUI<Image>("ShopCharacter").gameObject.SetActive(false);
    }
    /// <summary>
    /// ���� �� ��ư�� ���� ����
    /// </summary>
    private void SettingStartUI()
    {
        GetUI<TextMeshProUGUI>("BaseSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("BaseTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("EventSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("EventTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("ChangeBaseGacahText").SetText("��");
        GetUI<TextMeshProUGUI>("ChangeEventGacahText").SetText("�̺�Ʈ");
    }

    /// <summary>
    /// BaseGachaPanel Ȱ��ȭ
    /// </summary>
    private void ShowBaseGachaPanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
    }
    /// <summary>
    /// EventGachaPanel Ȱ��ȭ
    /// </summary>
    private void ShowEventGachaPanel()
    {
        GetUI<Image>("EventGachaPanel").gameObject.SetActive(true);
    }
    /// <summary>
    /// GachaResultPanel Ȱ��ȭ
    /// </summary>
    private void ShowGachaResultPanel()
    {
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(true);
    }
    /// <summary>
    /// SingleResultPanel Ȱ��ȭ
    /// </summary>
    private void ShowSingleResultPanel()
    {
        GetUI<Image>("SingleResultPanell").gameObject.SetActive(true);
    }
    /// <summary>
    /// TenResultPanel Ȱ��ȭ
    /// </summary>
    private void ShowTenResultPanel()
    {
        GetUI<Image>("TenResultPanel").gameObject.SetActive(true);
    }
    /// <summary>
    /// GachaResultPanel ��Ȱ��ȭ
    /// - ��� ���� ����Ʈ�� �ʱ�ȭ
    /// - ��� �г��� ��Ȱ��ȭ
    /// </summary>
    private void DisabledGachaResultPanel()
    {
        resultList.Clear();
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        GetUI<Image>("SingleResultPanell").gameObject.SetActive(false);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(false);
    }




    /// <summary>
    /// �⺻ 1���� ��ư ���� ��
    /// - baseGachaList�� ����� Ȯ���� ���
    /// - baseGachaList�� ����� Ȯ���� ���
    /// </summary>
    private void BaseSingleBtn()
    {
        int total = 0;
        for (int i = 0; i < baseGachaList.Count; i++)
        {
            total += baseGachaList[i].Probability;
        }

        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        for (int i = 0; i < baseGachaList.Count; i++)
        {
            weight += baseGachaList[i].Probability;
            if (selectNum <= weight)
            {
                if (baseGachaList[i].Check == 1)
                {
                    resultList.Add(Instantiate(resultPrefab, resultContent));
                    Debug.Log("��ȯ�� ������ : " + baseGachaList[i].ItemId);
                }
                else if (baseGachaList[i].Check == 0)
                {
                    resultList.Add(Instantiate(resultPrefab, resultContent));
                    Debug.Log("��ȯ�� ������ : " + baseGachaList[i].CharId);
                }

                break;
            }
        }
    }
    /// <summary>
    /// �⺻ 10���� ��ư ���� ��
    /// - baseGachaList�� ����� Ȯ���� ���
    /// - baseGachaList�� ����� Ȯ���� ���
    /// </summary>
    private void BaseTenBtn()
    {
        int total = 0;
        for (int i = 0; i < baseGachaList.Count; i++)
        {
            total += baseGachaList[i].Probability;
        }
        ShowGachaResultPanel();
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
                    Debug.Log(baseGachaList[i].ItemId);
                    resultList.Add(Instantiate(resultPrefab, resultContent));
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
    private void EventSingleBtn()
    {
        int total = 0;
        for (int i = 0; i < eventGachaList.Count; i++)
        {
            total += eventGachaList[i].Probability;
        }

        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        for (int i = 0; i < eventGachaList.Count; i++)
        {
            weight += eventGachaList[i].Probability;
            if (selectNum <= weight)
            {
                if (eventGachaList[i].Check == 1)
                {
                    resultList.Add(Instantiate(resultPrefab, resultContent));
                    Debug.Log("��ȯ�� ������ : " + eventGachaList[i].ItemId);
                }
                else if (eventGachaList[i].Check == 0)
                {

                    resultList.Add(Instantiate(resultPrefab, resultContent));
                    Debug.Log("��ȯ�� ������ : " + eventGachaList[i].CharId);
                }

                break;
            }
        }
    }
    /// <summary>
    /// �̺�Ʈ 10���� ��ư ���� ��
    /// - eventGachaList�� ����� Ȯ���� ���
    /// - eventGachaList�� ����� Ȯ���� ���
    /// </summary>
    private void EventTenBtn()
    {
        int total = 0;
        for (int i = 0; i < eventGachaList.Count; i++)
        {
            total += eventGachaList[i].Probability;
        }
        ShowGachaResultPanel();
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
                    Debug.Log(eventGachaList[i].ItemId);
                    resultList.Add(Instantiate(resultPrefab, resultContent));
                    count++;
                    weight = 0;
                    break;
                }
            }
        } while (count < 10);
    }
}
