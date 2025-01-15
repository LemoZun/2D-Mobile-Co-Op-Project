using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GachaManager : MonoBehaviour
{
    // UI ��� ����
    public Image scriptBg;          // ��ũ��Ʈ ��� �̹���
    public TMP_Text scriptText;     // ��ũ��Ʈ �ؽ�Ʈ (�̸� �Էµ� �ؽ�Ʈ�� Ÿ���� ȿ���� ���)
    public AudioSource audioSource; // ���� ����� ���� AudioSource
    public AudioClip scriptSd;      // ��ũ��Ʈ ��� �� ����Ǵ� ����
    public AudioClip pingSd;        // �� �ִϸ��̼ǿ��� ����Ǵ� ȿ����
    public AudioClip getSd;         // "get" �̺�Ʈ ���� Ŭ��
    public AudioClip showSd;        // "show" �̺�Ʈ ���� Ŭ��
    public AudioClip endSd;         // "end" �̺�Ʈ ���� Ŭ��

    public Image star1;             // �� �̹��� 1
    public Image star2;             // �� �̹��� 2
    public Image star3;             // �� �̹��� 3
    public Image star4;             // �� �̹��� 4
    public Image star5;             // �� �̹��� 5

    public Image background1;       // ��� �̹���
    public Image dinosaur1;         // ���� �̹���
    public Image character1;        // ĳ���� �̹���

    public TMP_Text name1;          // ĳ���� �̸� �ؽ�Ʈ 1
    public TMP_Text name2;          // ĳ���� �̸� �ؽ�Ʈ 2

    private Sequence animationSequence; // DOTween �ִϸ��̼� �������� �����ϴ� ����

    // OnEnable: ��ũ��Ʈ�� Ȱ��ȭ�� �� ����
    private void OnEnable()
    {
        MakeSequence(); // DOTween ������ ����
        animationSequence.Play();   // ������ ������ ����
    }

    // OnDisable: ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ����
    private void OnDisable()
    {
        Destroy(gameObject); // ���� ������Ʈ ���� (�޸� ����)
    }

    // Start: ���� ���� �� ȣ��
    private void Start()
    {
        InitializeUI(); // UI �ʱ�ȭ (��� ��Ҹ� ��Ȱ��ȭ ���·� ����)
    }

    // InitializeUI: UI �ʱ�ȭ
    private void InitializeUI()
    {
        // ��ũ��Ʈ ����� ���� ���·� �ʱ�ȭ
        scriptBg.color = new Color(scriptBg.color.r, scriptBg.color.g, scriptBg.color.b, 0);

        // ��ũ��Ʈ �ؽ�Ʈ�� ���� ���·� �ʱ�ȭ
        scriptText.alpha = 0;

        // ��� �� �̹����� ���� ���·� �ʱ�ȭ
        InitializeImage(star1);
        InitializeImage(star2);
        InitializeImage(star3);
        InitializeImage(star4);
        InitializeImage(star5);

        // ���, ����, ĳ���� �̹����� ���� ���·� �ʱ�ȭ
        InitializeImage(background1);
        InitializeImage(dinosaur1);
        InitializeImage(character1);

        // ĳ���� �̸� �ؽ�Ʈ�� ���� ���·� �ʱ�ȭ
        name1.alpha = 0;
        name2.alpha = 0;
    }

    // InitializeImage: �̹����� ���� ���·� ����
    private void InitializeImage(Image img)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
    }

    // MakeSequence: DOTween �ִϸ��̼� ������ ����
    private void MakeSequence()
    {
        animationSequence = DOTween.Sequence(); // ������ �ʱ�ȭ
        animationSequence.Pause();             // �ٷ� �������� �ʰ� ��� ���·� ����

        // 1. ��ũ��Ʈ ���� �ؽ�Ʈ ���̵� ��
        // animationSequence.AppendCallback(() => PlaySound(getSd));  // "get" ���� ���
        animationSequence.Append(scriptBg.DOFade(1.0f, 1f));       // ��� ���̵� ��
        animationSequence.AppendCallback(() =>
        {
            scriptText.alpha = 1; // Ÿ���� ���� �� �ؽ�Ʈ Ȱ��ȭ
            PlaySound(scriptSd);  // ��ũ��Ʈ ���� ���
            StartCoroutine(TypingEffect(scriptText, 0.05f)); // Ÿ���� ȿ�� ����
        });

        // 2. �ؽ�Ʈ�� 8�� ���� ����
        animationSequence.AppendInterval(8f);

        // 4. �� �ִϸ��̼� �߰�
        AppendStarAnimation(star1, 0.2f);
        AppendStarAnimation(star2, 0.2f);
        AppendStarAnimation(star3, 0.2f);
        AppendStarAnimation(star4, 0.2f);
        AppendStarAnimation(star5, 0.2f);

        // 5. ��� ���̵� ��
        animationSequence.Join(background1.DOFade(1.0f, 1f));

        // 6. ����� ĳ���� �̹��� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(showSd)); // "show" ���� ���
        animationSequence.Append(dinosaur1.DOFade(1.0f, 1f));
        animationSequence.Join(character1.DOFade(1.0f, 1f));

        // 7. ĳ���� �̸� �ؽ�Ʈ ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(endSd)); // "end" ���� ���
        animationSequence.Append(name1.DOFade(1.0f, 1f));
        animationSequence.Join(name2.DOFade(1.0f, 1f));

    }

    // AppendStarAnimation: �� �ִϸ��̼� �߰�
    private void AppendStarAnimation(Image star, float duration)
    {
        animationSequence.Append(star.rectTransform.DORotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360)); // �� ȸ��
        animationSequence.Join(star.DOFade(1.0f, duration)); // �� ���̵� ��
        animationSequence.AppendCallback(() => PlaySound(pingSd)); // ���� ���
        animationSequence.AppendInterval(duration); // ���� ����
    }

    // TypingEffect: �ؽ�Ʈ Ÿ���� ȿ��
    private IEnumerator TypingEffect(TMP_Text textComponent, float typingSpeed)
    {
        animationSequence.AppendCallback(() => PlaySound(getSd));  // "get" ���� ���
        string fullText = textComponent.text; // �̸� �Էµ� �ؽ�Ʈ ��������
        textComponent.text = "";             // �ؽ�Ʈ �ʱ�ȭ

        // �� ���ڸ� ���������� ���
        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed); // ��� �ӵ� ����
        }
        yield return new WaitForSeconds(5f);

        // 3. �ؽ�Ʈ ��Ȱ��ȭ
        animationSequence.Append(scriptText.DOFade(0.0f, 0.5f));
    }

    // PlaySound: ���� ���
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null) // AudioSource�� AudioClip�� ������ ���
        {
            audioSource.PlayOneShot(clip); // ȿ���� ���
        }
    }
}
