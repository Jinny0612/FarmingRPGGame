using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ع���
/// </summary>
public class PoolManager : SingletonMonoBehvior<PoolManager>
{
    /// <summary>
    /// ������ֵ�  key-Ԥ����ʵ��id  value-Ԥ�������
    /// </summary>
    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    /// <summary>
    /// �����   ���ֶ�������ЩԤ������Ҫ������й���
    /// </summary>
    [SerializeField] private Pool[] pool = null;
    /// <summary>
    /// ����ظ����壬���ڹ�����е�������
    /// </summary>
    [SerializeField] private Transform objectPoolTransform = null;


    /// <summary>
    /// �����
    /// </summary>
    [System.Serializable]
    public struct Pool
    {
        /// <summary>
        /// ����صĴ�С
        /// </summary>
        public int poolSize;
        /// <summary>
        /// ��Ϸ�����Ԥ����
        /// </summary>
        public GameObject prefab;
    }
    #region ��Hierarchy�еĲ㼶��ʾ
    //
    // - PoolManager(�������objectPoolTransform)
    //      - parentGameObject(����ΪprefabName + "Anchor")
    //          - prefab1
    //          - prefab2
    //          - prefab3   
    //          - .......
    #endregion

    private void Start()
    {
        // ��ʼ�������
        for(int i = 0; i < pool.Length; i++)
        {
            CreatePool(pool[i].prefab, pool[i].poolSize);
        }
    }

    /// <summary>
    /// ���������
    /// </summary>
    /// <param name="prefab">Ԥ����</param>
    /// <param name="poolSize">����ش�С</param>
    private void CreatePool(GameObject prefab, int poolSize)
    {
        // Ԥ�����ʵ��id
        int poolKey = prefab.GetInstanceID();
        // Ԥ��������
        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        // ���ø�����   ���ڹ���ͬԤ����
        parentGameObject.transform.SetParent(objectPoolTransform);

        if (!poolDictionary.ContainsKey(poolKey))
        {

            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for(int i = 0; i < poolSize; i++)
            {
                // ������Ϸ���壬����Ϊ�ǻ״̬����ʵ����Ϸ���崴��ʱ���޸�Ϊ�״̬
                GameObject newObject = Instantiate(prefab,parentGameObject.transform) as GameObject;
                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue( newObject );
            }
        }

    }

    /// <summary>
    /// ���ö�����еĶ���
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
            // �ӳ��л�ȡ����
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
    /// �Ӷ�����л�ȡ��Ϸ����
    /// </summary>
    /// <param name="poolKey"></param>
    /// <returns></returns>
    private GameObject GetObjectFromPool(int poolKey)
    {
        // ����Ԥ����ʵ��id��ȡ��Ӧ�Ķ��У��Ӷ�����ȡ����Ϸ����
        GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
        // ���¼�������У����ֶ��д�С����
        poolDictionary[poolKey].Enqueue(objectToReuse);

        if (objectToReuse.activeSelf)
        {
            Debug.Log(objectToReuse.name + " is already active!");
            objectToReuse.SetActive(false);
        }

        return objectToReuse;
    }

    /// <summary>
    /// ������Ϸ����
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
