using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehvior<SceneControllerManager>
{
    /// <summary>
    /// �Ƿ��ڵ���/����
    /// </summary>
    private bool isFading;
    /// <summary>
    /// ���뵭����ʱ��
    /// </summary>
    [SerializeField] private float fadeDuration = 1f;
    /// <summary>
    /// canvasgroup���
    /// </summary>
    [SerializeField] private CanvasGroup faderCanvasGrooup = null;
    /// <summary>
    /// ����/����ͼƬ
    /// </summary>
    [SerializeField] private Image faderImage = null;
    /// <summary>
    /// ��ʼ��������
    /// </summary>
    public SceneName startingSceneNmae;

    private IEnumerator Start()
    {
        //����loading������������
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGrooup.alpha = 1f;
        //�ȴ���ʼ�����������
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneNmae.ToString()));
        //�������������¼�
        EventHandler.CallAfterSceneLoadEvent();
        //������ǳ
        StartCoroutine(Fade(0f));
    }

    /// <summary>
    /// �����л�������
    /// </summary>
    /// <param name="sceneName">Ҫ�л����ĳ�������</param>
    /// <param name="spawnPosition">���spawnλ��</param>
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        //������δ����/����������ִ���л���������
        if(!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }

    /// <summary>
    /// �л�����
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="spawnPosition"></param>
    /// <returns></returns>
    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        #region ж�س���
        //������������ǰ���¼�
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();
        //loading�𽥺���
        yield return StartCoroutine(Fade(1f));
        //������ҵ�λ��
        Player.Instance.gameObject.transform.position = spawnPosition;
        
        //��������ж��ǰ���¼�
        EventHandler.CallBeforeSceneUnoladEvent();
        //�첽ж�ص�ǰ�����   ������������unity��build settings�п�������
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        #endregion

        #region �����³���
        //��ʼ����ָ���ĳ���
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        //�������������¼�
        EventHandler.CallAfterSceneLoadEvent();
        //loading������
        yield return StartCoroutine(Fade(0f));
        //�����������ص����¼�
        EventHandler.CallAfterSceneLoadFadeInEvent();
        #endregion
    }

    /// <summary>
    /// �м�loading��ɫ������������
    /// </summary>
    /// <param name="finalAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float finalAlpha)
    {
        //��������л�����
        isFading = true;
        //����canvasgroup��ֹ����Ͷ�䵽���·���UIԪ��
        //(ָhierarchy�Ӵ���ͬһ��canvas�µ����壬���Ӵ��и�������ô����Ļ����ʾ������²�)��ʹ���޷�����
        faderCanvasGrooup.blocksRaycasts = true;
        //������ɫ�䵭���ٶ�
        float fadeSpeed = MathF.Abs(faderCanvasGrooup.alpha - finalAlpha) / fadeDuration;
        //ѭ��ֱ���ﵽĿ��͸����
        while(!Mathf.Approximately(faderCanvasGrooup.alpha,finalAlpha))
        {
            //ʹ͸���ȸ�����Ŀ��͸����
            faderCanvasGrooup.alpha = Mathf.MoveTowards(faderCanvasGrooup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            //�ȴ�һ֡Ȼ��ִ�к�������
            yield return null;
        }

        //����л�����
        isFading = false;
        //������
        faderCanvasGrooup.blocksRaycasts = false;
    }

    /// <summary>
    /// ����ָ�����Ƶĳ�������Ϊ�����
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        //���ݳ��������첽���س�������ǰ�ĳ����У�������ֱ���滻��ǰ�ĳ���
        //��Ϊ�־û�������һֱ���ڵ�
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //��ȡ���¼��صĳ�����������ĳ���
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        //�����¼��صĳ�������
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    
}
