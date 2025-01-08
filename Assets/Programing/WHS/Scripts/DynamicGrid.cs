using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    private int characterCount;
    private RectTransform parent;
    private GridLayoutGroup grid;

    private void Awake()
    {
        parent = gameObject.GetComponent<RectTransform>();
        grid = gameObject.GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        characterCount = GetCharacterCount();
        SetDynamicGrid();
    }

    private void OnRectTransformDimensionsChange()
    {
        SetDynamicGrid();
    }

    private void SetDynamicGrid()
    {
        int cols = 5;
        int rows = Mathf.CeilToInt((float)characterCount / cols);

        // �� ũ�� ���
        float availableWidth = parent.rect.width - (grid.spacing.x * (cols + 1));
        float cellWidth = availableWidth / cols;
        float cellHeight = cellWidth;
        grid.cellSize = new Vector2(cellWidth, cellHeight);

        // grid layout group�� padding�� spacing�� �����ϰ� ����
        int padding = Mathf.RoundToInt(grid.spacing.x);
        grid.padding = new RectOffset(padding, padding, padding, padding);

        grid.constraintCount = cols;
    }

    private int GetCharacterCount()
    {
        InventoryPanel inventoryPanel = FindObjectOfType<InventoryPanel>();
        if (inventoryPanel != null)
        {
            return inventoryPanel.GetCharacterCount();
        }
        return 0;
    }
}
