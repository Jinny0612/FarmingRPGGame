
/// <summary>
/// 网格属性详情  包括是否可放置物品是否可挖掘以及种植信息等等
/// </summary>
[System.Serializable]
public class GridPropertyDetails 
{
    public int gridX;
    public int gridY;
    public bool isDiggable = false;
    public bool canDropItem = false;
    public bool canPlaceFurniture = false;
    public bool isPath = false;
    public bool isNPCObstacle = false;

    #region 种植相关
    /// <summary>
    /// 当前地块距离被挖掘的时间
    /// </summary>
    public int daysSinceDug = -1;
    /// <summary>
    /// 当前地块距离被浇水的时间
    /// </summary>
    public int daysSinceWatered = -1;
    /// <summary>
    /// 当前地块种子物品编号
    /// </summary>
    public int seedItemCode = -1;
    /// <summary>
    /// 当前地块生长天数
    /// </summary>
    public int growthDays = -1;
    /// <summary>
    /// 针对可多次收获的作物  距离上一次收获的时间
    /// </summary>
    public int daysSinceLastHarvest = -1;
    #endregion

    public GridPropertyDetails() { }
}
