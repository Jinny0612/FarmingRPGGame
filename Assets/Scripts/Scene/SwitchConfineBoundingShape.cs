
using UnityEngine;
using Cinemachine;
using System;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    private void OnEnable()
    {
        //�����ڳ���������ɺ��ټ���
        EventHandler.AfterSceneLoadEvent += SwitchBoundingShape;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;
    }

    /// <summary>
    /// �л�cinemachine�ı߽���״���������������Ļ��Ե
    /// </summary>
    private void SwitchBoundingShape()
    {
        //��ȡ��ǩΪ"BoundsConfiner"������˶������ײ���������Ϸ���󣬱������������Ļ��Ե
        //�ڳ���Scene1_Farm�У���Ҫ�ȴ�����������ɺ���ܻ�ȡ
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();
        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();
        //���ñ߽���״ΪBoundsConfinder��ǩ��Ӧ��Ϸ�����趨����״
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        //һ����˵���������������仯ʱ��CinemachineConfiner ������Զ���Ⲣ����·�����棬�����ֶ����ø÷�����
        //����һЩ��������£�����������ʱ��̬�ı����������������Ҫ������Чʱ��
        //�����ֶ����� InvalidatePathCache() ����������·���������Ч������ȷ�������������ȷ�Ժ�ʵʱ�ԡ�
        cinemachineConfiner.InvalidatePathCache();
    }
}
