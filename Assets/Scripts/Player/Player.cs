
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

/// <summary>
/// 玩家控制类  单例
/// 场景中只有一个玩家
/// </summary>
public class Player : SingletonMonoBehvior<Player>
{

    #region 角色行为

    private float yInput;
    private float xInput;
    private bool isWalking;
    private bool isRunning;
    private bool isIdle;
    private bool isCarrying = false;
    private bool isUsingToolUp;
    private bool isUsingToolDown;
    private bool isUsingToolRight;
    private bool isUsingToolLeft;
    private bool isSwingingToolUp;
    private bool isSwingingToolDown;
    private bool isSwingingToolRight;
    private bool isSwingingToolLeft;
    private bool isLiftingToolUp;
    private bool isLiftingToolDown;
    private bool isLiftingToolRight;
    private bool isLiftingToolLeft;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isPickingRight;
    private bool isPickingLeft;
    private ToolEffect toolEffect = ToolEffect.none;

    #endregion

    #region 角色动画切换相关

    /// <summary>
    /// 动画覆盖，用于更改动画控制器中的动画剪辑，可以在运行时替换动画剪辑
    /// 从而实现角色在不同状态、行为或者环境下播放不同的动画，并且无需修改动画剪辑本身
    /// </summary>
    private AnimationOverrides animationOverrides;
    /// <summary>
    /// 角色属性自定义列表
    /// 传递给animationOverrides的角色属性列表
    /// </summary>
    private List<CharacterAttribute> characterAttributeCustomissationList;

