using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// �����ui����
/// </summary>
/// IBeginDragHandler, IEndDragHandler�ӿ� �������϶��¼�������Ӧ
public class UIInventorySlot : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// �����  ��Ҫ����ת������
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// ������
    /// </summary>
    private Transform parentItem;
    /// <summary>
    /// ����ק������
    /// </summary>
    private GameObject draggedItem;

    /// <summary>
    /// ��λ�ı߿�
    /// </summary>
    public Image inventorySlotHighlight;
    /// <summary>
    /// ��λ����ƷͼƬ
    /// </summary>
    public Image inventorySlotImage;
    /// <summary>
    /// ��ʾ����Ʒ����
    /// </summary>
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPerfab = null;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [HideInInspector] public ItemDetails itemDetails;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [HideInInspector] public int itemQuantity;


    private void Start()
    {
        mainCamera = Camera.main;
        //����Ʒλ��
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
    }

    /// <summary>
    /// ��ק��ʼ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            //��Ʒ����ʱ

            //���ü������룬��ɫ���ƶ�
            Player.Instance.DisablePlayerInput();

            //�������������Ϊ����ק������  ʵ����
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            //��ȡ����ק������ͼ��
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;
        }
    }

    /// <summary>
    /// ��ק��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        //������ק
        if(draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// ��ק����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //�������ڱ���ק����Ʒ
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            //"pointerCurrentRaycast" �� Unity �� EventSystem ���е�һ���ֶΣ����ڴ洢��ǰ��꣨�����㣩���䵽�Ķ������Ϣ��
            //����ֶδ洢����һ�� RaycastResult �ṹ�壬�����˱���������еĶ���������Ϣ��
            //���类���е� GameObject����ײ�㡢���߷���ȡ�
            //���Ի�ȡ��ǰ��������е� UI Ԫ�ص���Ϣ���Ӷ�������Ӧ�Ĵ���������Ӧ����¼�����ʾ������ʾ�ȡ�
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                //��Ʒ��δ��ק������湤��������������
                Debug.Log("δ����������");
            }
            else
            {
                //��ק��ȥ
                if(itemDetails.canBeDropped)
                {
                    //������ק����Ʒ���õ���굱ǰλ��
                    DropSelectedItemAtMousePosition();
                }
            }

            //�����û����룬��ɫ�����ƶ���
            Player.Instance.EnablePlayerInput();
        }
    }

    /// <summary>
    /// ����ק������õ���굱ǰλ��
    /// </summary>
    private void DropSelectedItemAtMousePosition()
    {
        if(itemDetails != null)
        {
            //���ｫ����ת��Ϊ�������꣬�������ʵ����Ԥ����ᵼ��λ�ô��󣬲��������Χ����ʾ
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //��Ԥ������ʵ����һ�����嵽��굱ǰλ��
            GameObject itemGameObject = Instantiate(itemPerfab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            //�ӿ�����Ƴ���Ʒ
            InventoryManager.Instance.RemoveItem(InventoryLocation.player,item.ItemCode);
        }
    }
}
