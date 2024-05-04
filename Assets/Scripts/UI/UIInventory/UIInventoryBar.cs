
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色背包工具栏ui管理
/// </summary>
public class UIInventoryBar : MonoBehaviour
{
    /// <summary>
    /// 空白图片，重置时使用
    /// </summary>
    [SerializeField] private Sprite blank16x16sprite = null;
    /// <summary>
    /// 库存栏插槽（每一格）
    /// </summary>
    [SerializeField] private UIInventorySlot[] inventorySlot = null;
    /// <summary>
    /// 被取出的物品（鼠标拖拽）
    /// </summary>
    public GameObject inventoryBarDraggedItem;

    /// <summary>
    /// 位置组件
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// 库存工具栏是否在底部位置
    /// </summary>
    private bool _isInventoryBarPositionBottom = true;
    /// <summary>
    /// 库存工具栏是否在底部位置
    /// </summary>
    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //基于玩家位置调整底部工具栏位置
        SwitchInventoryBarPosition();
    }


    private void OnEnable()
    {
        //脚本启用时，订阅库存更新事件
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void OnDisable()
    {
        //脚本禁用，取消订阅
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }



    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        //物品存入角色背包
        if(inventoryLocation == InventoryLocation.player)
        {
            //清除插槽
            ClearInventorySlots();

            //重建插槽
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
    /// 清除插槽
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
    /// 切换工具栏位置（基于玩家位置）
    /// </summary>
    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();
        //左下角坐标0 0，右上角坐标1 1
        //如果玩家位置y距离左下角大于三分之一，则工具栏应该放在下方
        //否则工具栏应该在上方
        if(playerViewportPosition.y > 0.3f && !IsInventoryBarPositionBottom)
        {
            //pivot 属性的值为一个二维向量 (0, 0) 到 (1, 1)，其中 (0, 0) 表示 RectTransform 的左下角，(1, 1) 表示右上角。
            //将 UI 元素的旋转和缩放中心设置为自身的底部中心。
            rectTransform.pivot = new Vector2(0.5f, 0f);

            //anchorMin 定义了 UI 元素相对于父级容器的左下角的位置，而 anchorMax 定义了相对于右上角的位置。
            //这里将 UI 元素相对于父级容器的锚定位置设置为底部中心。
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);

            //将 UI 元素相对于锚定位置的偏移量设置为 (0, 2.5)，使得 UI 元素在父级容器中向上偏移了 2.5 个单位。
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
