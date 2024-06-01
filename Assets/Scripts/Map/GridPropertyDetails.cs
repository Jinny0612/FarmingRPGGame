
/// <summary>
/// ������������  �����Ƿ�ɷ�����Ʒ�Ƿ���ھ��Լ���ֲ��Ϣ�ȵ�
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

    #region ��ֲ���
    /// <summary>
    /// ��ǰ�ؿ���뱻�ھ��ʱ��
    /// </summary>
    public int daysSinceDug = -1;
    /// <summary>
    /// ��ǰ�ؿ���뱻��ˮ��ʱ��
    /// </summary>
    public int daysSinceWatered = -1;
    /// <summary>
    /// ��ǰ�ؿ�������Ʒ���
    /// </summary>
    public int seedItemCode = -1;
    /// <summary>
    /// ��ǰ�ؿ���������
    /// </summary>
    public int growthDays = -1;
    /// <summary>
    /// ��Կɶ���ջ������  ������һ���ջ��ʱ��
    /// </summary>
    public int daysSinceLastHarvest = -1;
    #endregion

    public GridPropertyDetails() { }
}
