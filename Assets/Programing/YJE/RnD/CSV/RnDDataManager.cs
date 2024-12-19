using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


// �˾Ƶα�
// inspector â���� �ִ� _urls�� ������ ������ /edit~ �κ��� /export?=format=csv �������� ��ȯ�Ͽ� ���
// ex)
// https://docs.google.com/spreadsheets/d/1KXfqH_i8RFvsEWVfqmSbnVuuLuMm6GvW3kF6nt5iMI4/edit?gid=0#gid=0 �� url��
// https://docs.google.com/spreadsheets/d/1KXfqH_i8RFvsEWVfqmSbnVuuLuMm6GvW3kF6nt5iMI4/export?=format=csv �� ����

public class RnDDataManager : MonoBehaviour
{
    private static RnDDataManager _instance;
    public static RnDDataManager Instance { get { return _instance; } set { _instance = value; } }

    //csv ��ũ��
    [SerializeField] private string[] _urls;

    //csvData��
    [SerializeField] private string[] _csvDatas;

    //csvData Parsing�� Data��
    public List<Dictionary<string, string>>[] DataLists { get; set; }

    UnityWebRequest _request;

    private Coroutine _downLoadRoutine;

    private bool _isLoad;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            if (_isLoad == false)
            {
                _downLoadRoutine = StartCoroutine(DownloadRoutine());
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DownloadRoutine()
    {
        _isLoad = true;

        // Data �ʱ�ȭ 
        DataLists = new List<Dictionary<string, string>>[_csvDatas.Length];
        for (int i = 0; i < DataLists.Length; i++)
        {
            DataLists[i] = new List<Dictionary<string, string>>();
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
    }

    List<Dictionary<string, string>> ChangeCsvToList(string data)
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        string[] lines = data.Split('\n');

        Debug.Log(lines.Length);

        // CSV ù ���� ���
        string[] headers = lines[0].Split(',');

        // CSV ������ �Ľ�
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            Dictionary<string, string> dataDic = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                dataDic[headers[j].Trim()] = values[j].Trim();
            }

            dataList.Add(dataDic);
        }

        //foreach (var expando in dataList)
        //{
        //    foreach (var pair in expando)
        //    {
        //        Debug.Log($"{pair.Key}: {pair.Value}");
        //    }
        //}

        return dataList;
    }




}
