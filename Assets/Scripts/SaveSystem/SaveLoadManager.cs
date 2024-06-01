using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 存储&读取管理
/// </summary>
public class SaveLoadManager : SingletonMonoBehvior<SaveLoadManager>
{
    /// <summary>
    /// 需要存储的信息集合
    /// </summary>
    public List<ISaveable> iSaveableObjectList;

    protected override void Awake()
    {
        base.Awake();

        iSaveableObjectList = new List<ISaveable>();
    }

    /// <summary>
    /// 存储当前场景数据
    /// </summary>
    public void StoreCurrentSceneData()
    {
        //遍历需要存储的信息集合，存储场景信息
        foreach(ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// 恢复当前场景数据
    /// </summary>
    public void RestoreCurrentSceneData()
    {
        //遍历需要存储的信息集合，恢复场景信息
        foreach(ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }

}
