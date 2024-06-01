
/// <summary>
/// 场景内的物品
/// </summary>
[System.Serializable]
public class SceneItem 
{
    /// <summary>
    /// 物品编码
    /// </summary>
    public int itemCode;
    /// <summary>
    /// 物品位置
    /// </summary>
    public Vector3Serializable position;
    /// <summary>
    /// 物品名称
    /// </summary>
    public string itemName;

    public SceneItem()
    {
        position = new Vector3Serializable();
    }
}
