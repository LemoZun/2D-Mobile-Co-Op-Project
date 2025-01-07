using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GachaManager : MonoBehaviour
{
    // UI ���
    public Image scriptBg;          // ��ũ��Ʈ ��� �̹���
    public TMP_Text scriptText;     // ��ũ��Ʈ �ؽ�Ʈ
    public AudioSource audioSource; // ���� ����� ���� AudioSource
    public AudioClip scriptSd;      // ��ũ��Ʈ ��� �� ����
    public AudioClip pingSd;        // ���� ��Ÿ�� �� ����Ǵ� ping ����
    public AudioClip getSd;         // 'get' ���� Ŭ��
    public AudioClip showSd;        // 'show' ���� Ŭ��
    public AudioClip endSd;         // 'end' ���� Ŭ��

    public Image star1;             // �� �̹��� 1
    public Image star2;             // �� �̹��� 2
    public Image star3;             // �� �̹��� 3
    public Image star4;             // �� �̹��� 4
    public Image star5;             // �� �̹��� 5

    public Image background1;       // ��� �̹���
    public Image dinosaur1;         // ���� �̹���
    public Image character1;        // ĳ���� �̹���

    public TMP_Text name1;          // ĳ���� �̸� �ؽ�Ʈ1
    public TMP_Text name2;          // ĳ���� �̸� �ؽ�Ʈ2

    private Sequence animationSequence; // DOTween�� �ִϸ��̼� �������� �����ϴ� ����

    // OnEnable: ��ũ��Ʈ�� Ȱ��ȭ�� �� ����
    private void OnEnable()
    {
        MakeSequence(); // DOTween �������� ����
        animationSequence.Play();   // ������ �������� �ٷ� ����
    }

    // OnDisable: ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ����
    private void OnDisable()
    {
        Destroy(gameObject); // ���� ������Ʈ�� �����Ͽ� �޸� ����
    }

    // Start: ���� ���� �� ȣ��
    void Start()
    {
        InitializeUI(); // ��� UI ��Ҹ� �ʱ�ȭ (��Ȱ��ȭ ���·� ����)
    }

    // InitializeUI: UI ��Ҹ� �ʱ� ���·� ����
    private void InitializeUI()
    {
        // ��ũ��Ʈ ���� �ؽ�Ʈ�� ���� ���·� �ʱ�ȭ
        scriptBg.color = new Color(scriptBg.color.r, scriptBg.color.g, scriptBg.color.b, 0);
        scriptText.alpha = 0;

        // �� �̹����� ���� ���·� �ʱ�ȭ
        star1.color = new Color(star1.color.r, star1.color.g, star1.color.b, 0);
        star2.color = new Color(star2.color.r, star2.color.g, star2.color.b, 0);
        star3.color = new Color(star3.color.r, star3.color.g, star3.color.b, 0);
        star4.color = new Color(star4.color.r, star4.color.g, star4.color.b, 0);
        star5.color = new Color(star5.color.r, star5.color.g, star5.color.b, 0);

        // ���, ����, ĳ���� �̹����� ���� ���·� �ʱ�ȭ
        background1.color = new Color(background1.color.r, background1.color.g, background1.color.b, 0);
        dinosaur1.color = new Color(dinosaur1.color.r, dinosaur1.color.g, dinosaur1.color.b, 0);
        character1.color = new Color(character1.color.r, character1.color.g, character1.color.b, 0);

        // ĳ���� �̸� �ؽ�Ʈ�� ���� ���·� �ʱ�ȭ
        name1.alpha = 0;
        name2.alpha = 0;
    }

    // MakeSequence: DOTween �������� ����
    private void MakeSequence()
    {
        // ������ �ʱ�ȭ
        animationSequence = DOTween.Sequence();
        animationSequence.Pause(); // �������� ��� �������� �ʰ� ��� ���·� ����

        // 1. ��ũ��Ʈ ���� �ؽ�Ʈ�� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(getSd));    // 'get' ���� ���
        animationSequence.Append(scriptBg.DOFade(1.0f, 1f));   // ��� ���̵� ��
        animationSequence.Join(scriptText.DOFade(1.0f, 1f));   // �ؽ�Ʈ�� ���� ���ÿ� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(scriptSd)); // ���̵� ���� ���� �� ���� ���

        // 2. Ÿ���� ȿ�� ����
        animationSequence.AppendCallback(() =>
        {
            StartCoroutine(TypingEffect(scriptText, "��Ǳ����� Ʈ���ɶ��齺, Ʈ������. ���ú��� ���� ���� ���а� �Ǿ��ٰ�", 0.05f));
        });

        // 3. �ؽ�Ʈ�� 6�� ���� ����
        animationSequence.AppendInterval(6f);

        // 4. ��ũ��Ʈ �ؽ�Ʈ�� ��Ȱ��ȭ
        animationSequence.AppendInterval(0.5f); // �ణ�� ������ �߰�
        animationSequence.AppendCallback(() => scriptText.DOFade(0.0f, 0.5f));

        // 5. �� �ִϸ��̼� �߰� (1�� �ȿ� 5���� ���� ���������� ��Ÿ��)
        float starDuration = 1f;             // �� �� �ִϸ��̼� �ð�
        float starInterval = starDuration / 5f; // �� �ϳ��� ������ �ð� ����

        AppendStarAnimation(star1, starInterval);
        AppendStarAnimation(star2, starInterval);
        AppendStarAnimation(star3, starInterval);
        AppendStarAnimation(star4, starInterval);
        AppendStarAnimation(star5, starInterval);

        // 6. ��� �̹����� ���̵� ��
        animationSequence.Join(background1.DOFade(1.0f, 1f)); // ��� �̹����� ���̵� ��

        // 7. ����� ĳ���� �̹����� ���ÿ� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(showSd)); // 'show' ���� ���
        animationSequence.Append(dinosaur1.DOFade(1.0f, 1f));
        animationSequence.Join(character1.DOFade(1.0f, 1f));

        // 8. ĳ���� �̸� �ؽ�Ʈ�� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(endSd)); // 'end' ���� ���
        animationSequence.Append(name1.DOFade(1.0f, 1f));
        animationSequence.Join(name2.DOFade(1.0f, 1f));
    }

    // AppendStarAnimation: �� �ִϸ��̼� �߰�
    private void AppendStarAnimation(Image star, float interval)
    {
        animationSequence.Append(star.rectTransform.DORotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360)); // �� ȸ��
        animationSequence.Join(star.DOFade(1.0f, 0.2f)); // �� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(pingSd)); // ���� ��Ÿ�� �� ���� ���
        animationSequence.AppendInterval(interval); // ���� �� �ִϸ��̼� ������ ���
    }

    // TypingEffect: Ÿ���� ȿ�� ����
    private IEnumerator TypingEffect(TMP_Text textComponent, string fullText, float typingSpeed)
    {
        textComponent.text = ""; // �ؽ�Ʈ �ʱ�ȭ
        textComponent.alpha = 1; // �ؽ�Ʈ ǥ��
        foreach (char letter in fullText) // �� ���ڸ� ���������� ���
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed); // ���� �� ��� �ӵ� ����
        }
    }

    // PlaySound: ���� ���
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null) // AudioSource�� AudioClip�� �����Ǿ� �ִ� ���
        {
            audioSource.PlayOneShot(clip); // ������ Ŭ�� ���
        }
    }
}
