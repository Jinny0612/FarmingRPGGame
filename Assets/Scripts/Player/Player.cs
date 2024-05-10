
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

/// <summary>
/// ��ҿ�����  ����
/// ������ֻ��һ�����
/// </summary>
public class Player : SingletonMonoBehvior<Player>
{
    #region ��ɫ��Ϊ

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

    #region ��ɫ�����л����
    /// <summary>
    /// �������ǣ����ڸ��Ķ����������еĶ�������������������ʱ�滻��������
    /// �Ӷ�ʵ�ֽ�ɫ�ڲ�ͬ״̬����Ϊ���߻����²��Ų�ͬ�Ķ��������������޸Ķ�����������
    /// </summary>
    private AnimationOverrides animationOverrides;
    /// <summary>
    /// ��ɫ�����Զ����б�
    /// ���ݸ�animationOverrides�Ľ�ɫ�����б�
    /// </summary>
    private List<CharacterAttribute> characterAttributeCustomissationList;

    /// <summary>
    /// �Ѿ����ú���Ʒ�ľ�����Ⱦ��
    /// </summary>
    [Tooltip("Ӧ��perfab��ʹ�����䱸����Ʒ������Ⱦ���������")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;
    /// <summary>
    /// ��ɫ���л������ԣ��ֱ�
    /// </summary>
    private CharacterAttribute armsCharacterAttribute;
    /// <summary>
    /// ��ɫ���л������ԣ�����
    /// </summary>
    private CharacterAttribute toolCharacterAttribute;
    #endregion

    /// <summary>
    /// �����
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// �������
    /// </summary>
    private new Rigidbody2D rigidbody2D;
    /// <summary>
    /// ��ɫ�������ڴ浵
    /// </summary>
    private Direction playerDirection;
    /// <summary>
    /// ��ɫ�ƶ��ٶ�
    /// </summary>
    private float movementSpeed;
    /// <summary>
    /// �Ƿ����������룬���޷��ƶ�
    /// </summary>
    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();
        //ʵ�������л��Ľ�ɫ����
        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.Arms, PartVariantColour.none, PartVariantType.none);
        characterAttributeCustomissationList = new List<CharacterAttribute>();

        //��ȡ�����������
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //�������Υ���ã���ҿ����ƶ�
        if(!PlayerInputIsDisabled)
        {
            #region �������

            //������Ҷ���������
            ResetAnimationTriggers();

            //�������ƶ�����
            PlayerMovementInput();

            //��ⲽ��/�ܲ�����
            PlayerWalkInput();

            //TODO: ����ʱ������
            //PlayerTestInput();

            //�������Ϊ�¼����͸�������
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
    /// ����ƶ�����  �������߼����
    /// </summary>
    private void PlayerMovement()
    {
        //��Time.deltaTime��صĶ�Ҫ�ŵ�FixedUpdate��
        Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);

        rigidbody2D.MovePosition(rigidbody2D.position +  move);
    }


    /// <summary>
    /// ��ⲽ�л����ܲ�����
    /// </summary>
    private void PlayerWalkInput()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //������/��shift,�л���·
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            //����Ĭ���ܲ�
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }
    }

    //����ƶ�����
    private void PlayerMovementInput()
    {
        //��ȡˮƽ�ʹ�ֱ���������
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if(xInput != 0 && yInput != 0)
        {
            //ͬʱ����xy���򣬶��ƶ�������д���
            //����ֱ��������ABC,ABAC��ֱ��DΪAC�е㣬�ƶ�����ΪAD����
            //��λʱ�����ƶ�����ٶ�Ϊ1����ôAD=1
            //AB = AC = ����2��EΪAB�е㣬��ôAE = ����2 / 2 ��ԼΪ0.71
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        if(xInput != 0 ||  yInput != 0)
        {
            //ֻ��һ����������
            isRunning = true; 
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;

            //������ҷ��򣬱�����Ϸ�浵
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
            //��������û������
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }
    }

    /// <summary>
    /// ������Ҷ���������
    /// ȷ��������ȷʵ��������
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
    /// ���ý�ɫ���벢�����ƶ��¼�
    /// </summary>
    public void DisablePlayerInputAndResetMovement()
    {
        //���ý�ɫ���룬����ɫ��ʱ�޷��ƶ�
        DisablePlayerInput();
        ResetMovement();

        //������ɫ�ƶ��¼�
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            false, false, false, false);
    }

    // TODO: REMOVE
    /*private void PlayerTestInput()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //�������ƽ�ʱ��
            TimeManager.Instance.TestAdvanceGameMinute();
        }

        if(Input.GetKeyDown(KeyCode.G)) 
        { 
            //�����ƽ�ʱ��
            TimeManager.Instance.TestAdvanceGameDay();
        }
    }*/

    /// <summary>
    /// ���ý�ɫ�ƶ���Ϣ
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
    /// ���ý�ɫ����
    /// </summary>
    public void DisablePlayerInput()
    {
        PlayerInputIsDisabled = true;
    }

    /// <summary>
    /// ���ý�ɫ����
    /// </summary>
    public void EnablePlayerInput()
    {
        PlayerInputIsDisabled = false;
    }

    /// <summary>
    /// ����������ת��Ϊ��Ļ��������
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerViewportPosition()
    {
        //����������ת��Ϊ�ӿ�����ķ������ӿ�����������������ͼ�Ĺ�һ������ϵ��
        //��Χ�� (0,0) �� (1,1)������ (0,0) ��ʾ��ͼ�����½ǣ�(1,1) ��ʾ��ͼ�����Ͻǡ�
        //�����������¶������ã�����������Ļ�ռ���Ч��UI Ԫ�صĶ�λ�ȡ�
        return mainCamera.WorldToViewportPoint(transform.position);
    }

    /// <summary>
    /// ��ʾ�������ŵ���Ʒ
    /// </summary>
    /// <param name="itemCode"></param>
    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            //��ȡ���ŵ���Ʒ��ͼƬ
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f,1f,1f,1f);
            //���ö������ԣ�����Я����Ʒ
            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomissationList.Clear();
            characterAttributeCustomissationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);
            //����Я����Ʒ
            isCarrying = true;
        }
    }

    /// <summary>
    /// ���Я����Ʒ����ʾ
    /// </summary>
    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f,1f,1f,0f);

        //�ֱ�Я������������ʼ��
        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomissationList.Clear();
        characterAttributeCustomissationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomissationList);

        isCarrying = false;
    }
}
