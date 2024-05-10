using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    /// <summary>
    /// 人物角色
    /// </summary>
    [SerializeField] private GameObject character = null;
    /// <summary>
    /// 角色动画类型数组
    /// </summary>
    [SerializeField] private SO_AnimationType[] soAnimationTypeArry = null;
    /// <summary>
    /// 通过上面的数组构建的集合，key-动画剪辑，value-动画类型信息
    /// </summary>
    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;
    /// <summary>
    /// 通过上面的数组构建的集合，key-动画类型信息组合字符串，value-动画类型信息
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
            //todo 感觉这里key没办法保持唯一性，后续需要测试一下
            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            animationTypeDictionaryByCompositeAttributeKey.Add(key,item);
        }
    }

    /// <summary>
    /// 设置角色自定义参数，用于在运行过程中动态地修改游戏对象地动画行为
    /// </summary>
    /// <param name="characterattributesList"></param>
    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterattributesList)
    {

        //为每一个角色属性设置动画重写控制器
        foreach(CharacterAttribute characterAttribute in characterattributesList)
        {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            //这个参数，要与下面获取的player子物体的动画组件中的动画名称相同
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
            //AnimatorOverrideController允许在运行时替换animator的动画剪辑
            //runtimeAnimatorController 是 Unity 中的一个属性，
            //用于在运行时指定游戏对象上 Animator 组件所使用的 Animator Controller（动画控制器）。
            //通过设置这个属性，您可以在游戏运行时动态地更改游戏对象的动画行为，而无需更改场景或预制体。
            //这使得您可以根据游戏中的各种条件和情况来动态地控制动画播放，从而增加了游戏的交互性和可玩性。
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
