using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ�������͹���
/// </summary>
[CreateAssetMenu(fileName = "so_AnimationType", menuName = "Scriptable Objects/Animation/Animation Type")]
public class SO_AnimationType : ScriptableObject
{
    /// <summary>
    /// ��������
    /// </summary>
    public AnimationClip animationClip;
    /// <summary>
    /// ��������
    /// </summary>
    public AnimationName animationName;
    /// <summary>
    /// ��ɫ������λ
    /// </summary>
    public CharacterPartAnimator characterPart;
    /// <summary>
    /// ��λԤ���������ɫ
    /// </summary>
    public PartVariantColour partVariantColour;
    /// <summary>
    /// ��λԤ�����������
    /// </summary>
    public PartVariantType partVariantType;
}
