
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

/// <summary>
/// 动画名称枚举值
/// </summary>
public enum AnimationName
{
    #region 空闲动画
    idleDown,
    idleUp,
    idleLeft,
    idleRight,
    #endregion
    #region 行走动画
    walkUp,
    walkDown,
    walkLeft,
    walkRight,
    #endregion
    #region 跑步动画
    runUp,
    runDown,
    runLeft,
    runRight,
    #endregion
    #region 使用工具动画
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    #endregion
    #region 挥动工具动画
    swingToolUp,
    swingToolDown,
    swingToolLeft,
    swingToolRight,
    #endregion
    #region 拔起作物动画
    liftToolUp,
    liftToolDown,
    liftToolLeft,
    liftToolRight,
    #endregion
    #region 举起东西动画
    holdToolUp,
    holdToolDown,
    holdToolLeft,
    holdToolRight,
    #endregion
    #region 捡起东西动画
    pickDown,
    pickUp,
    pickLeft,
    pickRight,
    #endregion
    count
}


/// <summary>
/// 角色部位动画
/// </summary>
public enum CharacterPartAnimator
{
    Body,//身体
    Arms,//手臂
    Hair,//头发
    Tool,//工具
    Hat,//帽子
    count
}

/// <summary>
/// 部件预制体变体颜色
/// </summary>
public enum PartVariantColour
{
    none,
    count
}

/// <summary>
/// 部件预制体类型
/// </summary>
public enum PartVariantType
{
    none,
    carry,//携带物品
    hoe,//锄头
    pickaxe,//鹤嘴锄
    axe,//斧头
    scythe,//镰刀
    wateringCan,//喷壶
    count
}

/// <summary>
/// 季节枚举值
/// </summary>
public enum Season
{
    Spring,
    Summer,
    Autum,
    Winter,
    none,
    count
}

