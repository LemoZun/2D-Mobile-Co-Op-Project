using ExitGames.Client.Photon;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	[SerializeField] int gridNum; // ���� �׸��� ��ȣ 
	private	Image			image;
	private	RectTransform	rect;
	private bool isFull; // ���� �׸��忡 ������Ʈ�� �ִ��� ������

	private void Awake()
	{
		image	= GetComponent<Image>();
		rect	= GetComponent<RectTransform>();
		isFull = false;
		if (transform.childCount >= 1) 
		{
			isFull = true;
		}

    }

	/// <summary>
	/// ���콺 ����Ʈ�� ���� ������ ���� ���� ���η� �� �� 1ȸ ȣ��
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData)
	{
		// ������ ������ ������ ��������� ����
		//image.color = Color.green;
	}

	/// <summary>
	/// ���콺 ����Ʈ�� ���� ������ ���� ������ �������� �� 1ȸ ȣ��
	/// </summary>
	public void OnPointerExit(PointerEventData eventData)
	{
		// ������ ������ ������ �Ͼ������ ����
		//image.color = Color.black;
	}

	/// <summary>
	/// ���� ������ ���� ���� ���ο��� ����� ���� �� 1ȸ ȣ��
	/// </summary>
	public void OnDrop(PointerEventData eventData)
	{
		if (isFull == true)  
		{
			return;
		}
		if (gridNum == 0) 
		{
			return;
		}
		// pointerDrag�� ���� �巡���ϰ� �ִ� ���(=������)
		if ( eventData.pointerDrag != null )
		{
			// �巡���ϰ� �ִ� ����� �θ� ���� ������Ʈ�� �����ϰ�, ��ġ�� ���� ������Ʈ ��ġ�� �����ϰ� ����
			eventData.pointerDrag.transform.SetParent(transform);
			eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

			BattleSceneManager.Instance.inGridObject[gridNum] = eventData.pointerDrag;
			isFull = true;
		}
	}


    public void OnTransformChildrenChanged() // �ڽ��� ���� ����ɶ����� ȣ��
    {
		if (transform.childCount == 0)
		{			
            BattleSceneManager.Instance.inGridObject[gridNum] = null; //�׸��忡�� ������ ����
			isFull=false;
        }
		else if (transform.childCount >= 1) 
		{
            isFull = true;
        }
    }

}

