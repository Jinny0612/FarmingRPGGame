
/// <summary>
/// ��������
/// </summary>
[System.Serializable]
public class GridProperty
{
    /// <summary>
    /// ��������
    /// </summary>
    public GridCoordinate gridCoordinate;
    /// <summary>
    /// �����bool����
    /// </summary>
    public GridBoolProperty gridBoolProperty;
    /// <summary>
    /// ����boolֵ
    /// </summary>
    public bool gridBoolValue = false;

    public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)
    {
        this.gridCoordinate = gridCoordinate;
        this.gridBoolProperty = gridBoolProperty;
        this.gridBoolValue = gridBoolValue;
    }
}
