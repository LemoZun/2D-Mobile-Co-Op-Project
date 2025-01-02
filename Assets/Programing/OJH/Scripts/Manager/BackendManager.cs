using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance { get; private set; }

    private FirebaseApp app;

    public static FirebaseApp App { get { return Instance.app; } }

    private FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return Instance.auth; } }

    private FirebaseDatabase database;

    public static FirebaseDatabase Database { get { return Instance.database; } }

    private void Awake()
    {
        CreateSingleton();
    }

    private void Start()
    {
        CheckDependency();
    }

    private void CreateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //ȣȯ�� ���� üũ
    private void CheckDependency()
    {
        // checkandfixDependenciesasync�� ��û, continuewithonmainTHread�� ����.
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            //����� ��밡���ϸ�
            if (task.Result == DependencyStatus.Available)
            {

                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                database.SetPersistenceEnabled(false);
                Debug.Log("Firebase dependencies check success");

            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");

                app = null;
                auth = null;
                database = null;
            }
        });
    }

}