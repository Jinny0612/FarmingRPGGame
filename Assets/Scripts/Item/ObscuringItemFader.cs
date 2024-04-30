using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 遮挡玩家的物体颜色变化管理
/// requirecompent保证加载这个脚本的游戏物体一定有某个组件，如果没有会自动将这个组件添加到游戏物体上
/// 这个脚本需要添加到所有需要的预制件中
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ObscuringItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 渐隐
    /// </summary>
    public void FadeOut()
    {
        //开启一个协程
        //协程通常更适合处理需要延迟执行或等待条件满足的异步操作
        StartCoroutine(FadeOutRoutine());
    }

    /// <summary>
    /// 渐显
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine (FadeInRoutine());
    }

    /// <summary>
    /// 游戏物体渐隐的协程函数
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutRoutine()
    {
        //获取物体当前alpha（透明度）
        float currentAlpha = spriteRenderer.color.a;
        //当前alpha与目标alpha之间的差距，根据这个逐帧渐隐
        float distance = currentAlpha - Settings.targetAlpha;

        while(currentAlpha - Settings.targetAlpha > 0.01f)
        {
            //未达到目标透明度，执行循环
            //设定Settings.fadeOutSeconds时间内达到目标透明度，
            //因此每帧应该减少的透明度为distance / Settings.fadeOutSeconds * Time.deltaTime
            currentAlpha = currentAlpha - distance / Settings.fadeOutSeconds * Time.deltaTime;
            //使透明度平滑过渡，之后对比一下试一试
            //currentAlpha = Mathf.Lerp(currentAlpha, Settings.targetAlpha, Time.deltaTime / Settings.fadeOutSeconds);
            //更新游戏物体透明度
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,currentAlpha);
            
            yield return null;
        }
        //最终设置为目标透明度
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Settings.targetAlpha);
    }

    /// <summary>
    /// 游戏物体渐显的协程函数
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInRoutine()
    {
        //获取物体当前alpha（透明度）
        float currentAlpha = spriteRenderer.color.a;
        //需要回到初始透明度1f
        float distance = 1f - currentAlpha;

        while (1f - currentAlpha  > 0.01f)
        {
            //未达到目标透明度，执行循环
            //设定Settings.fadeInSeconds时间内达到目标透明度，
            //因此每帧应该增加的透明度为distance / Settings.fadeOutSeconds * Time.deltaTime
            currentAlpha = currentAlpha + distance / Settings.fadeInSeconds * Time.deltaTime;
            //更新游戏物体透明度
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,currentAlpha);
            
            yield return null;
        }
        //最终设置为初始透明度1f
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }


}
