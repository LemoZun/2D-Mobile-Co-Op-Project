using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSceneController : UIBInder
{
    ShopBtnManager shopBtnManager;
    [SerializeField] AudioClip shopBgm;
    [SerializeField] AudioClip buttonSfx;
    public AudioClip ButtonSfx { get { return buttonSfx; } set { buttonSfx = value; } }

    [Header("UI")]
    [SerializeField] GameObject resultCharPrefab; // ����� ĳ������ ��� ����� ������
    [SerializeField] GameObject resultItemPrefab; // ����� �������� ��� ����� ������
    [SerializeField] RectTransform returnContent; // �ߺ�ĳ���� ������ ��ȯ �������� ���� �� ��ġ
    [SerializeField] GameObject returnPrefab; // �ߺ�ĳ���� ������ ��ȯ ������

    private void Awake()
    {
        shopBtnManager = gameObject.GetComponent<ShopBtnManager>();
        BindAll();
        SoundBgm();
        ShowBaseGachaPanel();
        SettingBtn();
    }

    private void OnEnable()
    {
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Coin] += UpdateCoinUI;
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoBlood] += UpdateDinoBloodUI;
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.BoneCrystal] += UpdateBoneCrystalUI;
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoStone] += UpdateDinoStoneUI;
        PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Stone] += UpdateStoneUI;

    }
    private void OnDisable()
    {
        if (PlayerDataManager.Instance != null && PlayerDataManager.Instance.PlayerData != null)
        {
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Coin] -= UpdateCoinUI;
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoBlood] -= UpdateDinoBloodUI;
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.BoneCrystal] -= UpdateBoneCrystalUI;
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.DinoStone] -= UpdateDinoStoneUI;
            PlayerDataManager.Instance.PlayerData.OnItemChanged[(int)E_Item.Stone] -= UpdateStoneUI;
        }
    }

    /// <summary>
    /// BGM ��� �Լ�
    /// - ShopBtnManager.cs���� ���
    /// </summary>
    public void SoundBgm()
    {
        SoundManager.Instance.PlayeBGM(shopBgm);
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
        UpdatePlayerData();
    }
    
    /// <summary>
    /// �� Item ��ȭ ��� ǥ��
    /// - ���� �� ��� ������Ʈ�� �ʿ��ϹǷ� �Լ��� �����Ͽ� ���
    /// </summary>
    public void UpdatePlayerData()
    {
        UpdateCoinUI(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin]);
        UpdateDinoBloodUI(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood]);
        UpdateBoneCrystalUI(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal]);
        UpdateDinoStoneUI(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoStone]);
        UpdateStoneUI(PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Stone]);
    }
    
    private void UpdateCoinUI(int value)
    {
        GetUI<TextMeshProUGUI>("CoinText").SetText(value.ToString());
    }
    private void UpdateDinoBloodUI(int value)
    {
        GetUI<TextMeshProUGUI>("DinoBloodText").SetText(value.ToString());
    }
    private void UpdateBoneCrystalUI(int value)
    {
        GetUI<TextMeshProUGUI>("BoneCrystalText").SetText(value.ToString());
    }
    private void UpdateDinoStoneUI(int value)
    {
        GetUI<TextMeshProUGUI>("DinoStoneText").SetText(value.ToString());
    }
    private void UpdateStoneUI(int value)
    {
        GetUI<TextMeshProUGUI>("StoneText").SetText(value.ToString());
    }

    private void SettingBtn()
    {
        // ����г� ��ư Ŭ�� �� �г� ��Ȱ��ȭ �Լ� ����
        GetUI<Button>("SingleResultPanel").onClick.AddListener(shopBtnManager.OnDisableGachaPanelBtn);
        GetUI<Button>("TenResultPanel").onClick.AddListener(shopBtnManager.OnDisableGachaPanelBtn);
        // GachaBtn ��ũ��Ʈ�� �� ��ư�� �Լ� ����
        GetUI<Button>("BaseSingleBtn").onClick.AddListener(shopBtnManager.OnBaseSingleBtn);
        GetUI<Button>("BaseSingleBtn").onClick.AddListener(() => SoundManager.Instance.PlaySFX(buttonSfx));
        GetUI<Button>("BaseTenBtn").onClick.AddListener(shopBtnManager.OnBaseTenBtn);
        GetUI<Button>("BaseTenBtn").onClick.AddListener(() => SoundManager.Instance.PlaySFX(buttonSfx));
        // Lobby�� ���ư��� ��ư �Լ� ����
        GetUI<Button>("BackBtn").onClick.AddListener(shopBtnManager.OnBackToRobby);
        GetUI<Button>("BackBtn").onClick.AddListener(() => SoundManager.Instance.PlaySFX(buttonSfx));
        // ���� Ȱ��ȭ ��ư ����
        GetUI<Button>("ChangeShopBtn").onClick.AddListener(shopBtnManager.OnShopBtn);
        GetUI<Button>("ChangeShopBtn").onClick.AddListener(() => SoundManager.Instance.PlaySFX(buttonSfx));
        // �⺻ Gacha ���� ��ư �Լ� ����
        GetUI<Button>("ChangeBaseGachaBtn").onClick.AddListener(shopBtnManager.OnBaseGachaBtn);
        GetUI<Button>("ChangeBaseGachaBtn").onClick.AddListener(() => SoundManager.Instance.PlaySFX(buttonSfx));
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
        // ����NPC Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
    }
    public void ShowShopPanel()
    {
        GetUI<Image>("BaseGachaPanel").gameObject.SetActive(false);
        GetUI<Image>("ShopPanel").gameObject.SetActive(true);
        // ����NPC Ȱ��ȭ
        GetUI<Image>("ShopCharacter").gameObject.SetActive(true);
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

        SoundManager.Instance.StopBGM();

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

        SoundManager.Instance.StopBGM();
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
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowBuyOverlapPopUp()
    {
        GetUI<Image>("BuyPopUp").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("OverlapBuyPopUpText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("OverlapGachaPopUpText").gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        GetUI<Image>("BuyPopUp").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("OverlapBuyPopUpText").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("OverlapGachaPopUpText").gameObject.SetActive(false);
    }

    /// <summary>
    /// ��í �õ� �� ��ȭ���� �ȳ�â
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowGachaOverlapPopUp()
    {
        GetUI<Image>("BuyPopUp").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("OverlapGachaPopUpText").gameObject.SetActive(true);
        GetUI<TextMeshProUGUI>("OverlapBuyPopUpText").gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        GetUI<Image>("BuyPopUp").gameObject.SetActive(false);
        GetUI<TextMeshProUGUI>("OverlapGachaPopUpText").gameObject.SetActive(false);
    }
}
