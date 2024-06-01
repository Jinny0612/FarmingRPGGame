using System.Collections.Generic;
/// <summary>
/// 游戏物体信息存储
/// </summary>
[System.Serializable]
public class GameObjectSave
{
    /// <summary>
    /// 场景数据字典，key-场景名称，value-场景数据存储信息
    /// </summary>
    public Dictionary<string, SceneSave> sceneData;

    public GameObjectSave()
    {
        sceneData = new Dictionary<string, SceneSave>();
    }

    public GameObjectSave(Dictionary<string, SceneSave> sceneData)
    {
        this.sceneData = sceneData;
    }
}
