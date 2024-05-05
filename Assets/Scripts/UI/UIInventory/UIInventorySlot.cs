using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 库存插槽ui管理
/// </summary>
/// IBeginDragHandler, IEndDragHandler, IDragHandler接口 对物体拖动事件进行响应
/// IPointerEnterHandler,IPointerExitHandler接口 对鼠标悬停事件进行相应
/// IPointerClickHandler接口 对鼠标点击事件进行响应
public class UIInventorySlot : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    /// <summary>
    /// 主相机  需要用来转换坐标
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// 父级画布
    /// </summary>
    private Canvas parentCanvas;

    /// <summary>
    /// 父物体
    /// </summary>
    private Transform parentItem;
    /// <summary>
    /// 被拖拽的物体
    /// </summary>
    private GameObject draggedItem;

    /// <summary>
    /// 槽位的边框
    /// </summary>
    public Image inventorySlotHighlight;
    /// <summary>
    /// 槽位的物品图片
    /// </summary>
    public Image inventorySlotImage;
    /// <summary>
    /// 显示的物品数量
    /// </summary>
    public TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPerfab = null;
    [SerializeField] private int slotNumber = 0;
    [SerializeField] private GameObject inventoryTextBoxPerfab = null;

    /// <summary>
    /// 物品详情
    /// </summary>
    [HideInInspector] public ItemDetails itemDetails;
    /// <summary>
    /// 物品数量
    /// </summary>
    [HideInInspector] public int itemQuantity;
    /// <summary>
    /// 当前插槽是否被选中
    /// </summary>
    [HideInInspector] public bool isSelected = false;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        //父物品位置
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
    }

    /// <summary>
    /// 拖拽开始
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            //物品存在时
            
            //禁用键盘输入，角色不移动
            Player.Instance.DisablePlayerInput();

            //将这个物体生成为被拖拽的物体  实例化
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            //获取被拖拽的物体图形
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

            //设置被选中高亮框
            SetSelectedItem();
        }
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        //跟踪拖拽
        if(draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// 拖拽结束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //销毁正在被拖拽的物品
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            //"pointerCurrentRaycast" 是 Unity 的 EventSystem 类中的一个字段，用于存储当前光标（或触摸点）所射到的对象的信息。
            //这个字段存储的是一个 RaycastResult 结构体，包含了被光标所射中的对象的相关信息，
            //比如被射中的 GameObject、碰撞点、法线方向等。
            //可以获取当前光标所射中的 UI 元素的信息，从而进行相应的处理，比如响应点击事件、显示交互提示等。
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                //物品并未拖拽超出库存工具栏，交换位置

                //获取拖拽结束后，物品被拖拽至哪个槽位
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;

                //交换位置
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);
                //销毁
                DestroyInventoryTextBox();
                //清除选中高亮
                ClearSelectedItem();
            }
            else
            {
                //如果可以丢弃，就拖拽出去
                if(itemDetails.canBeDropped)
                {
                    //将被拖拽的物品放置到鼠标当前位置
                    DropSelectedItemAtMousePosition();
                }
            }

            //启用用户输入，角色可以移动了
            Player.Instance.EnablePlayerInput();
        }
    }

    /// <summary>
    /// 将拖拽物体放置到鼠标当前位置
    /// </summary>
    private void DropSelectedItemAtMousePosition()
    {
        if(itemDetails != null && isSelected)
        {
            //这里将坐标转换为世界坐标，否则后面实例化预制体会导致位置错误，不在相机范围内显示
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //从预制体中实例化一个物体到鼠标当前位置
            GameObject itemGameObject = Instantiate(itemPerfab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            //从库存中移除物品
            InventoryManager.Instance.RemoveItem(InventoryLocation.player,item.ItemCode);

            //如果物品不存在，清除选中框
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.player,item.ItemCode) == -1)
            {
                ClearSelectedItem();
            }
        }
    }

    /// <summary>
    /// 鼠标指针移动到游戏对象
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemQuantity != 0)
        {
            //实例化文本框
            inventoryBar.inventoryTextBoxGameObject = Instantiate(inventoryTextBoxPerfab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameObject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameObject.GetComponent<UIInventoryTextBox>();

            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            //根据工具栏的位置设置文本框的位置
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                //工具栏在底部,文本框就在工具栏上面
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 25f, transform.position.z);

            }
            else
            {
                //工具栏在顶部,文本框就在工具栏下面
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 25f, transform.position.z);
            }

        }
    }

    /// <summary>
    /// 鼠标指针从游戏对象移开
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    /// <summary>
    /// 销毁物品详情文本框
    /// </summary>
    public void DestroyInventoryTextBox()
    {
        if(inventoryBar.inventoryTextBoxGameObject != null)
        {
            Destroy(inventoryBar.inventoryTextBoxGameObject);
        }
    }

    /// <summary>
    /// 鼠标点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //鼠标左键点击
            if(isSelected == true)
            {
                //这个插槽已经被选中，则取消选中
                ClearSelectedItem();
            }
            else
            {
                //这个插槽未被选中
                if(itemQuantity > 0)
                {
                    //当前插槽物体数量大于0，选中
                    SetSelectedItem();
                }
            }
        }
    }

    /// <summary>
    /// 选中物品
    /// </summary>
    private void SetSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = true;
        inventoryBar.SetHighlightedInventorySlots();

        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, itemDetails.itemCode);
    }

    /// <summary>
    /// 取消选中
    /// </summary>
    private void ClearSelectedItem()
    {
        //清除插槽的高亮框
        inventoryBar.ClearHighlightOnInventorySlots();
        isSelected = false;
        //清空选中标识
        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
    }
}
