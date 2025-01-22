using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct RequiredItems
{
    public int Coin;
    public int DinoBlood;
    public int BoneCrystal;
}

public class LevelUpPanel : UIBInder
{
    private PlayerUnitData _targetCharacter;
    private int _maxLevelUp;
    private int _curLevelUp;
    private LevelUp _levelUpSystem;

    [SerializeField] private ItemPanel _itemPanel;

    private void Awake()
    {
        BindAll();
        AddEvent("LevelUpConfirm", EventType.Click, OnConfirmButtonClick);
        GetUI<Slider>("LevelUpSlider").onValueChanged.AddListener(OnSliderValueChanged);
        AddEvent("DecreaseButton", EventType.Click, OnDecreaseButtonClick);
        AddEvent("IncreaseButton", EventType.Click, OnIncreaseButtonClick);

        _levelUpSystem = new LevelUp(
            CsvDataManager.Instance.DataLists[(int)E_CsvData.CharacterLevelUp],
            CsvDataManager.Instance.DataLists[(int)E_CsvData.Character]
        );
    }

    // ������ �г� �ʱ�ȭ
    public void Init(PlayerUnitData character)
    {
        _targetCharacter = character;
        CalculateMaxLevelUp();

        Slider slider = GetUI<Slider>("LevelUpSlider");
        slider.minValue = 1;
        slider.maxValue = _maxLevelUp;
        slider.value = 1;

        _curLevelUp = 1;
        UpdateUI();
    }

    private void OnSliderValueChanged(float value)
    {
        _curLevelUp = Mathf.RoundToInt(value);
        UpdateUI();
    }

    // ������, ���� ���� ����
    private void UpdateUI()
    {
        RequiredItems items = _levelUpSystem.CalculateRequiredItems(_targetCharacter, _curLevelUp);
        bool canLevelUp = _levelUpSystem.CanLevelUp(_targetCharacter, _curLevelUp);

        UpdateItemTexts(items);
        UpdateLevelText();
        UpdateButtonStates(canLevelUp);
    }

    // ������ �䱸�� �ؽ�Ʈ ����
    private void UpdateItemTexts(RequiredItems items)
    {
        CheckItemAmount("Coin", items.Coin, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Coin]);
        CheckItemAmount("DinoBlood", items.DinoBlood, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.DinoBlood]);
        CheckItemAmount("BoneCrystal", items.BoneCrystal, PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.BoneCrystal]);
    }

    // ������ ���� Ȯ��
    private void CheckItemAmount(string itemName, int requiredAmount, int currentAmount)
    {
        string itemNameKor = itemName;
        switch (itemNameKor)
        {
            case "Coin": 
                itemNameKor = "����";
                break;
            case "DinoBlood": 
                itemNameKor = "���̳����";
                break;
            case "BoneCrystal":
                itemNameKor = "��ũ����Ż";
                break;
        }

        int shortage = requiredAmount - currentAmount;
        string text = shortage > 0 ? $"{itemNameKor} {shortage} ����" : $"{itemNameKor} : {requiredAmount}";
        GetUI<TextMeshProUGUI>($"{itemName}Text").text = text;
    }

    // ���� ����
    private void UpdateLevelText()
    {
        int newLevel = Mathf.Min(_targetCharacter.UnitLevel + _curLevelUp, LevelUp.MAXLEVEL);
        GetUI<TextMeshProUGUI>("LevelText").text = $"Lv.{_targetCharacter.UnitLevel} -> Lv.{newLevel}";
    }

    // ������ ��ư ���� ����
    private void UpdateButtonStates(bool canLevelUp)
    {
        GetUI<Button>("DecreaseButton").interactable = (_curLevelUp > 1);
        GetUI<Button>("IncreaseButton").interactable = (_curLevelUp < _maxLevelUp);
        GetUI<Button>("LevelUpConfirm").interactable = canLevelUp;
        GetUI<Slider>("LevelUpSlider").interactable = canLevelUp;
        GetUI<RectTransform>("Handle Slide Area").gameObject.SetActive(canLevelUp);
    }

    // ������ �� �� �ִ� �ִ�ġ ���
    private void CalculateMaxLevelUp()
    {
        _maxLevelUp = 0;
        while (_levelUpSystem.CanLevelUp(_targetCharacter, _maxLevelUp + 1))
        {
            _maxLevelUp++;
        }
    }

    // ������ ��ư ����
    private void OnConfirmButtonClick(PointerEventData eventData)
    {
        if (_levelUpSystem.CanLevelUp(_targetCharacter, _curLevelUp))
        {
            _levelUpSystem.PerformLevelUp(_targetCharacter, _curLevelUp);
            UpdateCharacters(_targetCharacter);
            gameObject.SetActive(false);
        }

        _itemPanel.UpdateItems();
    }

    // CharacterPanel, InventoryPanel�� �������� ���� ����
    private void UpdateCharacters(PlayerUnitData character)
    {
        FindObjectOfType<CharacterPanel>()?.UpdateCharacterInfo(character);
        FindObjectOfType<InventoryPanel>()?.UpdateCharacterUI(character);
    }

    // ���ҹ�ư
    private void OnDecreaseButtonClick(PointerEventData eventData)
    {
        if (_curLevelUp > 1)
        {
            _curLevelUp--;
            GetUI<Slider>("LevelUpSlider").value = _curLevelUp;
        }
    }

    // ������ư
    private void OnIncreaseButtonClick(PointerEventData eventData)
    {
        if (_curLevelUp < _maxLevelUp)
        {
            _curLevelUp++;
            GetUI<Slider>("LevelUpSlider").value = _curLevelUp;
        }
    }
}
