using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{

    /// <summary>
    /// 在指定区域内获取所哟具有T组件的游戏对象
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

}
