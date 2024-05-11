using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehvior<SceneControllerManager>
{
    /// <summary>
    /// 是否在淡入/淡出
    /// </summary>
    private bool isFading;
    /// <summary>
    /// 淡入淡出的时间
    /// </summary>
    [SerializeField] private float fadeDuration = 1f;
    /// <summary>
    /// canvasgroup组件
    /// </summary>
    [SerializeField] private CanvasGroup faderCanvasGrooup = null;
    /// <summary>
    /// 淡入/淡出图片
    /// </summary>
    [SerializeField] private Image faderImage = null;
    /// <summary>
    /// 初始场景名称
    /// </summary>
    public SceneName startingSceneNmae;

    private IEnumerator Start()
    {
        //重置loading黑屏场景参数
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGrooup.alpha = 1f;
        //等待初始场景加载完成
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneNmae.ToString()));
        //发布场景加载事件
        EventHandler.CallAfterSceneLoadEvent();
        //黑屏渐浅
        StartCoroutine(Fade(0f));
    }

    /// <summary>
    /// 场景切换主方法
    /// </summary>
    /// <param name="sceneName">要切换到的场景名称</param>
    /// <param name="spawnPosition">玩家spawn位置</param>
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        //场景并未淡入/淡出，可以执行切换场景方法
        if(!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="spawnPosition"></param>
    /// <returns></returns>
    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        #region 卸载场景
        //发布场景淡出前的事件
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();
        //loading逐渐黑屏
        yield return StartCoroutine(Fade(1f));
        //设置玩家的位置
        Player.Instance.gameObject.transform.position = spawnPosition;
        
        //发布场景卸载前的事件
        EventHandler.CallBeforeSceneUnoladEvent();
        //异步卸载当前活动场景   场景的索引在unity的build settings中可以设置
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        #endregion

        #region 加载新场景
        //开始加载指定的场景
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        //发布场景加载事件
        EventHandler.CallAfterSceneLoadEvent();
        //loading逐渐亮屏
        yield return StartCoroutine(Fade(0f));
        //发布场景加载淡入事件
        EventHandler.CallAfterSceneLoadFadeInEvent();
        #endregion
    }

    /// <summary>
    /// 中间loading黑色场景渐隐渐显
    /// </summary>
    /// <param name="finalAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float finalAlpha)
    {
        //标记正在切换场景
        isFading = true;
        //设置canvasgroup阻止射线投射到其下方的UI元素
        //(指hierarchy视窗中同一父canvas下的物体，在视窗中更靠下那么在屏幕中显示会更靠下层)，使其无法交互
        faderCanvasGrooup.blocksRaycasts = true;
        //计算颜色变淡的速度
        float fadeSpeed = MathF.Abs(faderCanvasGrooup.alpha - finalAlpha) / fadeDuration;
        //循环直至达到目标透明度
        while(!Mathf.Approximately(faderCanvasGrooup.alpha,finalAlpha))
        {
            //使透明度更靠近目标透明度
            faderCanvasGrooup.alpha = Mathf.MoveTowards(faderCanvasGrooup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            //等待一帧然后执行后续代码
            yield return null;
        }

        //标记切换结束
        isFading = false;
        //允许交互
        faderCanvasGrooup.blocksRaycasts = false;
    }

    /// <summary>
    /// 加载指定名称的场景并设为活动场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        //根据场景名称异步加载场景至当前的场景中，而不是直接替换当前的场景
        //因为持久化场景是一直存在的
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //获取最新加载的场景，即上面的场景
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        //将最新加载的场景激活
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    
}
