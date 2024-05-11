
using UnityEngine;
using Cinemachine;
using System;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    private void OnEnable()
    {
        //必须在场景加载完成后再加载
        EventHandler.AfterSceneLoadEvent += SwitchBoundingShape;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;
    }

    /// <summary>
    /// 切换cinemachine的边界形状，避免相机超出屏幕边缘
    /// </summary>
    private void SwitchBoundingShape()
    {
        //获取标签为"BoundsConfiner"的添加了多边形碰撞器组件的游戏对象，避免相机超出屏幕边缘
        //在场景Scene1_Farm中，需要等待场景加载完成后才能获取
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();
        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();
        //设置边界形状为BoundsConfinder标签对应游戏物体设定的形状
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        //一般来说，当限制区域发生变化时，CinemachineConfiner 组件会自动检测并更新路径缓存，无需手动调用该方法。
        //但在一些特殊情况下，比如在运行时动态改变了限制区域或者需要立即生效时，
        //可以手动调用 InvalidatePathCache() 方法来触发路径缓存的无效化，以确保限制区域的正确性和实时性。
        cinemachineConfiner.InvalidatePathCache();
    }
}
