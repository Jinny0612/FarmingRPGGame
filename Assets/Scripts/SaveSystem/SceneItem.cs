
/// <summary>
/// �����ڵ���Ʒ
/// </summary>
[System.Serializable]
public class SceneItem 
{
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public int itemCode;
    /// <summary>
    /// ��Ʒλ��
    /// </summary>
    public Vector3Serializable position;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public string itemName;

    public SceneItem()
    {
        position = new Vector3Serializable();
    }
}
