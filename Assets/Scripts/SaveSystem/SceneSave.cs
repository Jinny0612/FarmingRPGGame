using System.Collections.Generic;

/// <summary>
/// ������Ϣ�洢
/// </summary>
[System.Serializable]
public class SceneSave
{
    /// <summary>
    /// ������Ʒ�б���
    /// </summary>
    public List<SceneItem> listSceneItem;
    /// <summary>
    /// �������������ֵ�
    /// </summary>
    public Dictionary<string, GridPropertyDetails> gridPropertyDetailsDictionary;
}
