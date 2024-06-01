using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �洢&��ȡ����
/// </summary>
public class SaveLoadManager : SingletonMonoBehvior<SaveLoadManager>
{
    /// <summary>
    /// ��Ҫ�洢����Ϣ����
    /// </summary>
    public List<ISaveable> iSaveableObjectList;

    protected override void Awake()
    {
        base.Awake();

        iSaveableObjectList = new List<ISaveable>();
    }

    /// <summary>
    /// �洢��ǰ��������
    /// </summary>
    public void StoreCurrentSceneData()
    {
        //������Ҫ�洢����Ϣ���ϣ��洢������Ϣ
        foreach(ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// �ָ���ǰ��������
    /// </summary>
    public void RestoreCurrentSceneData()
    {
        //������Ҫ�洢����Ϣ���ϣ��ָ�������Ϣ
        foreach(ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }

}
