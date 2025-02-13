using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterLock : MonoBehaviour
{
    [SerializeField] Button[] chapterButtons;
    [SerializeField] SceneChanger _sceneChanger;


    [SerializeField] private AudioClip _bgmClip;
    [SerializeField] private AudioClip buttonSfx;

    private void OnEnable()
    {
        SoundManager.Instance.PlayeBGM(_bgmClip);

        if (PlayerDataManager.Instance.PlayerData.IsStageClear[6] == false)
        {
            chapterButtons[0].interactable = true;
            chapterButtons[1].interactable = false;
            chapterButtons[2].interactable = false;
        }
        else if (PlayerDataManager.Instance.PlayerData.IsStageClear[13] == false)
        {
            chapterButtons[0].interactable = true;
            chapterButtons[1].interactable = true;
            chapterButtons[2].interactable = false;
        }
    }
    public void GoLobby()
    {
        SoundManager.Instance.PlaySFX(buttonSfx);
        _sceneChanger.CanChangeSceen = true;
        _sceneChanger.ChangeScene("Lobby_OJH");
    }


    // 각각 스테이지로
    public void GoZero()
    {
        SoundManager.Instance.PlaySFX(buttonSfx);
        _sceneChanger.CanChangeSceen = true;
        _sceneChanger.ChangeScene("ChapterZero_LJH");
    }
    public void GoFirst()
    {
        SoundManager.Instance.PlaySFX(buttonSfx);
        _sceneChanger.CanChangeSceen = true;
        _sceneChanger.ChangeScene("ChapterFirst_LJH");
    }
    public void GoSecond()
    {
        SoundManager.Instance.PlaySFX(buttonSfx);
        _sceneChanger.CanChangeSceen = true;
        _sceneChanger.ChangeScene("ChapterSecond_LJH");
    }
}
