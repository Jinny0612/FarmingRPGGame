
//����ö��ֵ

/// <summary>
/// �ֳֹ���Ч��ö��ֵ
/// </summary>
public enum ToolEffect
{
    none,//��
    watering//��ˮ
}

//�ƶ�����
public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

/// <summary>
/// ��Ʒ����ö��ֵ
/// </summary>
public enum ItemType
{
    Seed,//����
    Commodity,//��Ʒ
    Watering_tool,//ˮ��
    Hoeing_tool,//��ͷ
    Chopping_tool,//��ͷ
    Breaking_tool,//����
    Reaping_tool,//�ո��
    Collecting_tool,//�ռ�����
    Reapable_scenary,//���ջ�ĳ��������罬����ľ�ԣ�
    Furniture,//�Ҿ�
    none,
    count//�б�����Ʒ������
}

/// <summary>
/// ��Ʒ�洢λ��ö��ֵ
/// </summary>
public enum InventoryLocation
{
    player,//��ұ���
    chest,//������
    count//����
}