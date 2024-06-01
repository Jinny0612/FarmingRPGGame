using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 场景物品管理
/// </summary>
[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : SingletonMonoBehvior<SceneItemsManager>, ISaveable
{
    /// <summary>
    /// 父级物品信息
    /// </summary>
    private Transform parentItem;
    /// <summary>
    /// 预制体
    /// </summary>
    [SerializeField] private GameObject itemPerfab = null;

    /// <summary>
    /// 唯一存储标识
    /// </summary>
    private string _iSaveableUniqueID;

    public string ISaveableUniqueId { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    /// <summary>
    /// 游戏物体存储信息
    /// </summary>
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    /// <summary>
    /// 场景加载后需要处理的逻辑
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
    /// 销毁场景内的物品
    /// </summary>
    private void DestroySceneItems()
    {
        //获取场景内所有物品
        Item[] itemsInScene = GameObject.FindObjectsByType<Item>(FindObjectsSortMode.None);

        //销毁所有物品
        for(int i = itemsInScene.Length - 1 ; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }

    /// <summary>
    /// 实例化场景物品(单个)
    /// </summary>
    /// <param name="itemCode">物品代码</param>
    /// <param name="itemPosition">物品位置</param>
    public void InstantiateSceneItem(int itemCode, Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPerfab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemCode);
    }

    /// <summary>
    /// 实例化一组场景物品
    /// </summary>
    /// <param name="sceneItemList">场景物品信息集合</param>
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
    /// 接口注销
    /// </summary>
    public void ISaveableDeregister()
    {
        //移除当前sceneitemmanager
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    /// <summary>
    /// 接口注册
    /// </summary>
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    /// <summary>
    /// 恢复场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableRestoreScene(string sceneName)
    {
        //恢复指定名称的场景
        if(GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if(sceneSave.listSceneItem != null)
            {
                //销毁物品
                DestroySceneItems();
                //实例化物品集合
                InstantiateSceneItems(sceneSave.listSceneItem);
            }
        }
    }

    /// <summary>
    /// 存储场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableStoreScene(string sceneName)
    {
        //先清除场景数据信息
        GameObjectSave.sceneData.Remove(sceneName);

        //获取场景内的所有物品
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

        //创建场景信息字典
        SceneSave sceneSave = new SceneSave();
        sceneSave.listSceneItem = sceneItemList;

        //将场景存储信息添加至游戏物体存储信息字典中
        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }
}
