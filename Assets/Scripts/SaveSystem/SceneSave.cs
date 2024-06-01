using System.Collections.Generic;

/// <summary>
/// 场景信息存储
/// </summary>
[System.Serializable]
public class SceneSave
{
    /// <summary>
    /// 场景物品列表集合
    /// </summary>
    public List<SceneItem> listSceneItem;
    /// <summary>
    /// 网格属性详情字典
    /// </summary>
    public Dictionary<string, GridPropertyDetails> gridPropertyDetailsDictionary;
}
