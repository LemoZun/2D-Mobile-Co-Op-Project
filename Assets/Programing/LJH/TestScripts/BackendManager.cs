using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance;

    private FirebaseApp _app;
    public static FirebaseApp App { get { return Instance._app; } private set { Instance._app = value; } }
    private FirebaseAuth _auth;
    public static FirebaseAuth Auth { get { return Instance._auth; } private set { Instance._auth = value; } }

    private FirebaseDatabase database;

    public static FirebaseDatabase Database { get { return Instance.database; } }


    private Dictionary<string, object> _SettingDic = new Dictionary<string, object>();
    public static Dictionary<string, object> SettingDic { get { return Instance._SettingDic; } }
    private void Awake()
    {
        InitSingleTon();
        CheckDependency();
    }
    /// <summary>
    /// ȣȯ�� üũ
    /// </summary>
    private void CheckDependency()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().
            ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available) // ȣȯ ����?
                {
                    Debug.Log("BackendManager : ȣȯ üũ ����");
                    App = FirebaseApp.DefaultInstance;
                    Auth = FirebaseAuth.DefaultInstance;
                    database = FirebaseDatabase.DefaultInstance;
                }
                else
                {
                    App = null;
                    Auth = null;
                    database = null;
                }
            });
    }

    /// <summary>
    /// �̱��� ����
    /// </summary>
    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}