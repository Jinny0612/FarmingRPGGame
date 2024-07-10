using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 网格光标   放置物品/家具等时，会在地面上显示当前网格是否可以放置
/// </summary>
public class GridCursor : MonoBehaviour
{
    /// <summary>
    /// ui画布
    /// </summary>
    private Canvas canvas;
    /// <summary>
    /// 网格
    /// </summary>
    private Grid grid;
    /// <summary>
    /// 主相机
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// 光标图片
    /// </summary>
    [SerializeField] private Image cursorImage = null;
    /// <summary>
    /// 位置 缩放等信息
    /// </summary>
    [SerializeField] private RectTransform cursorRectTransform = null;
    /// <summary>
    /// 绿色光标精灵图 即当前位置可以放置
    /// </summary>
    [SerializeField] private Sprite greenCursorSprite = null;
    /// <summary>
    /// 红色光标精灵图 即当前位置不可放置
    /// </summary>
    [SerializeField] private Sprite redCursorSprite = null;


    private bool _cursorPositionIsValid = false;
    /// <summary>
    /// 光标位置是否可用  即是否可以放置物品或是否可以锄地种地等等
    /// </summary>
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private int _itemUserGridRadius = 0;
    /// <summary>
    /// 物品使用网格半径
    /// </summary>
    public int ItemUserGridRadius { get => _itemUserGridRadius; set => _itemUserGridRadius = value; }

    private ItemType _selectedItemType;
    /// <summary>
    /// 选择的物品类型
    /// </summary>
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;
    /// <summary>
    /// 光标是否开启
    /// </summary>
    public bool CursorIsEnabled { get=> _cursorIsEnabled; set => _cursorIsEnabled = value; }


    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    /// <summary>
    /// 场景加载需要做的处理
    /// </summary>
    private void SceneLoaded()
    {
        //获取网格类型的游戏对象
        //这里没获取到不知道为什么
        grid = GameObject.FindObjectOfType<Grid>();
    }


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    /// <summary>
    /// 光标显示
    /// </summary>
    /// <returns></returns>
    private Vector3Int DisplayCursor()
    {
        if(grid != null)
        {
            //获取光标的网格坐标
            Vector3Int gridPosition = GetGridPositionForCursor();
            //Debug.Log("ScreenmouseX = " + gridPosition.x + "  ScreenmouseY = " + gridPosition.y);
            //获取玩家的网格坐标
            Vector3Int playerGridPosition = GetGridPositionForPlayer();
            //Debug.Log("playerX = " + playerGridPosition.x + "  playerY = " + playerGridPosition.y);
            //设置光标精灵图
            SetCursorValidity(gridPosition, playerGridPosition);
            //获取光标位置
            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

            return gridPosition;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    /// <summary>
    /// 获取光标的RectTransfrom位置
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    private Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition)
    {
        //网格坐标转换为世界坐标
        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        //将世界坐标转换为主相机的屏幕坐标
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        //将屏幕坐标调整为像素对齐以方便适应不同的屏幕和分辨率
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    /// <summary>
    /// 获取玩家的网格坐标
    /// 将世界坐标转换为屏幕空间内的坐标
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetGridPositionForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    /// <summary>
    /// 获取光标的网格坐标
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetGridPositionForCursor()
    {
        // z坐标位置游戏物体在相机前面的距离，因为相机的z=-10，所有物品的z都是0，因此这里用-mainCamera.transform.position.z
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        return grid.WorldToCell(worldPosition);
    }

    /// <summary>
    /// 设置光标的有效性
    /// </summary>
    /// <param name="cursorGridPosition"></param>
    /// <param name="playerGridPosition"></param>
    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        //设置光标为有效
        SetCursorToValid();

        //判断物品使用的网格半径
        if(Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUserGridRadius
            || Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUserGridRadius)
        {
            //设置光标为无效
            SetCursorToInvalid();
            return;
        }

        //获取已选择物品的详情
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if(itemDetails == null)
        {
            SetCursorToInvalid();
            return;
        }

        //获取光标位置的网格属性详细信息
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        if(gridPropertyDetails != null)
        {
            // 基于选中的物品和网格属性详情确定光标的有效性
            switch(itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!IsCursorValidForSeed(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Commodity:
                    if (!IsCursorValildForCommodity(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Breaking_tool:
                case ItemType.Chopping_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collecting_tool:
                    if(!IsCursorValidForTool(gridPropertyDetails, itemDetails))
                    {
                        SetCursorToInvalid();
                        return ;
                    }
                    break;

                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
                default:
                    break;
            }
        }
        else
        {
            SetCursorToInvalid();
            return;
        }
    }

    /// <summary>
    /// 网格光标对工具类物品是否可用
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="itemDetails"></param>
    /// <returns></returns>
    private bool IsCursorValidForTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if(gridPropertyDetails.isDiggable && gridPropertyDetails.daysSinceDug == -1)
                {
                    #region 获取在当前光标位置的物品，确认是否可以在这里挖掘

                    // 获取光标的世界坐标
                    //网格位置从网格左下角来标记的，但是光标设置的中心点，因此需要偏移
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPositionForCursor().x + 0.5f, GetWorldPositionForCursor().y + 0.5f, 0f);
                    
                    List<Item> itemList = new List<Item>();
                    //获取区域内具有Item组件的物体（即各种item预制体的变体）
                    HelperMethods.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition, Settings.cursorSize, 0f);


                    #endregion

                    bool foundReapable = false;

                    foreach (Item item in itemList)
                    {
                        if(InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenary)
                        {
                            foundReapable = true;
                            break;
                        }
                    }

                    if (foundReapable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            case ItemType.Watering_tool:
                // 只有光标所在的网格已经被锄头挖掘并且未被浇水，当前光标才可用
                if(gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.daysSinceWatered == -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            default: return false;
        }
    }

    /// <summary>
    /// 获取光标世界坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetWorldPositionForCursor()
    {
        return grid.CellToWorld(GetGridPositionForCursor());
    }

    /// <summary>
    /// 网格光标对商品类物品是否可用
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private bool IsCursorValildForCommodity(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    /// <summary>
    /// 网格光标对种子类物品是否可用
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private bool IsCursorValidForSeed(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    /// <summary>
    /// 设置禁止使用网格光标
    /// 
    /// 在库存中的物品不再被选中时调用
    /// </summary>
    public void DisableCursor()
    {
        cursorImage.color = Color.clear;
        CursorIsEnabled = false;
    }

    /// <summary>
    /// 设置可以使用网格光标
    /// 选中库存物品时调用
    /// </summary>
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f,1f,1f,1f);
        CursorIsEnabled = true;
    }

    /// <summary>
    /// 设置光标为有效
    /// </summary>
    private void SetCursorToValid()
    {
        //设置为绿框
        cursorImage.sprite = greenCursorSprite;
        //光标有效性标记为有效
        CursorPositionIsValid = true;
    }

    /// <summary>
    /// 设置光标为无效
    /// </summary>
    private void SetCursorToInvalid()
    {
        //设置为红框
        cursorImage.sprite = redCursorSprite;
        //光标有效性标记为无效
        CursorPositionIsValid = false;
    }
}
