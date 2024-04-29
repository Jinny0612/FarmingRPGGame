using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementAnimationParameter : MonoBehaviour
{

    private Animator animator;

    //初始化
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //当游戏对象启用激活时，订阅事件

        EventHandler.MovementEvent += SetAnimationParameters;
    }

    private void OnDisable()
    {
        //当游戏对象非激活时，取消订阅
        EventHandler.MovementEvent -= SetAnimationParameters;
    }

    /// <summary>
    /// 设置动画参数
    /// </summary>
    private void SetAnimationParameters(float xinput, float yinput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, 
        ToolEffect toolEffect, 
        bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown, 
        bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown, 
        bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown, 
        bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown, 
        bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
    {
        animator.SetFloat(Settings.xInput, xinput);
        animator.SetFloat (Settings.yInput, yinput);
        animator.SetBool(Settings.isWalking, isWalking);
        animator.SetBool (Settings.isRunning, isRunning);
        animator.SetInteger(Settings.toolEffect,(int)toolEffect);

        if (isUsingToolRight)
        {
            animator.SetTrigger(Settings.isUsingToolRight);
        }
        if (isUsingToolLeft)
        {
            animator.SetTrigger (Settings.isUsingToolLeft);
        }
        if (isUsingToolUp)
        {
            animator.SetTrigger(Settings.isUsingToolUp);
        }
        if(isUsingToolDown)
        {
            animator.SetTrigger(Settings.isUsingToolDown);
        }

        if(isLiftingToolRight)
        {
            animator.SetTrigger(Settings.isLiftingToolRight);
        }
        if (isLiftingToolLeft)
        {
            animator.SetTrigger(Settings.isLiftingToolLeft);
        }
        if (isLiftingToolUp)
        {
            animator.SetTrigger((Settings.isLiftingToolUp));
        }
        if(isLiftingToolDown)
        {
            animator.SetTrigger ((Settings.isLiftingToolDown));
        }

        if(isPickingRight)
        {
            animator.SetTrigger(Settings.isPickingRight);
        }
        if(isPickingLeft)
        {
            animator.SetTrigger(Settings.isPickingLeft);
        }
        if(isPickingUp)
        {
            animator.SetTrigger(Settings.isPickingUp);
        }
        if(isPickingDown)
        {
            animator.SetTrigger (Settings.isPickingDown);
        }
        if(isSwingingToolRight)
        {
            animator.SetTrigger(Settings.isSwingingToolRight);
        }
        if(isSwingingToolLeft)
        {
            animator.SetTrigger(Settings.isSwingingToolLeft);
        }
        if(isSwingingToolUp)
        {
            animator.SetTrigger(Settings.isSwingingToolUp);
        }
        if(isSwingingToolDown)
        {
            animator.SetTrigger(Settings.isSwingingToolDown);
        }

        if(idleRight)
        {
            animator.SetTrigger((Settings.idleRight));
        }
        if (idleLeft)
        {
            animator.SetTrigger(Settings.idleLeft);
        }
        if (idleUp)
        {
            animator.SetTrigger(Settings.idleUp);
        }
        if(idleDown)
        {
            animator.SetTrigger(Settings.idleDown);
        }
    }

    /// <summary>
    /// 动画事件 - 播放脚步声音
    /// 对应unity中在Animation中设置的event
    /// </summary>
    private void AnimationEventPlayFootstepSound()
    {

    }
}
