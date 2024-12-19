using UnityEngine;
using UnityEngine.EventSystems;

public class UiDrag: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // UI ����� RectTransform
    private Canvas canvas;               // UI�� ���� ĵ����
    private CanvasGroup canvasGroup;     // �巡�� �� ���� �� ��ȣ�ۿ� ����

    private Vector2 basePos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // �θ� ĵ���� ��������
        canvasGroup = GetComponent<CanvasGroup>(); // CanvasGroup�� ���� ����
        basePos = transform.position;   
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ���� �� ��ȣ�ۿ� ��Ȱ��ȭ (�ʿ� ��)
        canvasGroup.alpha = 0.3f;       // ���� ����
        canvasGroup.blocksRaycasts = false; // �巡�� �� �ٸ� UI�� �浹 ����
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� �� UI ��ġ �̵�
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ���� �� ����
        canvasGroup.alpha = 1.0f;       // ���� �������
        canvasGroup.blocksRaycasts = true; // ��ȣ�ۿ� ����
        //transform.position =  basePos;
    }

   
}