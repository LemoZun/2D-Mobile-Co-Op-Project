using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager _instance;
    public static PlayerDataManager Instance { get { return _instance; } set { _instance = value; } }

    [SerializeField] private PlayerData _playerData;

    public PlayerData PlayerData {  get { return _playerData; } private set { } }

    private int[] _housingIDs = new int[(int)E_Item.Length];

    private static int[] _itemIDs = { 500, 501, 502, 530, 504 };

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        //if (_instance == null)
            return;

        //UpdateItemValue();
    }

    // ��ȭ�� 1�д� �����ϴ� ������ �ҷ����� ���� HOusing ID ã���� ����.
    // LobbyPanel Start���� ȣ��.
    public void LoadHousingIDs()
    {
        Dictionary<int, Dictionary<string, string>> itemDic = CsvDataManager.Instance.DataLists[(int)E_CsvData.Item];

        //Item�� HousingID ����.
        for(int i = 0; i < (int)E_Item.Length; i++)
        {
            _housingIDs[i] = TypeCastManager.Instance.TryParseInt(itemDic[_itemIDs[i]]["HousingID"]);
        }
    }

    public void CheckCleaerStage()
    {

    }

    private void UpdateItemValue()
    {
        //ù �ε����� �α��ξ������� ����.
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
            return;

        //Player stageŬ���� ���� ���� Ȯ���ϱ�

       
        
    }



}
