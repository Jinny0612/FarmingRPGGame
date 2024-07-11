using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理
/// </summary>
public class PoolManager : SingletonMonoBehvior<PoolManager>
{
    /// <summary>
    /// 对象池字典  key-预制体实例id  value-预制体队列
    /// </summary>
    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    /// <summary>
    /// 对象池   可手动设置哪些预制体需要放入池中管理
    /// </summary>
    [SerializeField] private Pool[] pool = null;
    /// <summary>
    /// 对象池父物体，便于管理池中的子物体
    /// </summary>
    [SerializeField] private Transform objectPoolTransform = null;


    /// <summary>
    /// 对象池
    /// </summary>
    [System.Serializable]
    public struct Pool
    {
        /// <summary>
        /// 对象池的大小
        /// </summary>
        public int poolSize;
        /// <summary>
        /// 游戏物体的预制体
        /// </summary>
        public GameObject prefab;
    }
    #region 在Hierarchy中的层级显示
    //
    // - PoolManager(即上面的objectPoolTransform)
    //      - parentGameObject(名称为prefabName + "Anchor")
    //          - prefab1
    //          - prefab2
    //          - prefab3   
    //          - .......
    #endregion

    private void Start()
    {
        // 初始化对象池
        for(int i = 0; i < pool.Length; i++)
        {
            CreatePool(pool[i].prefab, pool[i].poolSize);
        }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="poolSize">对象池大小</param>
    private void CreatePool(GameObject prefab, int poolSize)
    {
        // 预制体的实例id
        int poolKey = prefab.GetInstanceID();
        // 预制体名称
        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        // 设置父物体   便于管理不同预制体
        parentGameObject.transform.SetParent(objectPoolTransform);

        if (!poolDictionary.ContainsKey(poolKey))
        {

            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for(int i = 0; i < poolSize; i++)
            {
                // 创建游戏物体，设置为非活动状态，在实际游戏物体创建时会修改为活动状态
                GameObject newObject = Instantiate(prefab,parentGameObject.transform) as GameObject;
                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue( newObject );
            }
        }

    }

    /// <summary>
    /// 重用对象池中的对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject ReuseObject(GameObject prefab,Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            // 从池中获取对象
            GameObject objectToReuse = GetObjectFromPool(poolKey);

            ResetObject(position, rotation, objectToReuse, prefab);

            return objectToReuse;
        }
        else
        {
            Debug.Log("No object pool for " + prefab);
            return null;
        }
    }

    /// <summary>
    /// 从对象池中获取游戏物体
    /// </summary>
    /// <param name="poolKey"></param>
    /// <returns></returns>
    private GameObject GetObjectFromPool(int poolKey)
    {
        // 根据预制体实例id获取相应的队列，从队列中取出游戏对象
        GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
        // 重新加入队列中，保持队列大小不变
        poolDictionary[poolKey].Enqueue(objectToReuse);

        if (objectToReuse.activeSelf)
        {
            Debug.Log(objectToReuse.name + " is already active!");
            objectToReuse.SetActive(false);
        }

        return objectToReuse;
    }

    /// <summary>
    /// 重置游戏对象
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="objectToReuse"></param>
    /// <param name="prefab"></param>
    private void ResetObject(Vector3 position, Quaternion rotation, GameObject objectToReuse, GameObject prefab)
    {
        objectToReuse.transform.position = position;
        objectToReuse.transform.rotation = rotation;

        objectToReuse.transform.localScale = prefab.transform.localScale;
    }
}
