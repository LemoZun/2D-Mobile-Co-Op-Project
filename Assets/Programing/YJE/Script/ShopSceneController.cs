using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSceneController : UIBInder
{
    ShopBtnManager shopBtnManager;

    [Header("UI")]
    [SerializeField] GameObject resultCharPrefab; // ����� ĳ������ ��� ����� ������
    [SerializeField] GameObject resultItemPrefab; // ����� �������� ��� ����� ������
    [SerializeField] RectTransform returnContent; // �ߺ�ĳ���� ������ ��ȯ �������� ���� �� ��ġ
    [SerializeField] GameObject returnPrefab; // �ߺ�ĳ���� ������ ��ȯ ������

    private void Awake()
    {
        shopBtnManager = gameObject.GetComponent<ShopBtnManager>();
        BindAll();
        ShowBaseGachaPanel();
        SettingBtn();
    }

    /// <summary>
    /// ���� �� ��ư�� ���� ����
    /// - ��ư�� ���� ���� ����
    //  - LoadingCheck.cs���� �̺�Ʈ�� ���
    /// </summary>
    public void SettingStartUI()
    {
        // �� Button �ؽ�Ʈ ����
        GetUI<TextMeshProUGUI>("BaseSingleText").SetText("1ȸ �̱�");
        GetUI<TextMeshProUGUI>("BaseTenText").SetText("10ȸ �̱�");
        GetUI<TextMeshProUGUI>("ChangeBaseGacahText").SetText("��");
        GetUI<TextMeshProUGUI>("ChangeShopText").SetText("����");
        GetUI<TextMeshProUGUI>("GachaSingleCostText").SetText($"{shopBtnManager.GachaCost}");
        GetUI<TextMeshProUGUI>("GachaTenCostText").SetText($"{shopBtnManager.GachaCost * 10}");
        UpdatePlayerUI();
    }

    /// <summary>
    /// �� Item ��ȭ ��� ǥ��
    /// - ���� �� ��� ������Ʈ�� �ʿ��ϹǷ� �Լ��� �����Ͽ� ���
    /// </summary>
    public void UpdatePlayerUI()
    {
        GetUI<TextMeshProUGUI>("CoinText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin].ToString());
        GetUI<TextMeshProUGUI>("DinoBloodText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood].ToString());
        GetUI<TextMeshProUGUI>("BoneCrystalText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal].ToString());
        GetUI<TextMeshProUGUI>("DinoStoneText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone].ToString());
        GetUI<TextMeshProUGUI>("StoneText").SetText(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Stone].ToString());
    }

    private void SettingBtn()
    {
        // ����г� ��ư Ŭ�� �� �г� ��Ȱ��ȭ �Լ� ����
        GetUI<Button>("SingleResultPanel").onClick.AddListener(shopBtnManager.OnDisableGachaPanelBtn);
        GetUI<Button>("TenResultPanel").onClick.AddListener(shopBtnManager.OnDisableGachaPanelBtn);
        // GachaBtn ��ũ��Ʈ�� �� ��ư�� �Լ� ����
        GetUI<Button>("BaseSingleBtn").onClick.AddListener(shopBtnManager.OnBaseSingleBtn);
        GetUI<Button>("BaseTenBtn").onClick.AddListener(shopBtnManager.OnBaseTenBtn);
        // Lobby�� ���ư��� ��ư �Լ� ����
        GetUI<Button>("BackBtn").onClick.AddListener(shopBtnManager.OnBackToRobby);
        // ���� Ȱ��ȭ ��ư ����
        GetUI<Button>("ChangeShopBtn").onClick.AddListener(shopBtnManager.OnShopBtn);
        // Gacha ���� ���� ��ư �Լ� ����
        GetUI<Button>("ChangeBaseGachaBtn").onClick.AddListener(shopBtnManager.OnBaseGachaBtn);
    }

    /// <summary>
    /// BaseGachaPanel Ȱ��ȭ
    /// </summary>
    public void ShowBaseGachaPanel()
    {
        // �⺻ �г� Ȱ��ȭ
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(true);
        // ���ư��� ��ư Ȱ��ȭ
        GetUI<Image>("BackBtn").gameObject.SetActive(true);
        // ���� ĳ���� Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
        // ��í ���� ��ư Ȱ��ȭ
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        // ���� �г� ��Ȱ��ȭ
        GetUI<Image>("ShopPanel").gameObject.SetActive(false);
        // ��í ��� �г� ��Ȱ��ȭ
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        // �ε� �г� ��Ȱ��ȭ
        GetUI<Image>("LoadingPanel").gameObject.SetActive(false);
        // �ߺ� ���� �˾�
        GetUI<Image>("BuyPopUp").gameObject.SetActive(false);
    }
    public void ShowShopPanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(false);
        GetUI<Image>("ShopPanel").gameObject.SetActive(true);
    }
    /// <summary>
    /// ��í 1���� �г� Ȱ��ȭ
    /// </summary>
    public void ShowSingleResultPanel()
    {
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(true);
        GetUI<Image>("SingleResultPanel").gameObject.SetActive(true);
        GetUI<Image>("SingleImage").gameObject.SetActive(true);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(false);
    }
    public void DisableSingleImage()
    {
        GetUI<Image>("SingleImage").gameObject.SetActive(false);
    }
    public void ShowTenResultPanel()
    {
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(true);
        GetUI<Image>("SingleResultPanel").gameObject.SetActive(false);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(true);
        GetUI<Image>("TenImage").gameObject.SetActive(true);
    }
    public void DisableTenImage()
    {
        GetUI<Image>("TenImage").gameObject.SetActive(false);
    }
    /// <summary>
    /// GachaResultPanel ��Ȱ��ȭ
    /// - ��� �г��� ��Ȱ��ȭ
    /// </summary>
    public void DisabledGachaResultPanel()
    {
        // �⺻ �̱� ���� ���� ��ư Ȱ��ȭ
        GetUI<Image>("ChangeBaseGachaBtn").gameObject.SetActive(true);
        // ��� �г� ��Ȱ��ȭ
        GetUI<Image>("GachaResultPanel").gameObject.SetActive(false);
        GetUI<Image>("SingleResultPanel").gameObject.SetActive(false);
        GetUI<Image>("TenResultPanel").gameObject.SetActive(false);
        // �� ������ ��ȭ Text Ȱ��ȭ
        GetUI<TextMeshProUGUI>("CoinText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("DinoBloodText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("BoneCrystalText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("DinoStoneText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("StoneText").gameObject.SetActive(true);
        // ���� ĳ���� Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
        // ���ư��� ��ư Ȱ��ȭ
        GetUI<Image>("BackBtn").gameObject.SetActive(true);

    }

    /// <summary>
    /// ���� ���� �� �ߺ� ���� ���� �˾�â
    /// ShopChar.cs���� ���
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowBuyOverlapPopUp()
    {
        Debug.Log("�˾�â �ڷ�ƾ ����");
        GetUI<Image>("BuyPopUp").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("OverlapPopUpText").gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        GetUI<Image>("BuyPopUp").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("OverlapPopUpText").gameObject.SetActive(false);
    }
}
