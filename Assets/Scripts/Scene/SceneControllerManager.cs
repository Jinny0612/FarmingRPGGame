using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �������ƹ���
/// </summary>
public class SceneControllerManager : SingletonMonoBehvior<SceneControllerManager>
{
    /// <summary>
    /// �Ƿ��ڵ���/����
    /// </summary>
    private bool isFading;
    /// <summary>
    /// ���뵭����ʱ��
    /// </summary>
    [SerializeField] private float fadeDuration = 0.5f;
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
    /// <summary>
    /// �ǳ�ʼ�����б�
    /// </summary>
    [SerializeField] private List<SceneName> listNonStartingSceneName = new List<SceneName>();

    private IEnumerator Start()
    {
        //����loading������������
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGrooup.alpha = 1f;

        //Ԥ������������������������ȷ����������������ʼ�Ĳ���Ԥ�Ƽ����ܱ���ȷ����
        //������Ϸ�������ԣ�������س���ʱ��������Ҫ�Ŀ���
        foreach (SceneName sceneName in listNonStartingSceneName)
        {
            //�ȴ������������
            yield return StartCoroutine(LoadSceneAndSetActive(sceneName.ToString()));
            //������������ʱ��
            EventHandler.CallAfterSceneLoadEvent();
            //�ָ������´洢������Ϣ
            SaveLoadManager.Instance.RestoreCurrentSceneData();
            SaveLoadManager.Instance.StoreCurrentSceneData();
            //�첽ж�س���
            yield return SceneManager.UnloadSceneAsync(sceneName.ToString());
        }

        //�ȴ���ʼ�����������
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneNmae.ToString()));
        //�������������¼�
        EventHandler.CallAfterSceneLoadEvent();
        //�ָ���ʼ��������
        SaveLoadManager.Instance.RestoreCurrentSceneData();
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
        //����������룬�޷��ƶ�
        Player.Instance.DisablePlayerInputAndResetMovement();

        //loading�𽥺���
        yield return StartCoroutine(Fade(1f));

        //�洢��ǰ��������
        SaveLoadManager.Instance.StoreCurrentSceneData();

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

        //�ָ��³���������
        SaveLoadManager.Instance.RestoreCurrentSceneData();

        //loading������
        yield return StartCoroutine(Fade(0f));
        //�����������ص����¼�
        EventHandler.CallAfterSceneLoadFadeInEvent();
        //����������룬�����ƶ�
        Player.Instance.EnablePlayerInput();
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
