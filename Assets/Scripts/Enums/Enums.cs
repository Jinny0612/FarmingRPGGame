
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
    Reapable_scenary,//可获得的场景？？？
    Furniture,//家具
    none,
    count//列表中物品的数量
}
