using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BackButtonManager : MonoBehaviour
{
    public static BackButtonManager Instance { get; private set; }

    private Stack<GameObject> _openPanels = new Stack<GameObject>();

    private SceneChanger _sceneChanger;

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
    }

    private void HandleBackButton()
    {
        // ���� �г��� ������ �ݱ�
        if (_openPanels.Count > 0)
        {
            CloseTopPanel();
        }
        // Lobby���� ��������
        else if (SceneManager.GetActiveScene().name == "Lobby_OJH")
        {
            // ���� �˾� UI ����ؼ� ���� ������ quit
            // Application.Quit();
            Debug.Log("���� ����");
        }
        // �ٸ� ������ �κ�� �̵�
        else if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            _sceneChanger.CanChangeSceen = true;
            _sceneChanger.ChangeScene("Lobby_OJH");
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        _openPanels.Push(panel);
    }

    private void CloseTopPanel()
    {
        if (_openPanels.Count > 0)
        {
            GameObject panel = _openPanels.Pop();
            panel.SetActive(false);
        }
    }

    public int OpenPanelCount => _openPanels.Count;
}