using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    private RectTransform _parent;
    private GridLayoutGroup _grid;
    [SerializeField] private int _cols; // ���� �� ����
    [SerializeField] private int _itemCount = 0; // ������ ����

    private void Awake()
    {
        _parent = gameObject.GetComponent<RectTransform>();
        _grid = gameObject.GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        SetDynamicGrid();
    }

    private void OnRectTransformDimensionsChange()
    {
        /*
        #if UNITY_ANDROID
        SetDynamicGrid();
        #endif
        */

        if (Application.platform == RuntimePlatform.Android)
        {
            SetDynamicGrid();
        }
    }

    private void SetDynamicGrid()
    {
        int rows = Mathf.CeilToInt((float)_itemCount / _cols);

        // �� ũ�� ���
        float availableWidth = _parent.rect.width - (_grid.spacing.x * (_cols + 1));
        float cellWidth = availableWidth / _cols;
        float cellHeight = cellWidth;
        _grid.cellSize = new Vector2(cellWidth, cellHeight);

        // grid layout group�� padding�� spacing�� �����ϰ� ����
        int padding = Mathf.RoundToInt(_grid.spacing.x);
        _grid.padding = new RectOffset(padding, padding, padding, padding);

        _grid.constraintCount = _cols;
    }

    public void SetItemCount(int count)
    {
        _itemCount = count;
        SetDynamicGrid();
    }
}
