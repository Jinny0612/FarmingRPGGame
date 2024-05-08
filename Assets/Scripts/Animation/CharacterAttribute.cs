
/// <summary>
/// 角色属性
/// </summary>
[System.Serializable]
public struct CharacterAttribute
{
    /// <summary>
    /// 角色部分动画
    /// </summary>
    public CharacterPartAnimator characterPart;
    /// <summary>
    /// 角色部分预制体变体颜色
    /// </summary>
    public PartVariantColour partVariantColour;
    /// <summary>
    /// 角色部分预制体变体类型
    /// </summary>
    public PartVariantType partVariantType;

    public CharacterAttribute(CharacterPartAnimator characterPart, PartVariantColour partVariantColour, PartVariantType partVariantType)
    {
        this.characterPart = characterPart;
        this.partVariantColour = partVariantColour;
        this.partVariantType = partVariantType;
    }
}
