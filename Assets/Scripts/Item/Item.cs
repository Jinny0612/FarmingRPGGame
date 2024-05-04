
using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 物品编号
    /// </summary>
    [ItemCodeDescription]
    [SerializeField]
    private int _itemCode;

    /// <summary>
    /// sprite渲染器
    /// </summary>
    private SpriteRenderer spriteRenderer;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if(ItemCode != 0)
        {
            Init(ItemCode);
        }
    }

    //根据设定的物品代码初始化物品信息
    private void Init(int itemCodeParam)
    {
        if (ItemCode != 0)
        {
            ItemCode = itemCodeParam;
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);
            spriteRenderer.sprite = itemDetails.itemSprite;
            //判断这个物品是否是可收获物品
            if(itemDetails.itemType == ItemType.Reapable_scenary)
            {
                //添加晃动效果
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }
}
