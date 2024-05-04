using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 库存插槽ui管理
/// </summary>
/// IBeginDragHandler, IEndDragHandler接口 对物体拖动事件进行响应
public class UIInventorySlot : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// 主相机  需要用来转换坐标
    /// </summary>
    private Camera mainCamera;
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

    /// <summary>
    /// 物品详情
    /// </summary>
    [HideInInspector] public ItemDetails itemDetails;
    /// <summary>
    /// 物品数量
    /// </summary>
    [HideInInspector] public int itemQuantity;


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
                //物品并未拖拽超出库存工具栏，不做处理
                Debug.Log("未超出工具栏");
            }
            else
            {
                //拖拽出去
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
        if(itemDetails != null)
        {
            //这里将坐标转换为世界坐标，否则后面实例化预制体会导致位置错误，不在相机范围内显示
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //从预制体中实例化一个物体到鼠标当前位置
            GameObject itemGameObject = Instantiate(itemPerfab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            //从库存中移除物品
            InventoryManager.Instance.RemoveItem(InventoryLocation.player,item.ItemCode);
        }
    }
}
