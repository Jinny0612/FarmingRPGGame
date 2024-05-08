
/// <summary>
/// ��ɫ����
/// </summary>
[System.Serializable]
public struct CharacterAttribute
{
    /// <summary>
    /// ��ɫ���ֶ���
    /// </summary>
    public CharacterPartAnimator characterPart;
    /// <summary>
    /// ��ɫ����Ԥ���������ɫ
    /// </summary>
    public PartVariantColour partVariantColour;
    /// <summary>
    /// ��ɫ����Ԥ�����������
    /// </summary>
    public PartVariantType partVariantType;

    public CharacterAttribute(CharacterPartAnimator characterPart, PartVariantColour partVariantColour, PartVariantType partVariantType)
    {
        this.characterPart = characterPart;
        this.partVariantColour = partVariantColour;
        this.partVariantType = partVariantType;
    }
}