    /// <summary>
    /// 已经设置好物品的精灵渲染器
    /// </summary>
    [Tooltip("应在perfab中使用已配备的物品精灵渲染器进行填充")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;
    /// <summary>
    /// 角色可切换的属性，手臂
    /// </summary>
    private CharacterAttribute armsCharacterAttribute;
    /// <summary>
    /// 角色可切换的属性，工具
    /// </summary>
    private CharacterAttribute toolCharacterAttribute;

    #endregion


    /// <summary>
    /// 网格光标
    /// </summary>
    private GridCursor gridCursor;
    /// <summary>
    /// 可收割物品的光标
    /// </summary>
    private Cursor cursor;

    /// <summary>
    /// 禁用角色使用工具
    /// </summary>
    private bool playerToolUseDisabled = false;

    #region 等待时间

    /// <summary>
    /// 使用工具后暂停
    /// </summary>
    private WaitForSeconds afterUseToolAnimationPause;
    /// <summary>
    /// 使用工具时暂停
    /// </summary>
    private WaitForSeconds useToolAnimationPause;
    /// <summary>
    /// 举起工具后暂停
    /// </summary>
    private WaitForSeconds afterLiftToolAnimationPause;
    /// <summary>
    /// 举起工具时暂停
    /// </summary>
    private WaitForSeconds liftToolAnimationPause;

    #endregion

    /// <summary>
    /// 主相机
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// 刚体组件
    /// </summary>
    private new Rigidbody2D rigidbody2D;
    /// <summary>
    /// 角色方向，用于存档
    /// </summary>
    private Direction playerDirection;
    /// <summary>
    /// 角色移动速度
    /// </summary>
    private float movementSpeed;
    /// <summary>
    /// 是否禁用玩家输入，即无法移动
    /// </summary>
    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();
        //实例化可切换的角色属性
        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.Arms, PartVariantColour.none, PartVariantType.none);
        toolCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.Tool, PartVariantColour.none, PartVariantType.hoe);
        
        characterAttributeCustomissationList = new List<CharacterAttribute>();

        //获取主相机的引用
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadFadeOutEvent += DisablePlayerInputAndResetMovement;
        EventHandler.AfterSceneLoadFadeInEvent += EnablePlayerInput;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadFadeOutEvent -= DisablePlayerInputAndResetMovement;
        EventHandler.AfterSceneLoadFadeInEvent -= EnablePlayerInput;
    }

    private void Start()
    {
        gridCursor = FindObjectOfType<GridCursor>();
        cursor = FindObjectOfType<Cursor>();

        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);

        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
    }

    private void Update()
    {
        //玩家输入未禁用，玩家可以移动
        if(!PlayerInputIsDisabled)
        {
            #region 玩家输入

            //重置玩家动画触发器
            ResetAnimationTriggers();

            //检测玩家移动输入
            PlayerMovementInput();

            //检测步行/跑步输入
            PlayerWalkInput();

            //TODO: 测试时间管理的
            PlayerTestInput();

            PlayerClickInput();

            //将玩家行为事件发送给监听者
            EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying,
                toolEffect,
                isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
                isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
                isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
                isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
                false, false, false, false
                );

            #endregion
        }
        


    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    /// <summary>
    /// 玩家移动控制  与物理逻辑相关
    /// </summary>
    private void PlayerMovement()
    {
        //与Time.deltaTime相关的都要放到FixedUpdate中
        Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);

        rigidbody2D.MovePosition(rigidbody2D.position +  move);
    }


    /// <summary>
    /// 检测步行或者跑步输入
    /// </summary>
    private void PlayerWalkInput()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //按下左/右shift,切换走路
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            //不按默认跑步
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }
    }

    /// <summary>
    /// 玩家左键点击输入
    /// </summary>
    private void PlayerClickInput()
    {
        if (!playerToolUseDisabled)
        {
            //PayerToolUseDisabled == false，即角色可以使用工具

            if (Input.GetMouseButton(0))
            {
                if (gridCursor.CursorIsEnabled || cursor.CursorIsEnabled)
                {
                    //获取光标和角色的坐标
                    Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();
                    Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();

                    ProcessPlayerClickInput(cursorGridPosition,playerGridPosition);
                }
            }
        }
        
    }

    /// <summary>
    /// 执行玩家左键点击
    /// </summary>
    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        //重置移动相关属性，避免扔物品时角色移动了
        ResetMovement();

        //获取角色方向
        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);
        //获取光标位置网格的属性详情
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails != null)
        {
            switch(itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputSeed(itemDetails);
                    }
                    break;

                case ItemType.Commodity:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputCommodity(itemDetails);
                    }
                    break;

                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Reaping_tool:
                    ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
                    break;

                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
                default:
                    break;
            }
        }

    }

    /// <summary>
    /// 当角色点击使用工具时的处理逻辑
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="itemDetails"></param>
    /// <param name="playerDirection"></param>
    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if(gridCursor.CursorPositionIsValid)
                {
                    //在当前光标处锄地
                    HoeGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;

            case ItemType.Watering_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    WaterGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;

            case ItemType.Reaping_tool:
                if (cursor.CursorPositionIsValid)
                {
                    // 获取玩家方向
                    playerDirection = GetPlayerDirection(cursor.GetWorldPositionForCursor(), GetPlayerCenterPosition());
                    // 根据朝向触发不同收割动画
                    ReapInPlayerDirectionAtCursor(itemDetails, playerDirection);
                }
                break;

            default : break;
        }
    }

    /// <summary>
    /// 在光标处浇水
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="playerDirection"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {
        // 触发动画
        StartCoroutine(WaterGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    }


    /// <summary>
    /// 在光标处浇水的例程
    /// </summary>
    /// <param name="playerDirection"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private IEnumerator WaterGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {
        // 禁止玩家输入和使用工具
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = false;

        // 设置工具动画为喷壶
        toolCharacterAttribute.partVariantType = PartVariantType.wateringCan;
        characterAttributeCustomissationList.Clear();
        characterAttributeCustomissationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);

        // todo :需要先判断喷壶中是否有水
        toolEffect = ToolEffect.watering;

        if(playerDirection == Vector3Int.right)
        {
            isLiftingToolRight = true;
        }
        else if(playerDirection == Vector3Int.left)
        {
            isLiftingToolLeft = true;
        }
        else if(playerDirection == Vector3Int.up)
        {
            isLiftingToolUp = true;
        }
        else if (playerDirection == Vector3Int.down)
        {
            isLiftingToolDown = true;
        }
        // 暂停指定浇水中的事件
        yield return liftToolAnimationPause;

        // 设置当前网格不能再被浇水
        if(gridPropertyDetails.daysSinceWatered == -1)
        {
            // 网格之前未被浇水过，设置为已浇水
            gridPropertyDetails.daysSinceWatered = 0;
        }

        // 设置浇水的网格属性
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);
        // 显示浇水的网格
        GridPropertiesManager.Instance.DisplayWateredGround(gridPropertyDetails);

        // 动画结束后暂定指定时间
        yield return afterLiftToolAnimationPause;

        // 恢复角色输入和工具使用
        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    /// <summary>
    /// 在光标处锄地
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="playerDirection"></param>
    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    {
        //触发动画
        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    }

    /// <summary>
    /// 锄地动画触发
    /// </summary>
    /// <param name="playerDirection"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <returns></returns>
    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    {
        //触发动画前禁止角色移动和切换使用别的工具
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        //设置动态替换锄地动画剪辑
        toolCharacterAttribute.partVariantType = PartVariantType.hoe;
        characterAttributeCustomissationList.Clear();//清空
        characterAttributeCustomissationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);

        //判断动画方向
        if(playerDirection == Vector3Int.right)
        {
            isUsingToolRight = true;
        }
        else if(playerDirection == Vector3Int.left)
        {
            isUsingToolLeft = true;
        }
        else if(playerDirection == Vector3Int.up)
        {
            isUsingToolUp = true;
        }
        else if(playerDirection == Vector3Int.down)
        {
            isUsingToolDown = true;
        }
        //使用工具时暂停
        yield return useToolAnimationPause;

        //更改网格属性中的挖掘信息
        if(gridPropertyDetails.daysSinceDug == -1)
        {
            //从未挖掘过 改为 距离被挖掘0天即今天被挖掘
            gridPropertyDetails.daysSinceDug = 0;
        }
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        // 显示锄地挖掘的网格
        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);

        //挖掘后暂停  防止动画触发得太快了
        yield return afterUseToolAnimationPause;

        //动画完全结束，解除对角色移动和使用工具的禁止
        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    private void ReapInPlayerDirectionAtCursor(ItemDetails itemDetails, Vector3Int playerDirection)
    {
        StartCoroutine(ReapInPlayerDirectionAtCursorRoutine(itemDetails, playerDirection));
    }

    private IEnumerator ReapInPlayerDirectionAtCursorRoutine(ItemDetails itemDetails, Vector3Int playerDirection)
    {
        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        toolCharacterAttribute.partVariantType = PartVariantType.scythe;
        characterAttributeCustomissationList.Clear();
        characterAttributeCustomissationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);

        UseToolInPlayerDirection(itemDetails, playerDirection);

        yield return useToolAnimationPause;

        PlayerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }

    private void UseToolInPlayerDirection(ItemDetails equippedItemDetails, Vector3Int playerDirection)
    {
        if (Input.GetMouseButton(0))
        {
            switch (equippedItemDetails.itemType)
            {
                case ItemType.Reaping_tool:
                    if(playerDirection == Vector3Int.right)
                    {
                        isSwingingToolRight = true;
                    }
                    else if (playerDirection == Vector3Int.left)
                    {
                        isSwingingToolLeft = true;
                    }
                    else if(playerDirection == Vector3Int.up)
                    {
                        isSwingingToolUp = true;
                    }
                    else if(playerDirection == Vector3Int.down)
                    {
                        isSwingingToolDown = true;
                    }
                    break;
            }

            Vector2 point = new Vector2(GetPlayerCenterPosition().x + (playerDirection.x * (equippedItemDetails.itemUseRadius / 2f)), GetPlayerCenterPosition().y
                    + playerDirection.y * (equippedItemDetails.itemUseRadius / 2f));

            Vector2 size = new Vector2(equippedItemDetails.itemUseRadius, equippedItemDetails.itemUseRadius);

            Item[] itemArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<Item>(Settings.maxCollidersToTestPerReapSwing, point, size, 0f);

            int reapableItemCount = 0;

            for(int i = itemArray.Length - 1; i >= 0; i--)
            {
                if (itemArray[i] != null)
                {
                    if (InventoryManager.Instance.GetItemDetails(itemArray[i].ItemCode).itemType == ItemType.Reapable_scenary)
                    {
                        // 收获效果应该出现的位置
                        Vector3 effectPosition = new Vector3(itemArray[i].transform.position.x, itemArray[i].transform.position.y + Settings.gridCellSize / 2f,
                            itemArray[i].transform.position.z);

                        EventHandler.CallHarvestActionEffectEvent(effectPosition, HarvestActionEffect.reaping);

                        Destroy(itemArray[i].gameObject);

                        reapableItemCount++;
                        if(reapableItemCount >= Settings.maxTargetComponentsToDestroyPerReapSwing)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 对比光标和角色位置来判断方向
    /// </summary>
    /// <param name="cursorGridPosition"></param>
    /// <param name="playerGridPosition"></param>
    /// <returns></returns>
    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if(cursorGridPosition.x > playerGridPosition.x)
        {
            //光标在角色右边
            return Vector3Int.right;
        }
        else if(cursorGridPosition.x < playerGridPosition.x)
        {
            return Vector3Int.left;
        }
        else if(cursorGridPosition.y > playerGridPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }

    /// <summary>
    /// 获取player方向
    /// </summary>
    /// <param name="cursorPosition"></param>
    /// <param name="playerPosition"></param>
    /// <returns></returns>
    private Vector3Int GetPlayerDirection(Vector3 cursorPosition, Vector3 playerPosition)
    {
        if(cursorPosition.x > playerPosition.x &&
            cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f) &&
            cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f))
        {
            return Vector3Int.right;
        }
        else if( cursorPosition.x < playerPosition.x
                &&
                cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f)
                &&
                cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f))
        {
            return Vector3Int.left;
        }
        else if(cursorPosition.y > playerPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }
    }

    /// <summary>
    /// 执行玩家点击商品类物品事件
    /// </summary>
    /// <param name="itemDetails"></param>
    private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails)
    {
        if(itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }

    /// <summary>
    /// 执行玩家点击种子类物品事件
    /// </summary>
    /// <param name="itemDetails"></param>
    private void ProcessPlayerClickInputSeed(ItemDetails itemDetails)
    {
        if(itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }

    //玩家移动操作
    private void PlayerMovementInput()
    {
        //获取水平和垂直方向的输入
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if(xInput != 0 && yInput != 0)
        {
            //同时输入xy方向，对移动距离进行处理
            //等腰直角三角形ABC,ABAC垂直，D为AC中点，移动方向为AD方向
            //单位时间内移动距离假定为1，那么AD=1
            //AB = AC = 根号2，E为AB中点，那么AE = 根号2 / 2 即约为0.71
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        if(xInput != 0 ||  yInput != 0)
        {
            //只有一个方向输入
            isRunning = true; 
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;

            //捕获玩家方向，便于游戏存档
            if(xInput < 0)
            {
                playerDirection = Direction.left;
            }
            else if(xInput > 0)
            {
                playerDirection = Direction.right;
            }
            else if(yInput < 0)
            {
                playerDirection = Direction.down;
            }
            else
            {
                playerDirection = Direction.up;
            }
        } 
        else if(xInput == 0 && yInput == 0)
        {
            //两个方向都没有输入
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }

    /// <summary>
    /// 重置玩家动画触发器
    /// 确保触发器确实被重置了
    /// </summary>
    private void ResetAnimationTriggers()
    {
        isUsingToolUp = false;
        isUsingToolDown = false;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isPickingRight = false;
        isPickingLeft = false;
        toolEffect = ToolEffect.none;
    }

    /// <summary>
    /// 禁用角色输入并重置移动事件
    /// </summary>
    public void DisablePlayerInputAndResetMovement()
    {
        //禁用角色输入，即角色暂时无法移动
        DisablePlayerInput();
        ResetMovement();

        //发布角色移动事件
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            false, false, false, false);
    }

    // TODO: REMOVE
    private void PlayerTestInput()
    {
        if (Input.GetKey(KeyCode.T))
        {
            //按分钟推进时间
            TimeManager.Instance.TestAdvanceGameMinute();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            //按天推进时间
            TimeManager.Instance.TestAdvanceGameDay();
        }

        
    }

    /// <summary>
    /// 重置角色移动信息
    /// </summary>
    private void ResetMovement()
    {
        xInput = 0f;
        yInput = 0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }

    /// <summary>
    /// 禁用角色输入
    /// </summary>
    public void DisablePlayerInput()
    {
        PlayerInputIsDisabled = true;
    }

    /// <summary>
    /// 启用角色输入
    /// </summary>
    public void EnablePlayerInput()
    {
        PlayerInputIsDisabled = false;
    }

    /// <summary>
    /// 将世界坐标转换为屏幕区域坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerViewportPosition()
    {
        //将世界坐标转换为视口坐标的方法。视口坐标是相对于相机视图的归一化坐标系，
        //范围从 (0,0) 到 (1,1)，其中 (0,0) 表示视图的左下角，(1,1) 表示视图的右上角。
        //这在许多情况下都很有用，例如用于屏幕空间特效、UI 元素的定位等。
        return mainCamera.WorldToViewportPoint(transform.position);
    }

    /// <summary>
    /// 显示手上拿着的物品
    /// </summary>
    /// <param name="itemCode"></param>
    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            //获取拿着的物品的图片
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f,1f,1f,1f);
            //设置动画属性，手上携带物品
            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomissationList.Clear();
            characterAttributeCustomissationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);
            //正在携带物品
            isCarrying = true;
        }
    }

    /// <summary>
    /// 获取player的中心点位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerCenterPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + Settings.playerCenterYOffset, transform.position.z);
    }

    /// <summary>
    /// 清除携带物品的显示
    /// </summary>
    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f,1f,1f,0f);

        //手臂携带动画参数初始化
        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomissationList.Clear();
        characterAttributeCustomissationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);

        isCarrying = false;
    }
}
