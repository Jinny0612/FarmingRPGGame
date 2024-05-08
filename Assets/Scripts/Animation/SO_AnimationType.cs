using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色动画类型管理
/// </summary>
[CreateAssetMenu(fileName = "so_AnimationType", menuName = "Scriptable Objects/Animation/Animation Type")]
public class SO_AnimationType : ScriptableObject
{
    /// <summary>
    /// 动画剪辑
    /// </summary>
    public AnimationClip animationClip;
    /// <summary>
    /// 动画名称
    /// </summary>
    public AnimationName animationName;
    /// <summary>
    /// 角色动画部位
    /// </summary>
    public CharacterPartAnimator characterPart;
    /// <summary>
    /// 部位预制体变体颜色
    /// </summary>
    public PartVariantColour partVariantColour;
    /// <summary>
    /// 部位预制体变体类型
    /// </summary>
    public PartVariantType partVariantType;
}
