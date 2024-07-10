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
    /// ��ʾ���
    /// </summary>
    private void DisplayCursor()
    {
        // ��ȡ��������λ������
        Vector3 cursorWorldPosition = GetWorldPositionForCursor();

        // ���ù���ͼ��
        SetCursorValidity(cursorWorldPosition, Player.Instance.GetPlayerCenterPosition());

        // ��ȡ���Ĳ��ֶ�λ��Ϣ
        cursorRectTransform.position = GetRectTransformPositionForCursor();
    }

    /// <summary>
    /// ���ù�����Ч��
    /// </summary>
    /// <param name="cursorWorldPosition"></param>
    /// <param name="value"></param>
    private void SetCursorValidity(Vector3 cursorPosition, Vector3 playerPosition)
    {
        SetCursorToValid();

        // ���ʹ�÷�Χ���ĸ���
        if( cursorPosition.x > (playerPosition.x + ItemUseRadius /2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f)
            ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f)
            ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            ||
            cursorPosition.x > (playerPosition.x + ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            ) 
        {
            // ������Χ ������
            SetCursorToInvalid();
            return;
        }

        // �����Ʒʹ�÷�Χ�Ƿ����
        if(Mathf.Abs(cursorPosition.x - playerPosition.x) > ItemUseRadius
            || Mathf.Abs(cursorPosition.y - playerPosition.y) > ItemUseRadius)
        {
            // ����ʹ�÷�Χ ������
            SetCursorToInvalid();
            return;
        }

        // ��ȡѡ����Ʒ������
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);
        if( itemDetails == null )
        {
            // ��Ʒ������  ������
            SetCursorToInvalid();
            return;
        }

        // ���Ŀ�����ȡ����ѡ�еĿ���е���Ʒ�͹���ϵ���Ʒ
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
    /// ���ù����Ч
    /// </summary>
    private void SetCursorToInvalid()
    {
        cursorImage.sprite = transparentCursorSprite;
        CursorPositionIsValid = false;

        gridCursor.EnableCursor();
    }

    /// <summary>
    /// ���ù����Ч
    /// </summary>
    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;

        gridCursor.DisableCursor();
    }

    /// <summary>
    /// Ϊ�������ö�Ӧ������Ч��
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
    /// �����ջ񹤾ߵĹ����Ч��
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
    /// ��ȡ������������
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWorldPositionForCursor()
    {
        // ��Ļ����ת��������
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    /// <summary>
    /// ��ȡ���Ĳ��ֶ�λ��Ϣ
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRectTransformPositionForCursor()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        // RectTransformUtility.PixelAdjustPoint���ھ�ȷ����UI����Ļ�ϵ�����λ�ã���ֹ����ģ��
        return RectTransformUtility.PixelAdjustPoint(screenPosition, cursorRectTransform, canvas);
    }

    /// <summary>
    /// ���ù��
    /// </summary>
    public void DisableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 0f);
        CursorIsEnabled = false;
    }

    /// <summary>
    /// ���ù��
    /// </summary>
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }

}
