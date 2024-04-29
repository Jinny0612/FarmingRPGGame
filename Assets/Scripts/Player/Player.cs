
using System;
using UnityEditor.Search;
using UnityEngine;

/// <summary>
/// ��ҿ�����  ����
/// ������ֻ��һ�����
/// </summary>
public class Player : SingletonMonoBehvior<Player>
{
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

    private new Rigidbody2D rigidbody2D;

    private Direction playerDirection;

    private float movementSpeed;

    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region �������

        //������Ҷ���������
        ResetAnimationTriggers();

        //�������ƶ�����
        PlayerMovementInput();

        //��ⲽ��/�ܲ�����
        PlayerWalkInput();

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
}
