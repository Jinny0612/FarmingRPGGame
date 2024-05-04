
//管理枚举值

/// <summary>
/// 手持工具效果枚举值
/// </summary>
public enum ToolEffect
{
    none,//无
    watering//浇水
}

//移动方向
public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

/// <summary>
/// 物品类型枚举值
/// </summary>
public enum ItemType
{
    Seed,//种子
    Commodity,//商品
    Watering_tool,//水壶
    Hoeing_tool,//锄头
    Chopping_tool,//斧头
    Breaking_tool,//稿子
    Reaping_tool,//收割工具
    Collecting_tool,//收集工具
    Reapable_scenary,//可收获的场景（例如浆果灌木丛）
    Furniture,//家具
    none,
    count//列表中物品的数量
}

/// <summary>
/// 物品存储位置枚举值
/// </summary>
public enum InventoryLocation
{
    player,//玩家背包
    chest,//储物箱
    count//数量
}