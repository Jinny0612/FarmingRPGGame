using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// �ڵ���ҵ�������ɫ�仯����
/// requirecompent��֤��������ű�����Ϸ����һ����ĳ����������û�л��Զ�����������ӵ���Ϸ������
/// ����ű���Ҫ��ӵ�������Ҫ��Ԥ�Ƽ���
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
    /// ����
    /// </summary>
    public void FadeOut()
    {
        //����һ��Э��
        //Э��ͨ�����ʺϴ�����Ҫ�ӳ�ִ�л�ȴ�����������첽����
        StartCoroutine(FadeOutRoutine());
    }

    /// <summary>
    /// ����
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine (FadeInRoutine());
    }

    /// <summary>
    /// ��Ϸ���彥����Э�̺���
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutRoutine()
    {
        //��ȡ���嵱ǰalpha��͸���ȣ�
        float currentAlpha = spriteRenderer.color.a;
        //��ǰalpha��Ŀ��alpha֮��Ĳ�࣬���������֡����
        float distance = currentAlpha - Settings.targetAlpha;

        while(currentAlpha - Settings.targetAlpha > 0.01f)
        {
            //δ�ﵽĿ��͸���ȣ�ִ��ѭ��
            //�趨Settings.fadeOutSecondsʱ���ڴﵽĿ��͸���ȣ�
            //���ÿ֡Ӧ�ü��ٵ�͸����Ϊdistance / Settings.fadeOutSeconds * Time.deltaTime
            currentAlpha = currentAlpha - distance / Settings.fadeOutSeconds * Time.deltaTime;
            //ʹ͸����ƽ�����ɣ�֮��Ա�һ����һ��
            //currentAlpha = Mathf.Lerp(currentAlpha, Settings.targetAlpha, Time.deltaTime / Settings.fadeOutSeconds);
            //������Ϸ����͸����
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,currentAlpha);
            
            yield return null;
        }
        //��������ΪĿ��͸����
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Settings.targetAlpha);
    }

    /// <summary>
    /// ��Ϸ���彥�Ե�Э�̺���
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInRoutine()
    {
        //��ȡ���嵱ǰalpha��͸���ȣ�
        float currentAlpha = spriteRenderer.color.a;
        //��Ҫ�ص���ʼ͸����1f
        float distance = 1f - currentAlpha;

        while (1f - currentAlpha  > 0.01f)
        {
            //δ�ﵽĿ��͸���ȣ�ִ��ѭ��
            //�趨Settings.fadeInSecondsʱ���ڴﵽĿ��͸���ȣ�
            //���ÿ֡Ӧ�����ӵ�͸����Ϊdistance / Settings.fadeOutSeconds * Time.deltaTime
            currentAlpha = currentAlpha + distance / Settings.fadeInSeconds * Time.deltaTime;
            //������Ϸ����͸����
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,currentAlpha);
            
            yield return null;
        }
        //��������Ϊ��ʼ͸����1f
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }


}
