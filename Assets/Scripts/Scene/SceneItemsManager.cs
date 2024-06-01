using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ������Ʒ����
/// </summary>
[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : SingletonMonoBehvior<SceneItemsManager>, ISaveable
{
    /// <summary>
    /// ������Ʒ��Ϣ
    /// </summary>
    private Transform parentItem;
    /// <summary>
    /// Ԥ����
    /// </summary>
    [SerializeField] private GameObject itemPerfab = null;

    /// <summary>
    /// Ψһ�洢��ʶ
    /// </summary>
    private string _iSaveableUniqueID;

    public string ISaveableUniqueId { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    /// <summary>
    /// ��Ϸ����洢��Ϣ
    /// </summary>
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    /// <summary>
    /// �������غ���Ҫ������߼�
    /// </summary>
    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
    }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueId = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();

    }

    /// <summary>
    /// ���ٳ����ڵ���Ʒ
    /// </summary>
    private void DestroySceneItems()
    {
        //��ȡ������������Ʒ
        Item[] itemsInScene = GameObject.FindObjectsByType<Item>(FindObjectsSortMode.None);

        //����������Ʒ
        for(int i = itemsInScene.Length - 1 ; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }

    /// <summary>
    /// ʵ����������Ʒ(����)
    /// </summary>
    /// <param name="itemCode">��Ʒ����</param>
    /// <param name="itemPosition">��Ʒλ��</param>
    public void InstantiateSceneItem(int itemCode, Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPerfab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemCode);
    }

    /// <summary>
    /// ʵ����һ�鳡����Ʒ
    /// </summary>
    /// <param name="sceneItemList">������Ʒ��Ϣ����</param>
    private void InstantiateSceneItems(List<SceneItem> sceneItemList)
    {
        GameObject itemGameObject;
        foreach(SceneItem sceneItem in sceneItemList)
        {
            itemGameObject = Instantiate(itemPerfab, new Vector3(sceneItem.position.x, sceneItem.position.y, sceneItem.position.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = sceneItem.itemCode;
            item.name = sceneItem.itemName;
        }
    }

    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    /// <summary>
    /// �ӿ�ע��
    /// </summary>
    public void ISaveableDeregister()
    {
        //�Ƴ���ǰsceneitemmanager
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    /// <summary>
    /// �ӿ�ע��
    /// </summary>
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    /// <summary>
    /// �ָ�����
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableRestoreScene(string sceneName)
    {
        //�ָ�ָ�����Ƶĳ���
        if(GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if(sceneSave.listSceneItem != null)
            {
                //������Ʒ
                DestroySceneItems();
                //ʵ������Ʒ����
                InstantiateSceneItems(sceneSave.listSceneItem);
            }
        }
    }

    /// <summary>
    /// �洢����
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableStoreScene(string sceneName)
    {
        //���������������Ϣ
        GameObjectSave.sceneData.Remove(sceneName);

        //��ȡ�����ڵ�������Ʒ
        List<SceneItem> sceneItemList = new List<SceneItem>();
        Item[] itemInScene = FindObjectsOfType<Item>();

        foreach (Item item in itemInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemCode = item.ItemCode;
            sceneItem.position = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
            sceneItem.itemName = item.name;

            sceneItemList.Add(sceneItem);
        }

        //����������Ϣ�ֵ�
        SceneSave sceneSave = new SceneSave();
        sceneSave.listSceneItem = sceneItemList;

        //�������洢��Ϣ�������Ϸ����洢��Ϣ�ֵ���
        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }
}
