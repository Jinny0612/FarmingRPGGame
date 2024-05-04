
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 角色路过物品时物品摇晃效果控制
/// </summary>
public class ItemNudge : MonoBehaviour
{
    /// <summary>
    /// 等待指令，用于暂停协程执行一段时间
    /// </summary>
    private WaitForSeconds pause;
    /// <summary>
    /// 是否执行动画中
    /// </summary>
    private bool isAnimating = false;

    private void Awake()
    {
        pause = new WaitForSeconds(0.04f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAnimating)
        {
            if(gameObject.transform.position.x < collision.gameObject.transform.position.x)
            {
                //物体在碰撞物体(玩家)左边，逆时针旋转
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                //物体在碰撞物体(玩家)右边，顺时针旋转
                StartCoroutine(RotateClock());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAnimating)
        {
            if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
            {
                //物体在碰撞物体(玩家)右边，逆时针旋转
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                //物体在碰撞物体(玩家)左边，顺时针旋转
                StartCoroutine(RotateClock());
            }
        }
    }

    /// <summary>
    /// 顺时针旋转
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateClock()
    {
        isAnimating = true;//执行动画

        for (int i = 0; i < 4; i++)
        {
            //顺时针旋转2°  围绕z轴旋转实际就是在xy平面上产生旋转
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);
            //每次旋转中间都有轻微的暂停
            yield return pause;
        }
        //比上面旋转次数多一次是为了产生更好的反弹效果
        for (int i = 0; i < 5; i++)
        {
            //逆时针旋转2°
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);
            //每次旋转中间都有轻微的暂停
            yield return pause;
        }
        //旋转回初始位置
        gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);
        //轻微暂停
        yield return pause;

        isAnimating = false;//动画执行结束
    }

    /// <summary>
    /// 逆时针旋转
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateAntiClock()
    {
        isAnimating = true;//执行动画

        for (int i = 0; i < 4; i++)
        {
            //逆时针旋转2°
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);
            //每次旋转中间都有轻微的暂停
            yield return pause;
        }
        //比上面旋转次数多一次是为了产生更好的反弹效果
        for (int i = 0; i <5; i++)
        {
            //顺时针旋转2°
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);
            //每次旋转中间都有轻微的暂停
            yield return pause;
        }
        //旋转回初始位置
        gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);
        //轻微暂停
        yield return pause;

        isAnimating = false;//动画执行结束
    }
}
