
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

/// <summary>
/// ��������ö��ֵ
/// </summary>
public enum AnimationName
{
    #region ���ж���
    idleDown,
    idleUp,
    idleLeft,
    idleRight,
    #endregion
    #region ���߶���
    walkUp,
    walkDown,
    walkLeft,
    walkRight,
    #endregion
    #region �ܲ�����
    runUp,
    runDown,
    runLeft,
    runRight,
    #endregion
    #region ʹ�ù��߶���
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    #endregion
    #region �Ӷ����߶���
    swingToolUp,
    swingToolDown,
    swingToolLeft,
    swingToolRight,
    #endregion
    #region �������ﶯ��
    liftToolUp,
    liftToolDown,
    liftToolLeft,
    liftToolRight,
    #endregion
    #region ����������
    holdToolUp,
    holdToolDown,
    holdToolLeft,
    holdToolRight,
    #endregion
    #region ����������
    pickDown,
    pickUp,
    pickLeft,
    pickRight,
    #endregion
    count
}


/// <summary>
/// ��ɫ��λ����
/// </summary>
public enum CharacterPartAnimator
{
    Body,//����
    Arms,//�ֱ�
    Hair,//ͷ��
    Tool,//����
    Hat,//ñ��
    count
}

/// <summary>
/// ����Ԥ���������ɫ
/// </summary>
public enum PartVariantColour
{
    none,
    count
}

/// <summary>
/// ����Ԥ��������
/// </summary>
public enum PartVariantType
{
    none,
    carry,//Я����Ʒ
    hoe,//��ͷ
    pickaxe,//�����
    axe,//��ͷ
    scythe,//����
    wateringCan,//���
    count
}

/// <summary>
/// ����ö��ֵ
/// </summary>
public enum Season
{
    Spring,
    Summer,
    Autum,
    Winter,
    none,
    count
}

