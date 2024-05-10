using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    /// <summary>
    /// �����ɫ
    /// </summary>
    [SerializeField] private GameObject character = null;
    /// <summary>
    /// ��ɫ������������
    /// </summary>
    [SerializeField] private SO_AnimationType[] soAnimationTypeArry = null;
    /// <summary>
    /// ͨ����������鹹���ļ��ϣ�key-����������value-����������Ϣ
    /// </summary>
    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;
    /// <summary>
    /// ͨ����������鹹���ļ��ϣ�key-����������Ϣ����ַ�����value-����������Ϣ
    /// </summary>
    private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey;

    // Start is called before the first frame update
    void Start()
    {
        animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();

        foreach(SO_AnimationType item in soAnimationTypeArry)
        {
            animationTypeDictionaryByAnimation.Add(item.animationClip, item);
        }

        animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();

        foreach(SO_AnimationType item in soAnimationTypeArry)
        {
            //todo �о�����keyû�취����Ψһ�ԣ�������Ҫ����һ��
            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            animationTypeDictionaryByCompositeAttributeKey.Add(key,item);
        }
    }

    /// <summary>
    /// ���ý�ɫ�Զ�����������������й����ж�̬���޸���Ϸ����ض�����Ϊ
    /// </summary>
    /// <param name="characterattributesList"></param>
    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterattributesList)
    {

        //Ϊÿһ����ɫ�������ö�����д������
        foreach(CharacterAttribute characterAttribute in characterattributesList)
        {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            //���������Ҫ�������ȡ��player������Ķ�������еĶ���������ͬ
            string animatorSOAssetName = characterAttribute.characterPart.ToString();

            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

            foreach(Animator animator in animatorsArray)
            {
                if(animator.name == animatorSOAssetName)
                {
                    currentAnimator = animator;
                    break;
                }
            }
            //AnimatorOverrideController����������ʱ�滻animator�Ķ�������
            //runtimeAnimatorController �� Unity �е�һ�����ԣ�
            //����������ʱָ����Ϸ������ Animator �����ʹ�õ� Animator Controller����������������
            //ͨ������������ԣ�����������Ϸ����ʱ��̬�ظ�����Ϸ����Ķ�����Ϊ����������ĳ�����Ԥ���塣
            //��ʹ�������Ը�����Ϸ�еĸ����������������̬�ؿ��ƶ������ţ��Ӷ���������Ϸ�Ľ����ԺͿ����ԡ�
            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);

            foreach(AnimationClip animationClip in animationsList)
            {

                SO_AnimationType so_AnimationType;
                bool foundAnimation = animationTypeDictionaryByAnimation.TryGetValue(animationClip,out so_AnimationType);

                if (foundAnimation)
                {
                    string key = characterAttribute.characterPart.ToString() + characterAttribute.partVariantColour.ToString()
                        + characterAttribute.partVariantType.ToString() + so_AnimationType.animationName.ToString();

                    SO_AnimationType swapSO_AnimationType;
                    bool foundSwapAnimation = animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key,out swapSO_AnimationType);

                    if (foundSwapAnimation)
                    {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;

                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip,AnimationClip>(animationClip, swapAnimationClip));
                    }
                }
            }

            aoc.ApplyOverrides(animsKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
    }
}
