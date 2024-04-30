using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 参数设置管理类
/// </summary>
public static class Settings 
{
    //遮盖玩家的物体颜色透明度的变化
    public const float fadeInSeconds = 0.25f;
    public const float fadeOutSeconds = 0.35f;
    /// <summary>
    /// 目标透明度
    /// </summary>
    public const float targetAlpha = 0.45f;

    //玩家移动参数
    public const float runningSpeed = 5.333f;
    public const float walkingSpeed = 2.666f;

    //与player动画器中的参数名相同
    //玩家动画参数
    public static int yInput;
    public static int xInput;
    public static int isWalking;
    public static int isRunning;
    public static int toolEffect;
    public static int isUsingToolUp;
    public static int isUsingToolDown;
    public static int isUsingToolRight;
    public static int isUsingToolLeft;
    public static int isSwingingToolUp;
    public static int isSwingingToolDown;
    public static int isSwingingToolRight;
    public static int isSwingingToolLeft;
    public static int isLiftingToolUp;
    public static int isLiftingToolDown;
    public static int isLiftingToolRight;
    public static int isLiftingToolLeft;
    public static int isHoldingToolUp;
    public static int isHoldingToolDown;
    public static int isHoldingToolRight;
    public static int isHoldingToolLeft;
    public static int isPickingUp;
    public static int isPickingDown;
    public static int isPickingRight;
    public static int isPickingLeft;

    //共享动画参数 (玩家 & NPC)
    public static int idleUp;
    public static int idleDown;
    public static int idleLeft;
    public static int idleRight;


    static Settings()
    {
        //玩家动画参数
        //Animator.StringToHash 是一个用于将动画参数名称转换为哈希值的静态方法。
        //在 Unity 中，动画系统使用哈希值来管理动画参数，而不是直接使用字符串。
        //使用哈希值可以提高性能，因为在运行时比较哈希值比比较字符串更快。
        yInput = Animator.StringToHash("yInput");
        xInput = Animator.StringToHash("xInput");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        toolEffect = Animator.StringToHash("toolEffect");
        isUsingToolUp = Animator.StringToHash("isUsingToolUp");
        isUsingToolDown = Animator.StringToHash("isUsingToolDown");
        isUsingToolRight = Animator.StringToHash("isUsingToolRight");
        isUsingToolLeft = Animator.StringToHash("isUsingToolLeft");
        isSwingingToolUp = Animator.StringToHash("isSwingingToolUp");
        isSwingingToolDown = Animator.StringToHash("isSwingingToolDown");
        isSwingingToolRight = Animator.StringToHash("isSwingingToolRight");
        isSwingingToolLeft = Animator.StringToHash("isSwingingToolLeft");
        isLiftingToolUp = Animator.StringToHash("isLiftingToolUp");
        isLiftingToolDown = Animator.StringToHash("isLiftingToolDown");
        isLiftingToolRight = Animator.StringToHash("isLiftingToolRight");
        isLiftingToolLeft = Animator.StringToHash("isLiftingToolLeft");
        isHoldingToolUp = Animator.StringToHash("isHoldingToolUp");
        isHoldingToolDown = Animator.StringToHash("isHoldingToolDown");
        isHoldingToolRight = Animator.StringToHash("isHoldingToolRight");
        isHoldingToolLeft = Animator.StringToHash("isHoldingToolLeft");
        isPickingUp = Animator.StringToHash("isPickingUp");
        isPickingDown = Animator.StringToHash("isPickingDown");
        isPickingRight = Animator.StringToHash("isPickingRight");
        isPickingLeft = Animator.StringToHash("isPickingLeft");

        //共享动画参数 (玩家 & NPC)
        idleUp = Animator.StringToHash("idleUp");
        idleDown = Animator.StringToHash("idleDown");
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");
    }
    
}
