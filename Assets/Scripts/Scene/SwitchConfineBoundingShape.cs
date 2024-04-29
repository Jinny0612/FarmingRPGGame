
using UnityEngine;
using Cinemachine;
using System;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SwitchBoundingShape();
    }

    /// <summary>
    /// �л�cinemachine�ı߽���״���������������Ļ��Ե
    /// </summary>
    private void SwitchBoundingShape()
    {
        //��ȡ��ǩΪ"BoundsConfiner"������˶������ײ���������Ϸ���󣬱������������Ļ��Ե
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
