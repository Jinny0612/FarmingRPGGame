using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网格属性管理
/// </summary>
[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonoBehvior<GridPropertiesManager>, ISaveable
{
    public Grid grid;
    /// <summary>
    /// 网格属性详情字典
    /// </summary>
    private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;
    /// <summary>
    /// 网格属性数组，scriptableobjectassets中的SO_GridProperties
    /// 当前脚本就是对这个数组进行管理
    /// </summary>
    [SerializeField] private SO_GridProperties[] so_gridPropertiesArry = null;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueId { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }
    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueId = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        //内容注册并订阅事件
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
    }

    private void OnDisable()
    {
        //内容注销并取消订阅
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
    }

    private void Start()
    {
        InitialiseGridProperties();
    }

    /// <summary>
    /// 从SO_GridProperties资源中实例化网格属性字典
    /// </summary>
    private void InitialiseGridProperties()
    {
        //读取所有SO_GridProperties资源信息
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArry)
        {
            //网格属性详情字典
            Dictionary<string, GridPropertyDetails> gridPropertyDictionary = new Dictionary<string, GridPropertyDetails>();
            //读取每个场景网格属性集合信息并填充进入上面字典中
            foreach(GridProperty gridProperty in so_GridProperties.gridPropertyList)
            {
                GridPropertyDetails gridPropertyDetails;
                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDictionary);
                if (gridPropertyDetails == null)
                {
                    gridPropertyDetails = new GridPropertyDetails();
                }
                switch(gridProperty.gridBoolProperty)
                {
                    case GridBoolProperty.diggable:
                        gridPropertyDetails.isDiggable = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.canDropItem:
                        gridPropertyDetails.canDropItem = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.canPlaceFurniture:
                        gridPropertyDetails.canPlaceFurniture = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.isPath:
                        gridPropertyDetails.isPath = gridProperty.gridBoolValue; break;
                    case GridBoolProperty.isNPCObstacle:
                        gridPropertyDetails.isNPCObstacle = gridProperty.gridBoolValue; break;
                    default: break;
                }
                //填充网格详情
                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetails, gridPropertyDictionary);
            }
            //为当前游戏物体（挂载了当前脚本的物体）创建场景存储信息
            SceneSave sceneSave = new SceneSave();
            sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

            if(so_GridProperties.sceneName.ToString().Equals(SceneControllerManager.Instance.startingSceneNmae.ToString()))
            {
                this.gridPropertyDictionary = gridPropertyDictionary;
            }
            //添加场景数据
            GameObjectSave.sceneData.Add(so_GridProperties.sceneName.ToString(), sceneSave);
        }
    }

    private string constructKeyForCoordinate(int gridX,int gridY)
    {
        return "x" + gridX + "y" + gridY;
    }

    /// <summary>
    /// 根据坐标和网格属性详情添加至字典中   用于初始化
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="gridPropertyDetails"></param>
    /// <param name="gridPropertyDictionary"></param>
    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        //根据坐标构建字典的key
        string key = constructKeyForCoordinate(x, y);

        gridPropertyDetails.gridX = x;
        gridPropertyDetails.gridY = y;

        if (gridPropertyDictionary.ContainsKey(key))
        {
            //当前网格已存在，重写属性
            gridPropertyDetails.isDiggable = gridPropertyDictionary[key].isDiggable || gridPropertyDetails.isDiggable;
            gridPropertyDetails.canPlaceFurniture = gridPropertyDictionary[key].canPlaceFurniture || gridPropertyDetails.canPlaceFurniture;
            gridPropertyDetails.canDropItem = gridPropertyDictionary[key].canDropItem || gridPropertyDetails.canDropItem;
            gridPropertyDetails.isPath = gridPropertyDictionary[key].isPath || gridPropertyDetails.isPath;
            gridPropertyDetails.isNPCObstacle = gridPropertyDictionary[key].isNPCObstacle || gridPropertyDetails.isNPCObstacle;

        }
        gridPropertyDictionary[key] = gridPropertyDetails;
    }

    public void SetGridPropertyDetails(int gridX,int gridY, GridPropertyDetails gridPropertyDetails)
    {
        SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertyDictionary);
    }

    /// <summary>
    /// 根据坐标和网格详情字典获取具体的网格详细信息
    /// </summary>
    /// <param name="gridX">网格坐标x</param>
    /// <param name="gridY">网格坐标y</param>
    /// <param name="gridPropertyDictionary">网格信息字典</param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyDetails> gridPropertyDictionary)
    {
        //根据坐标构建字典的key
        string key = constructKeyForCoordinate(gridX, gridY);
        GridPropertyDetails gridPropertyDetails;
        //获取坐标对应的网格详情
        if (!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails))
        {
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }

    /// <summary>
    /// 根据网格坐标获取网格详情
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int gridX,int gridY)
    {
        return GetGridPropertyDetails(gridX,gridY,gridPropertyDictionary);
    }

    /// <summary>
    /// 在场景加载完成后执行
    /// </summary>
    private void AfterSceneLoaded()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        //实例化场景信息
        if(GameObjectSave.sceneData.TryGetValue(sceneName,out SceneSave sceneSave))
        {
            if(sceneSave.gridPropertyDetailsDictionary != null)
            {
                gridPropertyDictionary = sceneSave.gridPropertyDetailsDictionary;
            }
        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
        //存储场景信息
        GameObjectSave.sceneData.Remove(sceneName);

        SceneSave sceneSave = new SceneSave();

        sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }
}
