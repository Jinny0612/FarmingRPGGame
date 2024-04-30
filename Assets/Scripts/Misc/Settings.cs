using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// �������ù�����
/// </summary>
public static class Settings 
{
    //�ڸ���ҵ�������ɫ͸���ȵı仯
    public const float fadeInSeconds = 0.25f;
    public const float fadeOutSeconds = 0.35f;
    /// <summary>
    /// Ŀ��͸����
    /// </summary>
    public const float targetAlpha = 0.45f;

    //����ƶ�����
    public const float runningSpeed = 5.333f;
    public const float walkingSpeed = 2.666f;

    //��player�������еĲ�������ͬ
    //��Ҷ�������
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

    //���������� (��� & NPC)
    public static int idleUp;
    public static int idleDown;
    public static int idleLeft;
    public static int idleRight;


    static Settings()
    {
        //��Ҷ�������
        //Animator.StringToHash ��һ�����ڽ�������������ת��Ϊ��ϣֵ�ľ�̬������
        //�� Unity �У�����ϵͳʹ�ù�ϣֵ��������������������ֱ��ʹ���ַ�����
        //ʹ�ù�ϣֵ����������ܣ���Ϊ������ʱ�ȽϹ�ϣֵ�ȱȽ��ַ������졣
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

        //���������� (��� & NPC)
        idleUp = Animator.StringToHash("idleUp");
        idleDown = Animator.StringToHash("idleDown");
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");
    }
    
}
