
using UnityEngine;
/// <summary>
/// 存储接口
/// </summary>
public interface ISaveable
{
    /// <summary>
    /// 存储的唯一标识
    /// </summary>
    string ISaveableUniqueId { get; set; }

    /// <summary>
    /// 存储的信息
    /// </summary>
    GameObjectSave GameObjectSave { get; set; }

    /// <summary>
    /// 内容注册
    /// </summary>
    void ISaveableRegister();
    /// <summary>
    /// 内容注销
    /// </summary>
    void ISaveableDeregister();
    /// <summary>
    /// 存储指定场景名称的场景信息
    /// </summary>
    /// <param name="sceneName"></param>
    void ISaveableStoreScene(string sceneName);
    /// <summary>
    /// 恢复指定场景名称的场景信息
    /// </summary>
    /// <param name="sceneName"></param>
    void ISaveableRestoreScene(string sceneName);
}
