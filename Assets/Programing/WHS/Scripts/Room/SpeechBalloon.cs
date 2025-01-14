using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBalloon : MonoBehaviour
{
    [SerializeField] GameObject _speechBallon;
    [SerializeField] float _displayTime = 3f; // �������� �ð�
    [SerializeField] float _intervalTime = 2f; // ���� �ð�

    private bool _isDisplaying = false;

    private void Start()
    {
        if (_speechBallon != null)
        {
            _speechBallon.SetActive(false);
        }

        StartCoroutine(BalloonLoop());
    }

    private IEnumerator BalloonLoop()
    {
        while (true)
        {
            if (_speechBallon != null)
            {
                // 0.2�ʰ� �巯��
                yield return StartCoroutine(FadeSpeechBalloon(true, 0.2f));
                _isDisplaying = true;

                // disPlayTime���� ��ǳ�� ���̱�
                yield return new WaitForSeconds(_displayTime);

                // 0.2�ʰ� �����
                yield return StartCoroutine(FadeSpeechBalloon(false, 0.2f));
                _isDisplaying = false;

                // intervalTime���� ��ǳ�� �����
                yield return new WaitForSeconds(_intervalTime);
            }
        }
    }

    private IEnumerator FadeSpeechBalloon(bool fadeIn, float duration)
    {
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        float elapsedTime = 0f;

        CanvasGroup canvasGroup = _speechBallon.GetComponent<CanvasGroup>();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        _speechBallon.SetActive(fadeIn);
    }
}
