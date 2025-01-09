using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BackButtonManager : MonoBehaviour
{
    public static BackButtonManager Instance { get; private set; }

    private Stack<Action> _backActions = new Stack<Action>();

    private SceneChanger _sceneChanger;

    public int BackActionCount => _backActions.Count;
    [SerializeField] private int _backActionCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _sceneChanger = FindObjectOfType<SceneChanger>();
    }

    private void Update()
    {
        // ESC��ư (������ �ڷΰ��� ���)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleBackButton();
        }

        _backActionCount = _backActions.Count;
    }

    private void HandleBackButton()
    {
        // backAction ������ ������ �Ѱ� �����
        if (_backActions.Count > 0)
        {
            _backActions.Pop().Invoke();
        }
        // Lobby���� ��������
        else if (SceneManager.GetActiveScene().name == "Lobby_OJH")
        {
            Application.Quit();
        }
        // �ٸ� ������ �κ�� �̵�
        else if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            _sceneChanger.CanChangeSceen = true;
            _sceneChanger.ChangeScene("Lobby_OJH");
        }
    }

    // UIâ�� �� �� backAction ���� �߰��ϱ�
    public void AddBackAction(Action action)
    {
        _backActions.Push(action);
    }

    private void ShowExitConfirmation()
    {
        // �� ���� Ȯ�� �˾� ���
        Debug.Log("������ �����Ͻ÷ƴϱ�? Application.Quit(); ");
    }
}