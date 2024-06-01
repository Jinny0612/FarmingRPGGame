using System.Collections.Generic;
/// <summary>
/// ��Ϸ������Ϣ�洢
/// </summary>
[System.Serializable]
public class GameObjectSave
{
    /// <summary>
    /// ���������ֵ䣬key-�������ƣ�value-�������ݴ洢��Ϣ
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
