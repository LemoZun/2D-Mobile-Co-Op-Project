using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;


public enum E_CsvData { Character, Element, Stat, CharacterSkill, CharacterLevelUp,
    Monster, Stages, StageReward, MontserGroup, Item, Gacha, GachaReturn, Raids, RaidReward, WeeklyReward, Housing}

public class CsvDataManager : MonoBehaviour
{
    private static CsvDataManager _instance;
    public static CsvDataManager Instance { get { return _instance; } set { _instance = value; } }

    //csv ��ũ��
    [SerializeField] private string[] _urls;

    //csvData��
    [SerializeField] private string[] _csvDatas;

    //csvData Parsing�� Data��
    public Dictionary<int, Dictionary<string, string>>[] DataLists { get; set; }

    UnityWebRequest _request;

    private Coroutine _downLoadRoutine;

    private bool _isLoad;

    public bool IsLoad { get { return _isLoad; } private set { } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _downLoadRoutine = StartCoroutine(DownloadRoutine());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DownloadRoutine()
    {
        // Data �ʱ�ȭ 
        DataLists = new Dictionary<int, Dictionary<string, string>>[_csvDatas.Length];
        for (int i = 0; i < DataLists.Length; i++)
        {
            DataLists[i] = new Dictionary<int, Dictionary<string, string>>();
        }

        for (int i = 0; i < _urls.Length; i++)
        {
            _request = UnityWebRequest.Get(_urls[i]);

            // ��û �� ���ϴٿ�ε� �Ϸ���� ���
            yield return _request.SendWebRequest();

            //�ٿ�ε� �Ϸ� �� string�� ����.
            _csvDatas[i] = _request.downloadHandler.text;

            //Parsing���� List�� ����.
            DataLists[i] = ChangeCsvToList(_csvDatas[i]);
        }

        _isLoad = true;
    }

    Dictionary< int, Dictionary<string, string>> ChangeCsvToList(string data)
    {
        Dictionary<int, Dictionary<string, string>> dataList = new Dictionary<int, Dictionary<string, string>>();

        string[] lines = data.Split('\n');

        // CSV ù ���� ���
        string[] headers = lines[0].Split(',');

        // CSV ������ �Ľ�
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(",");
            Dictionary<string, string> dataDic = new Dictionary<string, string>();

            //id�� �����ϰ� �����Ӽ����͸� ���ؼ� 1����
            for (int j = 1; j < headers.Length; j++)
            {
                dataDic[headers[j].Trim()] = values[j].Trim();
            }

            //values[0]�� 
            dataList[TypeCastManager.Instance.TryParseInt(values[0])] = dataDic;
        }

        return dataList;
    }


}
