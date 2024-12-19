using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;  // UI ����� RectTransform

    private Vector2 _minAnchor;            // anchorMin ��

    private Vector2 _maxAnchor;            // anchorMax ��

    private void Awake()
    {
        if (_rectTransform == null)
        {
            return;
        }

        // SafeZone�� ���� �Ʒ� �𼭸� ���
        _minAnchor.x = Screen.safeArea.position.x / Screen.width;
        _minAnchor.y = Screen.safeArea.position.y / Screen.height;

        //SafeZone�� ������ �� �𼭸� ���
        _maxAnchor.x = (Screen.safeArea.position.x + Screen.safeArea.size.x) / Screen.width;
        _maxAnchor.y = (Screen.safeArea.position.y + Screen.safeArea.size.y) / Screen.height;

        // ��Ŀ ���� RectTransform�� ����
        _rectTransform.anchorMin = _minAnchor;
        _rectTransform.anchorMax = _maxAnchor;

        // offset 0���� ����.
        _rectTransform.offsetMin = Vector2.zero; 
        _rectTransform.offsetMax = Vector2.zero; 
    }
}