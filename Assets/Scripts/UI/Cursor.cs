using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{

    private Canvas canvas;
    private Camera mainCamera;
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite transparentCursorSprite = null;
    [SerializeField] private GridCursor gridCursor = null;


    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private bool _cursorPositionIsValid = false;
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private ItemType _selectedItemType;
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private float _itemUseRadius = 0f;
    public float ItemUseRadius { get => _itemUseRadius; set => _itemUseRadius = value; }


    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    /// <summary>
    /// 显示光标
    /// </summary>
    private void DisplayCursor()
    {
        // 获取光标的世界位置坐标
        Vector3 cursorWorldPosition = GetWorldPositionForCursor();

        // 设置光标的图像
        SetCursorValidity(cursorWorldPosition, Player.Instance.GetPlayerCenterPosition());

        // 获取光标的布局定位信息
        cursorRectTransform.position = GetRectTransformPositionForCursor();
    }

    /// <summary>
    /// 设置光标的有效性
    /// </summary>
    /// <param name="cursorWorldPosition"></param>
    /// <param name="value"></param>
    private void SetCursorValidity(Vector3 cursorPosition, Vector3 playerPosition)
    {
        SetCursorToValid();

        // 检测使用范围的四个角
        if( cursorPosition.x > (playerPosition.x + ItemUseRadius /2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f)
            ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f)
            ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            ||
            cursorPosition.x > (playerPosition.x + ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            ) 
        {
            // 超出范围 不可用
            SetCursorToInvalid();
            return;
        }

        // 检测物品使用范围是否可用
        if(Mathf.Abs(cursorPosition.x - playerPosition.x) > ItemUseRadius
            || Mathf.Abs(cursorPosition.y - playerPosition.y) > ItemUseRadius)
        {
            // 超出使用范围 不可用
            SetCursorToInvalid();
            return;
        }

        // 获取选中物品的详情
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);
        if( itemDetails == null )
        {
            // 物品不存在  不可用
            SetCursorToInvalid();
            return;
        }

        // 光标的可用性取决于选中的库存中的物品和光标上的物品
        switch (itemDetails.itemType)
        {
            case ItemType.Watering_tool:
            case ItemType.Breaking_tool:
            case ItemType.Chopping_tool:
            case ItemType.Hoeing_tool:
            case ItemType.Reapable_scenary:
            case ItemType.Collecting_tool:
                if(!SetCursorValidityTool(cursorPosition, playerPosition, itemDetails))
                {
                    SetCursorToInvalid();
                    return;
                }
                break;

            case ItemType.none:
                break;

            case ItemType.count:
                break;

            default: break;
        }
    }

    /// <summary>
    /// 设置光标无效
    /// </summary>
    private void SetCursorToInvalid()
    {
        cursorImage.sprite = transparentCursorSprite;
        CursorPositionIsValid = false;

        gridCursor.EnableCursor();
    }

    /// <summary>
    /// 设置光标有效
    /// </summary>
    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;

        gridCursor.DisableCursor();
    }

    /// <summary>
    /// 为工具设置对应光标的有效性
    /// </summary>
    /// <param name="cursorPosition"></param>
    /// <param name="playerPosition"></param>
    /// <param name="itemDetails"></param>
    /// <returns></returns>
    private bool SetCursorValidityTool(Vector3 cursorPosition, Vector3 playerPosition, ItemDetails itemDetails)
    {
        switch(itemDetails.itemType)
        {
            case ItemType.Reaping_tool:
                return SetCursorValidityReapingTool(cursorPosition, playerPosition, itemDetails);

            default: 
                return false;
        }
    }

    /// <summary>
    /// 设置收获工具的光标有效性
    /// </summary>
    /// <param name="cursorPosition"></param>
    /// <param name="playerPosition"></param>
    /// <param name="itemDetails"></param>
    /// <returns></returns>
    private bool SetCursorValidityReapingTool(Vector3 cursorPosition, Vector3 playerPosition, ItemDetails itemDetails)
    {
        List<Item> itemList = new List<Item>();

        if(HelperMethods.GetComponentsAtCursorLocation<Item>(out itemList, cursorPosition))
        {
            if(itemList.Count != 0)
            {
                foreach(Item item in itemList)
                {
                    if(InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenary)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 获取光标的世界坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWorldPositionForCursor()
    {
        // 屏幕坐标转世界坐标
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    /// <summary>
    /// 获取光标的布局定位信息
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRectTransformPositionForCursor()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        // RectTransformUtility.PixelAdjustPoint用于精确调整UI在屏幕上的像素位置，防止像素模糊
        return RectTransformUtility.PixelAdjustPoint(screenPosition, cursorRectTransform, canvas);
    }

    /// <summary>
    /// 禁用光标
    /// </summary>
    public void DisableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 0f);
        CursorIsEnabled = false;
    }

    /// <summary>
    /// 启用光标
    /// </summary>
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }

}
