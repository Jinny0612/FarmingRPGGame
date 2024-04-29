
using UnityEngine;

/// <summary>
/// 所有可以添加到游戏物体上的游戏脚本的单例抽象类
/// </summary>
public abstract class SingletonMonoBehvior<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 实例
    /// </summary>
    private static T instance;

    /// <summary>
    /// 获取实例
    /// </summary>
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    
    /// <summary>
    /// 单例的初始化
    /// 可被继承类覆盖
    /// Awake方法在游戏对象的实例被创建时调用，即使脚本组件被禁用，也会执行Awake方法
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
