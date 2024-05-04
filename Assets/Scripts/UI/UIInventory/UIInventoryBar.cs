
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ����������ui����
/// </summary>
public class UIInventoryBar : MonoBehaviour
{
    /// <summary>
    /// �հ�ͼƬ������ʱʹ��
    /// </summary>
    [SerializeField] private Sprite blank16x16sprite = null;
    /// <summary>
    /// �������ۣ�ÿһ��
    /// </summary>
    [SerializeField] private UIInventorySlot[] inventorySlot = null;
    /// <summary>
    /// ��ȡ������Ʒ�������ק��
    /// </summary>
    public GameObject inventoryBarDraggedItem;

    /// <summary>
    /// λ�����
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// ��湤�����Ƿ��ڵײ�λ��
    /// </summary>
    private bool _isInventoryBarPositionBottom = true;
    /// <summary>
    /// ��湤�����Ƿ��ڵײ�λ��
    /// </summary>
    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //�������λ�õ����ײ�������λ��
        SwitchInventoryBarPosition();
    }


    private void OnEnable()
    {
        //�ű�����ʱ�����Ŀ������¼�
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void OnDisable()
    {
        //�ű����ã�ȡ������
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }



    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        //��Ʒ�����ɫ����
        if(inventoryLocation == InventoryLocation.player)
        {
            //������
            ClearInventorySlots();

            //�ؽ����
            if(inventorySlot.Length > 0 && inventoryList.Count >0)
            {

                for(int i = 0; i< inventorySlot.Length; i++)
                {
                    if (i < inventoryList.Count)
                    {
                        int itemCode = inventoryList[i].itemCode;

                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if (itemDetails != null)
                        {
                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    private void ClearInventorySlots()
    {
        if(inventorySlot.Length >0 )
        {
            for(int i = 0;i< inventorySlot.Length; i++)
            {
                inventorySlot[i].inventorySlotImage.sprite = blank16x16sprite;
                inventorySlot[i].textMeshProUGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;
            }
        }

    }

    /// <summary>
    /// �л�������λ�ã��������λ�ã�
    /// </summary>
    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();
        //���½�����0 0�����Ͻ�����1 1
        //������λ��y�������½Ǵ�������֮һ���򹤾���Ӧ�÷����·�
        //���򹤾���Ӧ�����Ϸ�
        if(playerViewportPosition.y > 0.3f && !IsInventoryBarPositionBottom)
        {
            //pivot ���Ե�ֵΪһ����ά���� (0, 0) �� (1, 1)������ (0, 0) ��ʾ RectTransform �����½ǣ�(1, 1) ��ʾ���Ͻǡ�
            //�� UI Ԫ�ص���ת��������������Ϊ����ĵײ����ġ�
            rectTransform.pivot = new Vector2(0.5f, 0f);

            //anchorMin ������ UI Ԫ������ڸ������������½ǵ�λ�ã��� anchorMax ��������������Ͻǵ�λ�á�
            //���ｫ UI Ԫ������ڸ���������ê��λ������Ϊ�ײ����ġ�
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);

            //�� UI Ԫ�������ê��λ�õ�ƫ��������Ϊ (0, 2.5)��ʹ�� UI Ԫ���ڸ�������������ƫ���� 2.5 ����λ��
            rectTransform.anchoredPosition = new Vector2(0f, 2.5f);

            IsInventoryBarPositionBottom = true;
        }
        else if(playerViewportPosition.y <= 0.3f && IsInventoryBarPositionBottom)
        {
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -2.5f);

            IsInventoryBarPositionBottom = false;
        }
    }
}
