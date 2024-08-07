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

    # region 玩家移动参数
    public const float runningSpeed = 8f;//5.333f;
    public const float walkingSpeed = 4f;//2.666f;
    
    #endregion

    #region 库存相关
    /// <summary>
    /// 玩家初始背包容量
    /// </summary>
    public static int playerInitialInventoryCapacity = 24;
    /// <summary>
    /// 玩家最大背包容量
    /// </summary>
    public static int playerMaximumInventoryCapacity = 48;
    #endregion

    #region player

    /// <summary>
    /// 玩家中心点Y轴偏移量  0.875个单位
    /// 16像素为一单位
    /// </summary>
    public static float playerCenterYOffset = 0.875f;

    #endregion

    #region 玩家动画参数，名称与unity中设置的动画参数相同
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

    #endregion

    #region 工具描述
    public const string HoeingTool = "Hoe";
    public const string ChoppingTool = "Axe";
    public const string BreakingTool = "Pickaxe";
    public const string ReapingTool = "Scythe";
    public const string WateringTool = "Watering Can";
    public const string CollectingTool = "Basket";
    #endregion

    #region 收割作物

    /// <summary>
    /// 检测区域下方可检测到的最大物体数量
    /// </summary>
    public const int maxCollidersToTestPerReapSwing = 15;
    /// <summary>
    /// 每次收割最多收割数量（即摧毁被收割物体数量）
    /// </summary>
    public const int maxTargetComponentsToDestroyPerReapSwing = 2;

    #endregion

    #region 时间系统
    /// <summary>
    /// 单位游戏时间的现实秒数  大约每经过现实时间 0.7s 就过去 1 分钟游戏时间
    /// </summary>
    public const float secondsPerGameSecond = 0.012f;
    /// <summary>
    /// 使用工具时暂停的时间
    /// </summary>
    public static float useToolAnimationPause = 0.25f;
    /// <summary>
    /// 举起工具时暂停的时间
    /// </summary>
    public static float liftToolAnimationPause = 0.4f;
    /// <summary>
    /// 使用工具后暂停的时间
    /// </summary>
    public static float afterUseToolAnimationPause = 0.2f;
    /// <summary>
    /// 举起工具后暂停的时间
    /// </summary>
    public static float afterLiftToolAnimationPause = 0.4f;

    #endregion

    #region tilemap

    /// <summary>
    /// 网格单元大小  1单位
    /// </summary>
    public const float gridCellSize = 1f;
    /// <summary>
    /// 光标大小
    /// </summary>
    public static Vector2 cursorSize = Vector2.one;

    #endregion

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
