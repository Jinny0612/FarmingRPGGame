
using UnityEngine;


/// <summary>
/// System.Serializable使得类中的字段可以在Inspector界面中显示和编辑
/// 物品详细信息设置
/// </summary>
[System.Serializable]
public class ItemDetails
{
    /// <summary>
    /// 物品描述（名称）
    /// </summary>
    public string itemDescription;//这个字段放在第一位，那么unity中可视窗口会直接显示物体描述而非element 0
    /// <summary>
    /// 物品代码
    /// </summary>
    public int itemCode;
    /// <summary>
    /// 物品类型
    /// </summary>
    public ItemType itemType;
    
    /// <summary>
    /// 物品图片
    /// </summary>
    public Sprite itemSprite;
    /// <summary>
    /// 物品详情
    /// </summary>
    public string itemLongDescription;
    /// <summary>
    /// 物品使用范围 基于网格  网格光标的半径
    /// </summary>
    public short itemUseGridRadius;
    /// <summary>
    /// 物品使用范围，基于距离
    /// </summary>
    public float itemUseRadius;
    /// <summary>
    /// 是否是初始物品？？？
    /// </summary>
    public bool isStartingItem;
    /// <summary>
    /// 物品是否能被拾起
    /// </summary>
    public bool canBePickedUp;
    /// <summary>
    /// 物品是否能被丢弃
    /// </summary>
    public bool canBeDropped;
    /// <summary>
    /// 物品是否能被玩家食用
    /// </summary>
    public bool canBeEaten;
    /// <summary>
    /// 物品是否能被举起
    /// </summary>
    public bool canBeCarried;
}
