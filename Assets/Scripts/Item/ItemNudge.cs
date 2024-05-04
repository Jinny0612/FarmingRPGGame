
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ��ɫ·����Ʒʱ��Ʒҡ��Ч������
/// </summary>
public class ItemNudge : MonoBehaviour
{
    /// <summary>
    /// �ȴ�ָ�������ͣЭ��ִ��һ��ʱ��
    /// </summary>
    private WaitForSeconds pause;
    /// <summary>
    /// �Ƿ�ִ�ж�����
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
                //��������ײ����(���)��ߣ���ʱ����ת
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                //��������ײ����(���)�ұߣ�˳ʱ����ת
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
                //��������ײ����(���)�ұߣ���ʱ����ת
                StartCoroutine(RotateAntiClock());
            }
            else
            {
                //��������ײ����(���)��ߣ�˳ʱ����ת
                StartCoroutine(RotateClock());
            }
        }
    }

    /// <summary>
    /// ˳ʱ����ת
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateClock()
    {
        isAnimating = true;//ִ�ж���

        for (int i = 0; i < 4; i++)
        {
            //˳ʱ����ת2��  Χ��z����תʵ�ʾ�����xyƽ���ϲ�����ת
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);
            //ÿ����ת�м䶼����΢����ͣ
            yield return pause;
        }
        //��������ת������һ����Ϊ�˲������õķ���Ч��
        for (int i = 0; i < 5; i++)
        {
            //��ʱ����ת2��
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);
            //ÿ����ת�м䶼����΢����ͣ
            yield return pause;
        }
        //��ת�س�ʼλ��
        gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);
        //��΢��ͣ
        yield return pause;

        isAnimating = false;//����ִ�н���
    }

    /// <summary>
    /// ��ʱ����ת
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateAntiClock()
    {
        isAnimating = true;//ִ�ж���

        for (int i = 0; i < 4; i++)
        {
            //��ʱ����ת2��
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);
            //ÿ����ת�м䶼����΢����ͣ
            yield return pause;
        }
        //��������ת������һ����Ϊ�˲������õķ���Ч��
        for (int i = 0; i <5; i++)
        {
            //˳ʱ����ת2��
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);
            //ÿ����ת�м䶼����΢����ͣ
            yield return pause;
        }
        //��ת�س�ʼλ��
        gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);
        //��΢��ͣ
        yield return pause;

        isAnimating = false;//����ִ�н���
    }
}
