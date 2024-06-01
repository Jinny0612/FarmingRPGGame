
/// <summary>
/// 网格属性
/// </summary>
[System.Serializable]
public class GridProperty
{
    /// <summary>
    /// 网格坐标
    /// </summary>
    public GridCoordinate gridCoordinate;
    /// <summary>
    /// 网格的bool属性
    /// </summary>
    public GridBoolProperty gridBoolProperty;
    /// <summary>
    /// 网格bool值
    /// </summary>
    public bool gridBoolValue = false;

    public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)
    {
        this.gridCoordinate = gridCoordinate;
        this.gridBoolProperty = gridBoolProperty;
        this.gridBoolValue = gridBoolValue;
    }
}
