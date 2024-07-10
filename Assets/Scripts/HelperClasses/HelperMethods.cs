using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{

    /// <summary>
    /// 在指定区域内获取所有具有T组件的游戏对象
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="listComponentsAtBoxPosition"></param>
    /// <param name="point">区域中心</param>
    /// <param name="size">区域大小</param>
    /// <param name="angle">区域旋转角度</param>
    /// <returns></returns>
    public static bool GetComponentsAtBoxLocation<T>(out List<T> listComponentsAtBoxPosition, Vector2 point, Vector2 size, float angle)
    {
        bool found = false;
        List<T> componentList = new List<T> ();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(point,size,angle);

        for(int i = 0; i < collider2DArray.Length; i++)
        {
            T tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>();
            if (tComponent != null)
            {
                found = true;
                componentList.Add(tComponent);
            }
            else
            {
                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>();
                if(tComponent != null)
                {
                    found = true;
                    componentList.Add(tComponent);
                }
            }
        }
        listComponentsAtBoxPosition = componentList;

        return found;
    }

    public static T[] GetComponentsAtBoxLocationNonAlloc<T>(int numberOfCollidersToTest, Vector2 point, Vector2 size, float angle)
    {
        Collider2D[] collider2DArray = new Collider2D[numberOfCollidersToTest];
        Physics2D.OverlapBoxNonAlloc(point,size,angle,collider2DArray);

        T tComponent = default(T);

        T[] componentArray = new T[collider2DArray.Length];

        for(int i = collider2DArray.Length - 1; i >= 0; i--)
        {
            if (collider2DArray[i] != null)
            {
                tComponent = collider2DArray[i].gameObject.GetComponent<T>();
                if(tComponent != null)
                {
                    componentArray[i] = tComponent;
                }
            }
        }
        
        return componentArray;
    }

    /// <summary>
    /// 获取指定位置挂载了T组件的游戏对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="componentsAtPositionList"></param>
    /// <param name="positionToCheck"></param>
    /// <returns></returns>
    public static bool GetComponentsAtCursorLocation<T>(out List<T> componentsAtPositionList, Vector3 positionToCheck)
    {
        bool found = false;
        List<T> componentList = new List<T>();

        // Physics2D.OverlapPointAll 检测与该点重合的所有碰撞体。经常用于实现点击检测、射线检测、区域检测
        Collider2D[] collider2DArray = Physics2D.OverlapPointAll(positionToCheck);

        // 遍历所有碰撞体来获取T类型的组件
        T tComponent = default(T);
        for(int i = 0;i < collider2DArray.Length;i++)
        {
            tComponent = collider2DArray[i].gameObject.GetComponentInParent<T>();
            if(tComponent != null)
            {
                found = true;
                componentList.Add(tComponent);
            }
            else
            {
                tComponent = collider2DArray[i].gameObject.GetComponentInChildren<T>();
                if (tComponent != null)
                {
                    found = true;
                    componentList.Add(tComponent);
                }
            }
        }

        componentsAtPositionList = componentList;
        return found;
    }

}
