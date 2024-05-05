using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// �����ui����
/// </summary>
/// IBeginDragHandler, IEndDragHandler, IDragHandler�ӿ� �������϶��¼�������Ӧ
/// IPointerEnterHandler,IPointerExitHandler�ӿ� �������ͣ�¼�������Ӧ
/// IPointerClickHandler�ӿ� ��������¼�������Ӧ
public class UIInventorySlot : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    /// <summary>
    /// �����  ��Ҫ����ת������
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// ��������
    /// </summary>
    private Canvas parentCanvas;

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
    [SerializeField] private int slotNumber = 0;
    [SerializeField] private GameObject inventoryTextBoxPerfab = null;

    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [HideInInspector] public ItemDetails itemDetails;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    [HideInInspector] public int itemQuantity;
    /// <summary>
    /// ��ǰ����Ƿ�ѡ��
    /// </summary>
    [HideInInspector] public bool isSelected = false;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

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

            //���ñ�ѡ�и�����
            SetSelectedItem();
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
                //��Ʒ��δ��ק������湤����������λ��

                //��ȡ��ק��������Ʒ����ק���ĸ���λ
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;

                //����λ��
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);
                //����
                DestroyInventoryTextBox();
                //���ѡ�и���
                ClearSelectedItem();
            }
            else
            {
                //������Զ���������ק��ȥ
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
        if(itemDetails != null && isSelected)
        {
            //���ｫ����ת��Ϊ�������꣬�������ʵ����Ԥ����ᵼ��λ�ô��󣬲��������Χ����ʾ
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //��Ԥ������ʵ����һ�����嵽��굱ǰλ��
            GameObject itemGameObject = Instantiate(itemPerfab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            //�ӿ�����Ƴ���Ʒ
            InventoryManager.Instance.RemoveItem(InventoryLocation.player,item.ItemCode);

            //�����Ʒ�����ڣ����ѡ�п�
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.player,item.ItemCode) == -1)
            {
                ClearSelectedItem();
            }
        }
    }

    /// <summary>
    /// ���ָ���ƶ�����Ϸ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemQuantity != 0)
        {
            //ʵ�����ı���
            inventoryBar.inventoryTextBoxGameObject = Instantiate(inventoryTextBoxPerfab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameObject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameObject.GetComponent<UIInventoryTextBox>();

            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            //���ݹ�������λ�������ı����λ��
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                //�������ڵײ�,�ı�����ڹ���������
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 25f, transform.position.z);

            }
            else
            {
                //�������ڶ���,�ı�����ڹ���������
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 25f, transform.position.z);
            }

        }
    }

    /// <summary>
    /// ���ָ�����Ϸ�����ƿ�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    /// <summary>
    /// ������Ʒ�����ı���
    /// </summary>
    public void DestroyInventoryTextBox()
    {
        if(inventoryBar.inventoryTextBoxGameObject != null)
        {
            Destroy(inventoryBar.inventoryTextBoxGameObject);
        }
    }

    /// <summary>
    /// �����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //���������
            if(isSelected == true)
            {
                //�������Ѿ���ѡ�У���ȡ��ѡ��
                ClearSelectedItem();
            }
            else
            {
                //������δ��ѡ��
                if(itemQuantity > 0)
                {
                    //��ǰ���������������0��ѡ��
                    SetSelectedItem();
                }
            }
        }
    }

    /// <summary>
    /// ѡ����Ʒ
    /// </summary>
    private void SetSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = true;
        inventoryBar.SetHighlightedInventorySlots();

        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);
    }

    /// <summary>
    /// ȡ��ѡ��
    /// </summary>
    private void ClearSelectedItem()
    {
        //�����۵ĸ�����
        inventoryBar.ClearHighlightOnInventorySlots();
        isSelected = false;
        //���ѡ�б�ʶ
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
    }
}
