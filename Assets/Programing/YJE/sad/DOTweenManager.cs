using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public static class DOTweenTMPExtensions
{
    // TextMeshPro �ؽ�Ʈ�� ������ �ִϸ��̼����� �����ϴ� Ȯ�� �޼���
    public static Tweener DOText(this TMP_Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
    {
        int startValueLength = target.text.Length; // ���� �ؽ�Ʈ ���� (������� �ʴ� ��� ���� ����)
        return DOTween.To(() => target.text, x => target.text = x, endValue, duration)
            .SetOptions(richTextEnabled, scrambleMode, scrambleChars)
            .SetTarget(target);
    }
}

public class DOTweenManager : MonoBehaviour
{
    // UI �̹����� (�ִϸ��̼��� ������ ���)
    public Image testImage1;
    public Image testImage2;
    public Image testImage3;
    public Image testImage4;
    public Image testImage5;

    public RectTransform testRect1;

    // DOTween ������
    public Sequence testSequence;

    // ���� �߰��� ���� �ʿ��� ����
    public AudioSource audioSource; // ���� ����� ���� AudioSource
    public AudioClip clip1;         // ù ��° ���� Ŭ��
    public AudioClip clip2;         // �� ��° ���� Ŭ��

    // ���� ���� �� ȣ��Ǵ� �޼���
    void Start()
    {
        MakeSequence(); // ������ �ʱ�ȭ
        DOVirtual.DelayedCall(2f, () => testSequence.Play()); // 2�� �� �ڵ����� �������� ����
    }

    // �������� �����ϰ� �ִϸ��̼� ����
    public void MakeSequence()
    {
        // DOTween ������ �ʱ�ȭ �� ���� ���·� ����
        testSequence = DOTween.Sequence();
        testSequence.Pause();

        // 5���� ��� �ð� �߰�
        testSequence.AppendInterval(5f);

        // �̹��� ���̵� �ƿ� (0.5��)
        testSequence.Join(testImage5.DOFade(0.0f, 0.5f));

        // �̹������� ��ġ �̵� �ִϸ��̼� ����
        // �� �̹����� ������ ��ġ�� �̵� (3�� ����)
        testSequence.Join(testImage2.rectTransform.DOAnchorPos(new Vector2(0, 0), 3));
        testSequence.Join(testImage3.rectTransform.DOAnchorPos(new Vector2(0, 0), 3));
        testSequence.Join(testImage4.rectTransform.DOAnchorPos(new Vector2(0, 0), 3));

        // 0.5���� ��� �ð� �߰�
        testSequence.AppendInterval(0.5f);

        // ��� �׸� �̵� (1�� ����)
        testSequence.Join(testImage1.rectTransform.DOAnchorPos(new Vector2(0, 0), 1));

        // 0.1���� ��� �ð� �߰�
        testSequence.AppendInterval(0.1f);

        // ��� �׸� Shake �ִϸ��̼� �߰� (0.5�� ����, ������ 10, ���� Ƚ���� 10)
        testSequence.Append(testImage1.rectTransform.DOShakeAnchorPos(0.5f, 10f, 10, 90, false, true));

        // ���� ��� �ִϸ��̼� �߰�
        // ù ��° ���� Ŭ�� ��� (�ִϸ��̼� ���� ��)
        testSequence.Insert(0f, DOVirtual.DelayedCall(0f, () => PlaySound(clip1)));

        // �� ��° ���� Ŭ�� ��� (�̹������� �̵��ϴ� 3�� ����)
        testSequence.Insert(5.5f, DOVirtual.DelayedCall(0f, () => PlaySound(clip2)));
    }

    // ���� ��� �޼���
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // �������� ����ϴ� �޼��� (�ܺο��� ȣ�� ����)
    public void PlaySequence()
    {
        testSequence.Play();
    }
}
